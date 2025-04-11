using System.Security.Authentication;
using System.Text.Json;
using AccidentMonitor.Application.Accident.Dtos;
using AccidentMonitor.Application.BlockPolygon.Mappers;
using AccidentMonitor.Application.Common.Interfaces;
using AccidentMonitor.Application.Common.Results;
using AccidentMonitor.Application.ORService.Dto;
using AccidentMonitor.Application.ORService.ExtensionMappers;
using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;
using AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;
using AccidentMonitor.Domain.Entities.Accident;
using AccidentMonitor.Infrastructure.MQTT.MQTTMessage.Request;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Formatter;
using MQTTnet.Protocol;


namespace AccidentMonitor.Infrastructure.MQTT
{
    // TODO: Refactor: remove IORService from this to clean code and make it more testable
    public class MqttClientService : IMqttService, IAsyncDisposable, IDisposable
    {
        private readonly IMqttClient _mqttClient;
        private readonly ILogger _logger;
        private readonly MqttConnectionConfiguration _config;
        private readonly MqttClientFactory _mqttClientFactory;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IServiceProvider _serviceProvider;
        private bool _isDisposed;
        public event Action<string, string>? OnMessageReceived;

        private const int ReconnectDelay = 5;

        public MqttClientService(
            MqttConnectionConfiguration options,
            ILogger<MqttClientService> logger,
            IServiceProvider serviceProvider
            )
        {
            _config = options;
            _logger = logger;
            _mqttClientFactory = new MqttClientFactory();
            _mqttClient = _mqttClientFactory.CreateMqttClient();
            _cancellationTokenSource = new CancellationTokenSource();
            _serviceProvider = serviceProvider;
            ConfigureClientHandlers();
        }
        private void ConfigureClientHandlers()
        {
            _mqttClient.DisconnectedAsync += HandleDisconnectionAsync;
            _mqttClient.ApplicationMessageReceivedAsync += HandleMessageAsync;
        }
        public async Task<ServiceResult> StartAsync()
        {
            try
            {
                await ConnectAsync();
                if (!_mqttClient.IsConnected)
                {
                    _logger.LogError("Failed to start MQTT client");
                    return new ServiceResult(-1, "Failed to start MQTT client");
                }
                await SubscribeAsync();
                _logger.LogInformation("MQTT Client started successfully");
                return new ServiceResult(0, "MQTT Client started successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start MQTT client");
                throw;
            }
        }
        public async Task<ServiceResult> ConnectAsync()
        {
            var uri = $"{_config.Broker}:{_config.Port}/{_config.Protocol}";
            var builder = new MqttClientOptionsBuilder()
                .WithClientId(_config.ClientID)
                //.WithTcpServer(_config.Broker, _config.Port)
                .WithProtocolVersion((MqttProtocolVersion)_config.ProtocolVersion)
                .WithTimeout(TimeSpan.FromSeconds(5))
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .WithCredentials(_config.Username, _config.Password);

            builder = _config.UseWebSocket
                ? builder.WithWebSocketServer(o => o.WithUri($"ws://{uri}"))
                : builder.WithTcpServer(_config.Broker, _config.Port);

            if (_config.UseTls)
            {
                //if (string.IsNullOrWhiteSpace(_config.CertPath))
                //{
                //    _logger.LogError("Certificate path is not provided.");
                //    throw new ArgumentException("Certificate path must be provided", nameof(_config.CertPath));
                //}

                try
                {
                    //var cert = X509CertificateLoader.LoadCertificateFromFile(_config.CertPath);

                    builder = _config.TrustChain == null
                        ? builder.WithTlsOptions(o =>
                        {
                            o.WithCertificateValidationHandler(_ => true);
                            o.WithSslProtocols(SslProtocols.Tls12);
                            //o.WithClientCertificates(new[] { cert });
                        })
                        : builder.WithTlsOptions(new MqttClientTlsOptionsBuilder()
                            .WithTrustChain(_config.TrustChain)
                            .Build());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load certificate from path: {path}", _config.CertPath);
                    throw;
                }
            }

            var response = new MqttClientConnectResult();
            var clientOptions = builder.Build();
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    _logger.LogInformation("Attempting to connect to MQTT broker at {Server}:{Port}",
                        _config.Broker, _config.Port);
                    response = await _mqttClient.ConnectAsync(clientOptions, _cancellationTokenSource.Token);
                    _logger.LogInformation("Connected to MQTT broker successfully");
                }
            }
            catch (MqttConnectingFailedException ex)
            {
                _logger.LogError(ex, "Authentication failed when connecting to MQTT broker. Please verify credentials.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to connect to MQTT broker");
                throw;
            }

