using BienOblige.Execution.Builders;

namespace BienOblige.ApiService.IntegrationTest.Builders;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        return builder
            .Id($"https://example.org/actionitems/{string.Empty.GetRandom()}");
    }
}
