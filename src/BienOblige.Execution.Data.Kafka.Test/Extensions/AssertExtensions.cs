using BienOblige.Execution.Aggregates;

namespace BienOblige.Execution.Data.Kafka.Test.Extensions;

[ExcludeFromCodeCoverage]
public static class AssertExtensions
{
    public static void ActionItemsEquivalent(this Assert assert, ActionItem expected, ActionItem actual, string message)
    {
        Assert.Equal(expected.Id.Value.ToString(), actual.Id.Value.ToString());
    }
}