            return new ServiceResult(
                Convert.ToInt32(response.ResultCode),
                ""
            );

        }

        public async Task DisconnectAsync()
        {
            var disconnectOptions = _mqttClientFactory.CreateClientDisconnectOptionsBuilder().Build();
            await _mqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None);
            Console.WriteLine("The MQTT client is disconnected.");
        }

        private async Task HandleDisconnectionAsync(MqttClientDisconnectedEventArgs args)
        {
            if (_isDisposed) return;

            _logger.LogWarning("Disconnected from MQTT broker: {Reason}", args.Reason);
            while (!_mqttClient.IsConnected && !_isDisposed)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(ReconnectDelay), _cancellationTokenSource.Token);
                    await ConnectAsync();

                    if (_mqttClient.IsConnected)
                    {
                        await SubscribeAsync();
                        _logger.LogInformation("Successfully reconnected to MQTT broker");
                        break;
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, "Failed to reconnect to MQTT broker: ");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to reconnect to MQTT broker");
                }
            }
        }

        public async Task<ServiceResult> SubscribeAsync()
        {
            var subscribeOptions = _mqttClientFactory.CreateSubscribeOptionsBuilder();

            foreach (var topic in _config.SubTopics)
            {
                var topicFilter = topic.Contains("{id}") ? topic.Replace("{id}", "+") : topic;

                subscribeOptions = subscribeOptions.WithTopicFilter(topicFilter, MqttQualityOfServiceLevel.AtLeastOnce);
            }

            var builtSubscribeOptions = subscribeOptions.Build();
            try
            {
                var response = await _mqttClient.SubscribeAsync(builtSubscribeOptions, CancellationToken.None);
                _logger.LogInformation("Subscribed to topics successfully");
                return new ServiceResult(0, response.ReasonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe to topics");
                return new ServiceResult(-1, $"Subscribe failed: {ex.Message}");
            }
        }

        public async Task<ServiceResult> UnsubscribeAsync(string topic)
        {
            var unsubscribeOptions = _mqttClientFactory.CreateUnsubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();

            try
            {
                await _mqttClient.UnsubscribeAsync(unsubscribeOptions, CancellationToken.None);
                _logger.LogInformation("Unsubscribed from topic: {Topic}", topic);
                return new ServiceResult(0, $"Unsubscribed from topic {topic}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unsubscribe failed for topic {Topic}", topic);
                return new ServiceResult(-1, $"Unsubscribe failed: {ex.Message}");
            }
        }

        private async Task HandleMessageAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            try
            {
                var topic = args.ApplicationMessage.Topic;
                var payload = args.ApplicationMessage.ConvertPayloadToString();

                // Raise the event with the received message
                OnMessageReceived?.Invoke(topic, payload);
                switch (topic)
                {
                    case var t when t.StartsWith(_config.SubTopics[0]):
                        //HandleHumidity(payload);
                        break;
                    case var t when t.StartsWith(_config.SubTopics[1].Split('{')[0]):
                        var topicId = TopicParser.ExtractId(_config.SubTopics[1], t)!;
                        await HandleAccidentReport(topicId, payload, _cancellationTokenSource.Token);
                        break;
                    case var t when t.StartsWith(_config.SubTopics[2].Split('{')[0]):
                        topicId = TopicParser.ExtractId(_config.SubTopics[2], t)!;
                        await HandleDirectionRequestAsync(topicId, payload);
                        break;
                    case var t when t.StartsWith(_config.SubTopics[3].Split('{')[0]):
                        topicId = TopicParser.ExtractId(_config.SubTopics[3], t)!;
                        await HandleAdvancedDirectionRequestAsync(topicId, payload);
                        break;
                    default:
                        _logger.LogWarning("Unhandled topic: {Topic}", topic);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MQTT message");
            }

        }

        public async Task<ServiceResult> PublishAsync<T>(string topic, T dto)
        {

            string jsonPayload = JsonSerializer.Serialize(dto);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(jsonPayload)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel)_config.QoS)
                .WithRetainFlag()
                .Build();
            var response = await _mqttClient.PublishAsync(message, CancellationToken.None);
            return new ServiceResult(
                Convert.ToInt32(response.ReasonCode),
                response.ReasonString
            );
        }

        public void Dispose() => DisposeAsync().AsTask().Wait();

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed) return;

            _isDisposed = true;

            try
            {
                _cancellationTokenSource.Cancel();

                if (_mqttClient.IsConnected)
                {
                    var disconnectOptions = new MqttClientDisconnectOptionsBuilder()
                        .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                        .Build();

                    await _mqttClient.DisconnectAsync(disconnectOptions, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during MQTT client disposal");
            }
            finally
            {
                _cancellationTokenSource.Dispose();
                _mqttClient.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public async Task<TResponse> HealthCheck<TResponse>()
        {
            var topic = "HealthCheck";
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload("HealthCheck")
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            var tcs = new TaskCompletionSource<TResponse>();
            await _mqttClient.PublishAsync(message, CancellationToken.None);

            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);
            if (completedTask == timeoutTask)
            {
                _logger.LogWarning("HealthCheck timeout.");
                throw new TimeoutException("HealthCheck timeout.");
            }

            return await tcs.Task;
        }

        public Task<TResponse> GetStatus<TResponse>()
        {
            throw new NotImplementedException();
        }

        private async Task HandleAccidentReport(string topicId, string payload, CancellationToken cancellationToken)
        {
            try
            {
                var accidentDto = JsonSerializer.Deserialize<AccidentDto>(payload);
                var newAccident = new AccidentEntity
                {
                    Guid = Guid.NewGuid(),
                    Latitude = (float)accidentDto!.Latitude,
                    Longitude = (float)accidentDto.Longitude,
                    Severity = accidentDto.Severity,
                    Timestamp = DateTime.UtcNow,
                    IsBlockingWay = accidentDto.IsBlockingWay,

                };

                if (accidentDto == null)
                {
                    _logger.LogError($"Invalid accident report payload: {payload}");
                    return;
                }
                using var scope = _serviceProvider.CreateScope();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                var accidentRepository = scope.ServiceProvider.GetRequiredService<IAccidentRepository>();

                var accidentEntity = mapper.Map<AccidentEntity>(accidentDto);
                var result = await accidentRepository.AddAsync(accidentEntity);
                await accidentRepository.SaveChangesAsync(cancellationToken);

                await PublishAsync($"rsu/Responses/AccidentReport/{topicId}", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing routing request with payload: {Payload}", payload);
            }
        }

        private async Task HandleDirectionRequestAsync(string topicId, string payload)
        {
            try
            {
                var requestMessage = JsonSerializer.Deserialize<DirectionRequestMessage>(payload);
                if (requestMessage == null)
                {
                    _logger.LogError("Invalid payload: {Payload}", payload);
                    return;
                }

                var result = new GetDirectionResponseDto();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orsService = scope.ServiceProvider.GetRequiredService<IORService>();
                    result = await orsService.GetRoutingDirectionAsync<GetDirectionResponseDto>(requestMessage.Profile, requestMessage.Request);

                }
                string responseTopic = $"rsu/Responses/Directions/{topicId}";
                var response = result.ToDirectionCutResponseDto();
                await PublishAsync(responseTopic, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing routing request with payload: {Payload}", payload);
            }
        }

        private async Task HandleAdvancedDirectionRequestAsync(string topicId, string payload)
        {
            try
            {
                var requestMessage = JsonSerializer.Deserialize<AdvancedDirectionRequestMessage>(payload);
                using var scope = _serviceProvider.CreateScope();
                var blockedPolygonRepository = scope.ServiceProvider.GetRequiredService<IBlockedPolygonCoordRepository>();
                var orservice = scope.ServiceProvider.GetRequiredService<IORService>();

                var blockedPolygons = await blockedPolygonRepository.GetAllUnResolvedAsync();
                var blockedPolygonCoordinates = blockedPolygons
                    .GroupBy(p => p.AccidentId)
                    .Select(PolygonMapper.MapToMultiBlockedPolygon).ToList();

                var blockedPolygon = new AvoidPolygonsDto
                (
                    Coordinates: blockedPolygonCoordinates
                        .Select(p => p.Select(l => l.Select(c => new CoordinateDto(c.Longitude, c.Latitude)).ToList()).ToList())
                        .ToList(),
                    Type: "MultiPolygon"
                );

                requestMessage!.Request.RoutingOptions!.AvoidPolygons = blockedPolygon;

                var result = await orservice.GetAdvancedRoutingDirectionAsync<GetDirectionAdvancedResponseDto>(requestMessage.Profile, requestMessage.Request);

                var responseTopic = $"rsu/Responses/AdvancedDirections/{topicId}";
                var response = result.ToDirectionCutResponseDto();
                await PublishAsync(responseTopic, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing routing request with payload: {Payload}", payload);
            }
        }
    }
}
