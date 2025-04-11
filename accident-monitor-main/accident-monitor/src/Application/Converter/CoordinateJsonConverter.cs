using System.Text.Json;
using System.Text.Json.Serialization;
using AccidentMonitor.Application.ORService.Dto;

namespace AccidentMonitor.Application.Converter
{
    public class CoordinateJsonConverter : JsonConverter<CoordinateDto>
    {
        public override CoordinateDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            reader.Read();
            var longitude = reader.GetDouble();
            reader.Read();
            var latitude = reader.GetDouble();
            reader.Read();

            return reader.TokenType != JsonTokenType.EndArray
                ? throw new JsonException()
                : new CoordinateDto { Longitude = longitude, Latitude = latitude };
        }

        public override void Write(Utf8JsonWriter writer, CoordinateDto value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Longitude);
            writer.WriteNumberValue(value.Latitude);
            writer.WriteEndArray();
        }
    }
}
