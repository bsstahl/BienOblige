namespace BienOblige.ApiService.Configuration;

public class BearerTokenAuthenticationOptions
{
    public const string SectionName = "BearerTokenAuthentication";

    public IList<string> ValidTokens { get; set; } = [];
}
