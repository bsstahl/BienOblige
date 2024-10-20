using BienOblige.Demand.Builders;

namespace BienOblige.Demand.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        return builder
            .Id($"https://example.org/{string.Empty.GetRandom()}");
    }
}
