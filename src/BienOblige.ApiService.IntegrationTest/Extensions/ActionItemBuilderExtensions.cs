using BienOblige.Execution.Builders;

namespace BienOblige.ApiService.IntegrationTest.Builders;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        var idValue = string.Empty.GetRandom();
        return builder
            .Id($"https://example.org/actionitems/{idValue}")
            .Title($"Title of ActionItem {idValue}");
    }
}
