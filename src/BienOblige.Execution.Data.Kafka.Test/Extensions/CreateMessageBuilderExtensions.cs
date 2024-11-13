using BienOblige.Execution.Builders;
using BienOblige.Execution.Data.Kafka.Builders;

namespace BienOblige.Execution.Data.Kafka.Test.Extensions;

[ExcludeFromCodeCoverage]
public static class CreateMessageBuilderExtensions
{
    public static ActivityMessageBuilder UseRandomValues(this ActivityMessageBuilder builder)
    {
        return builder
            .ActivityType(Enum.GetNames<Application.Enumerations.ActivityType>().GetRandom())
            .CorrelationId($"urn:uid:{Guid.NewGuid()}")
            .PublishedNow()
            .Actor(new ActorBuilder().UseRandomValues())
            .ActionItem(new ActionItemBuilder().UseRandomValues());
    }
}
