using BienOblige.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TestHelperExtensions;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class ArticleTarget_AsNetworkObject_Should
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public ArticleTarget_AsNetworkObject_Should(ITestOutputHelper output)
    {
        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output))
            .BuildServiceProvider();

        _logger = _services.GetRequiredService<ILogger<CarTarget_AsNetworkObject_Should>>();
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperId()
    {
        Targets.Article article = GetRandomArticle();
        var actual = article.AsNetworkObject();

        actual.Log(_logger);
        Assert.Equal(article.Id, actual.Id);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperName()
    {
        Targets.Article article = GetRandomArticle();
        var actual = article.AsNetworkObject();

        actual.Log(_logger);
        Assert.Equal(article.Title, actual.Name);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperContent()
    {
        Targets.Article article = GetRandomArticle();
        var actual = article.AsNetworkObject();

        actual.Log(_logger);
        Assert.Equal(article.Content, actual.Content);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperMediaType()
    {
        Targets.Article article = GetRandomArticle();
        var actual = article.AsNetworkObject();

        actual.Log(_logger);
        Assert.Equal(article.MediaType, actual.MediaType?.ToString());
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperPublishedValue()
    {
        Targets.Article article = GetRandomArticle();
        var actual = article.AsNetworkObject();

        actual.Log(_logger);
        Assert.Equal(article.PublicationDate, actual.Published);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperSummary()
    {
        Targets.Article article = GetRandomArticle();
        var actual = article.AsNetworkObject();

        actual.Log(_logger);
        Assert.Equal(article.Summary, actual.Summary);
    }

    [Fact]
    public void RoundTripSerializationSuccessfully()
    {
        var entity = GetRandomArticle();
        var actual = entity.AsNetworkObject();
        actual.Log(_logger);

        var json = JsonSerializer.Serialize(actual);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        
        _logger.LogTrace("Deserialized: {@Deserialized}", deserialized);
        Assert.Equal(entity.Id, deserialized?.Id);
    }

    private static Targets.Article GetRandomArticle()
    {
        int articleNumber = 9999.GetRandom(10);
        return new Targets.Article()
        {
            Id = new Uri($"https://example.com/article/{articleNumber}"),
            Title = $"Article {articleNumber}",
            Content = $"This is the *content* of Article {articleNumber}",
            MediaType = (new List<string>() { "text/html", "text/markdown", "text/plain", "application/json" }).GetRandom(),
            PublicationDate = DateTimeOffset.UtcNow.AddMinutes(99999.GetRandom(-9999999)),
            Summary = $"Summary of Article {articleNumber}"
        };
    }
}
