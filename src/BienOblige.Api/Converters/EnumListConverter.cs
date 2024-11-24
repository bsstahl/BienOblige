using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Converters;

public class EnumListConverter<T> : JsonConverter<List<T>> where T : struct, Enum
{
    public override List<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new List<T>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            if ((reader.TokenType == JsonTokenType.String) && (Enum.TryParse(reader.GetString(), out T enumValue)))
                result.Add(enumValue);

        return result;
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
            writer.WriteStringValue(item.ToString());
        writer.WriteEndArray();
    }
}
