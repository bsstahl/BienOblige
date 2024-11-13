using BienOblige.Execution.Application.Enumerations;

namespace BienOblige.Execution.Application.Extensions;

public static class StringExtensions
{
    public static ActivityType AsActivityType(this string value)
    {
        return Enum.Parse<ActivityType>(value);
    }
}
