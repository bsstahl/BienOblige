using System.Text.Json.Serialization;
using System.Text.Json;
using BienOblige.Execution.Data.Kafka.Messages;

namespace BienOblige.Execution.Data.Kafka;

public class ContextCollectionConverter : JsonConverter<IEnumerable<Context>>
{
    public override IEnumerable<Context>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();

        //var list = new List<Context>();

        //if (reader.TokenType != JsonTokenType.StartArray)
        //    throw new JsonException();

        //while (reader.Read())
        //{
        //    if (reader.TokenType == JsonTokenType.EndArray)
        //        break;

        //    if (reader.TokenType == JsonTokenType.String)
        //    {
        //        list.Add(reader.GetString());
        //    }
        //    else if (reader.TokenType == JsonTokenType.StartObject)
        //    {
        //        var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options);
        //        list.Add(dictionary);
        //    }
        //}

        //return list;
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<Context> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        
        WriteUnkeyed(writer, value.Where(x => !x.HasKey), options);
        WriteKeyed(writer, value.Where(x => x.HasKey), options);

        writer.WriteEndArray();
    }

    private void WriteUnkeyed(Utf8JsonWriter writer, IEnumerable<Context> value, JsonSerializerOptions options)
    {
        foreach (var item in value)
            writer.WriteStringValue(item.Name.Value);
    }

    private void WriteKeyed(Utf8JsonWriter writer, IEnumerable<Context> value, JsonSerializerOptions options)
    {
        var dict = value.ToDictionary(x => x.Key!.Value, x => x.Name.Value);
        JsonSerializer.Serialize(writer, dict, options);
    }
}