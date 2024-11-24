namespace BienOblige.Api.Constants;

public static class Context
{
    public static List<KeyValuePair<string?, string>> Default = new()
    {
        new KeyValuePair<string?, string>(null, "https://www.w3.org/ns/activitystreams"),
        new KeyValuePair<string?, string>("bienoblige", "https://bienoblige.com/ns"),
        new KeyValuePair < string ?, string >("schema", "https://schema.org")
    };
}
