using System.Text.Json;
using System.Text.Json.Serialization;

public class ContextConverter : JsonConverter<List<KeyValuePair<string?, string>>>
{
    public override List<KeyValuePair<string?, string>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var contextList = new List<KeyValuePair<string?, string>>();

        // Start reading the array
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                return contextList;

            // Read each object within the array
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
                {
                    JsonElement element = doc.RootElement;
                    if (element.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var property in element.EnumerateObject())
                        {
                            var key = property.Name;
                            var v = property.Value.GetString() ?? string.Empty;
                            contextList.Add(new KeyValuePair<string?, string>(key, v));
                        }
                    }
                    else if (element.ValueKind == JsonValueKind.String)
                    { 
                        var textValue = element.GetString() ?? string.Empty;
                        contextList.Add(new KeyValuePair<string?, string>(null, textValue));
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid JSON Type {element.ValueKind}.");
                    }
                }
            }
        }

        throw new JsonException("Unexpected end of JSON array.");
    }

    public override void Write(Utf8JsonWriter writer, List<KeyValuePair<string?, string>> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value)
        {
            if (item.Key is null)
                writer.WriteStringValue(item.Value);
            else
            {
                writer.WriteStartObject();
                writer.WriteString(item.Key, item.Value);
                writer.WriteEndObject();
            }
        }

        writer.WriteEndArray();
    }
}