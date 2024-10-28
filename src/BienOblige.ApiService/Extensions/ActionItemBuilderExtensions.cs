using BienOblige.Execution.Builders;

namespace BienOblige.ApiService.Extensions;

public static class ActionItemBuilderExtensions
{
    public static JsonContent BuildJsonContent(this ActionItemBuilder builder)
    {
        return builder.Build().AsJsonContent();
    }
}
