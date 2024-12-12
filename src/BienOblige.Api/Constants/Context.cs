namespace BienOblige.Api.Constants;

public static class Context
{
    public const string ActivityStreamsNamespace = "https://www.w3.org/ns/activitystreams";
    public const string BienObligeNamespace = "https://bienoblige.com/ns";
    public const string SchemaNamespace = "https://schema.org";

    public static List<KeyValuePair<string?, string>> Default = new()
    {
        new KeyValuePair<string?, string>(null, ActivityStreamsNamespace),
        new KeyValuePair<string?, string>("bienoblige", BienObligeNamespace),
        new KeyValuePair < string ?, string >("schema", SchemaNamespace)
    };
}
