using BienOblige.ActivityStream.Builders;

namespace BienOblige.Execution.Application.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ObjectBuilder UseRandomValues(this ObjectBuilder builder)
    {
        var idValue = Guid.NewGuid().ToString();
        return builder
            .Id($"https://example.org/{idValue}")
            .AddTypeName("bienoblige:ActionItem")
            .AddTypeName("Object")
            .Name($"Title of task {idValue}")
            .Content($"Content of task {idValue}");
    }
}
