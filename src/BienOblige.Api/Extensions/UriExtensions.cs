namespace BienOblige.Api.Extensions;

public static class UriExtensions
{
    public static Uri AsInstanceId(this Uri instanceBaseUri)
    {
        return instanceBaseUri.AsInstanceId(Guid.NewGuid());
    }

    public static Uri AsInstanceId(this Uri instanceBaseUri, Guid guid)
    {
        return new Uri(instanceBaseUri, $"/actionitem/{guid}");
    }
}
