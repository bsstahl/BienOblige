using BienOblige.ActivityStream.Enumerations;

namespace BienOblige.Execution.Application.Extensions;

public static class StringExtensions
{
    public static ActivityType AsActivityType(this string? value)
    {
        return value is not null
            ? Enum.Parse<ActivityType>(value)
            : ActivityType.Reject; //  TODO: Restore - throw new ArgumentNullException(nameof(value));
    }
}
