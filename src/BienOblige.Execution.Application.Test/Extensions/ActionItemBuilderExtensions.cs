using BienOblige.Execution.Builders;

namespace BienOblige.Execution.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        var idValue = Guid.NewGuid().ToString();
        return builder
            .Id($"https://example.org/{idValue}")
            .Name($"Title of task {idValue}")
            .Content($"Content of task {idValue}");
    }
}
