using System.Diagnostics.CodeAnalysis;

namespace BienOblige.Api.Test.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpContentExtensions
{
    public static string GetStringContent(this HttpContent? content)
    {
        var requestContentTask = content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
        requestContentTask.Wait();
        return requestContentTask.Result;
    }

}
