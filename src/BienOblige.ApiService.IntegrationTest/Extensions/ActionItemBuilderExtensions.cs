using BienOblige.ActivityStream.ValueObjects;
using BienOblige.Api.Builders;
using BienOblige.ApiService.IntegrationTest.Extensions;
using System.Net.Http.Json;

namespace BienOblige.ApiService.IntegrationTest.Builders;

[ExcludeFromCodeCoverage]
public static class ActionItemBuilderExtensions
{
    public static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        var idValue = Guid.NewGuid();
        return builder
            .Id(idValue)
            .Name($"Title of ActionItem {idValue}")
            .Content($"Content of ActionItem {idValue}");
    }

}
