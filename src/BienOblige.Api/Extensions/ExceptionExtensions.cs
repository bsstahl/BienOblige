namespace BienOblige.Api.Extensions;

public static class ExceptionExtensions
{
    public static IEnumerable<string> GetExceptionMessages(this Exception ex)
    {
        var result = new List<string>();
        result.Add($"Failed to publish due to error: '{ex.Message}' ({ex.GetType().FullName ?? ex.GetType().Name})");
        result.AddRange(ex.GetInnerExceptionMessages());
        return result;
    }

    // Recursively get all inner exception messages
    public static IEnumerable<string> GetInnerExceptionMessages(this Exception? exception)
    {
        var result = new List<string>();
        if (exception is not null && exception.InnerException is not null)
        {
            Exception ex = exception.InnerException!;
            result.Add($"Inner exception: '{ex.Message}' ({ex.GetType().FullName ?? ex.GetType().Name})");
            result.AddRange(ex.GetInnerExceptionMessages());
        }
        return result;
    }
}
