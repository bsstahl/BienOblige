using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Execution.Data.Kafka.Extensions;

public static class SerializationExtensions
{
    public static JsonSerializerOptions BienObligeDefaults(this JsonSerializerOptions value)
    {
        return new JsonSerializerOptions(value)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}
