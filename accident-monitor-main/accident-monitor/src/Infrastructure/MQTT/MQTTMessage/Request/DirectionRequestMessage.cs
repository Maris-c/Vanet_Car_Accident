using System.Text.Json.Serialization;
using AccidentMonitor.Application.ORService.Queries.GetDirections.Dtos;

namespace AccidentMonitor.Infrastructure.MQTT.MQTTMessage.Request
{
    public class DirectionRequestMessage
    {
        [JsonPropertyName("profile")]
        public string Profile { get; set; } = "driving-car";
        [JsonPropertyName("request")]
        public GetDirectionRequestDto Request { get; set; } = new GetDirectionRequestDto();
    }
}
