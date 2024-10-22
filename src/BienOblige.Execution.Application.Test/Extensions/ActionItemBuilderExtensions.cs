using BienOblige.Execution.Builders;

namespace BienOblige.Execution.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        return builder
            .Id($"https://example.org/{string.Empty.GetRandom()}");
    }
}
