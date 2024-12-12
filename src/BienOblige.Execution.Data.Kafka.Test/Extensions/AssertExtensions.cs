using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.Execution.Data.Kafka.Test.Extensions;

[ExcludeFromCodeCoverage]
public static class AssertExtensions
{
    public static void ActionItemsEquivalent(this Assert assert, NetworkObject expected, NetworkObject actual, string message)
    {
        Assert.Equal(expected.Id.Value.ToString(), actual.Id.Value.ToString());
    }
}
