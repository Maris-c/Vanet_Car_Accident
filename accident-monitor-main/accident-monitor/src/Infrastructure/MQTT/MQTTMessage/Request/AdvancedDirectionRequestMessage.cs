using System.Text.Json.Serialization;
using AccidentMonitor.Application.ORService.Queries.GetDirectionAdvanced.Dtos;

namespace AccidentMonitor.Infrastructure.MQTT.MQTTMessage.Request;

public class AdvancedDirectionRequestMessage
{
    [JsonPropertyName("profile")]
    public string Profile { get; set; } = "driving-car";

    [JsonPropertyName("request")]
    public GetDirectionAdvanceRequestDto Request { get; set; } = new GetDirectionAdvanceRequestDto();

}
