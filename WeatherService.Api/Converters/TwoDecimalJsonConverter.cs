using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeatherService.Api.Converters;

public sealed class TwoDecimalJsonConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetDecimal();

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) => writer.WriteRawValue(value.ToString("F2", CultureInfo.InvariantCulture));
}
