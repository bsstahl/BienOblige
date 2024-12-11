using BienOblige.Api.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using TestHelperExtensions;
using Xunit.Abstractions;

namespace BienOblige.Api.Test;

[ExcludeFromCodeCoverage]
public class AudioTarget_AsNetworkObject_Should
{
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public AudioTarget_AsNetworkObject_Should(ITestOutputHelper output)
    {
        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output))
            .BuildServiceProvider();

        _logger = _services.GetRequiredService<ILogger<CarTarget_AsNetworkObject_Should>>();
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperId()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Id, actual.Id);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperName()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Title, actual.Name);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperContent()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Content, actual.Content);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperMediaType()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.MediaType, actual.MediaType?.ToString());
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperPublishedValue()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.PublicationDate, actual.Published);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperSummary()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Description, actual.Summary);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperUrls()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Urls, actual.Url);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperDuration()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Duration, actual.Duration);
    }

    [Fact]
    public void SerializesDurationInISO8601DurationFormat()
    {
        var entity = GetRandomAudio();
        entity.Duration = TimeSpan.FromMinutes(17);

        var actual = entity.AsNetworkObject();
        var json = JsonSerializer.Serialize(actual);
        var durationNode = JsonDocument.Parse(json).RootElement.GetProperty("duration");
        var durationNodeValue = durationNode.GetString();

        Assert.Equal("PT17M", durationNodeValue);
    }

    [Fact]
    public void DurationSurvivesSerializationAsNetworkObject()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        var json = JsonSerializer.Serialize(actual);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        Assert.Equal(entity.Duration, deserialized?.Duration);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperAttribution()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();

        Assert.Equal(entity.Creator?.Id, actual.AttributedTo?.Id);
        Assert.Equal(entity.Creator?.Name, actual.AttributedTo?.Name);
        Assert.Equal(entity.Creator?.ObjectType, actual.AttributedTo?.ObjectType);
        Assert.Equal(entity.Creator?.Summary, actual.AttributedTo?.Summary);
    }

    [Fact]
    public void ReturnANetworkObjectWithTheProperTags()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        Assert.Equal(entity.Tag.Select(t => t.Id), actual.Tag.Select(t => t.Id));
    }

    [Fact]
    public void RoundTripSerializationAsANetworkObjectSuccessfully()
    {
        var entity = GetRandomAudio();
        var actual = entity.AsNetworkObject();
        var json = JsonSerializer.Serialize(actual);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        Assert.Equal(entity.Id, deserialized?.Id);
    }

    [Fact]
    public void RoundTripSerializationAsAnAudioObjectSuccessfully()
    {
        var entity = GetRandomAudio();
        var json = JsonSerializer.Serialize(entity);
        var deserialized = JsonSerializer.Deserialize<NetworkObject>(json);
        Assert.Equal(entity.Id, deserialized?.Id);
    }

    private Targets.Audio GetRandomAudio()
    {
        int idNumber = 9999.GetRandom(10);

        var tagCount = 8.GetRandom(1);
        var tag = new List<NetworkObject>();
        for (int i = 0; i < tagCount; i++)
            tag.Add(new NetworkObject()
            {
                Id = new Uri($"https://example.com/file/{idNumber}/{string.Empty.GetRandom()}"),
                ObjectType = [TestHelpers.TagObjectTypes.GetRandom()]
            });

        var result = new Targets.Audio()
        {
            Id = new Uri($"https://example.com/entity/{idNumber}"),
            Title = $"Audio track number {idNumber}",
            Content = $"This is a transcript of the content of audio track {idNumber}",
            MediaType = (new List<string>() { "text/html", "text/markdown", "text/plain", "application/json" }).GetRandom(),
            PublicationDate = DateTimeOffset.UtcNow.AddMinutes(99999.GetRandom(-9999999)),
            Description = $"A description of Audio track {idNumber}",
            Urls = new List<Uri>()
            {
                new Uri($"https://tidal.com/track/{idNumber}"),
                new Uri($"https://spotify.com/track/{idNumber}"),
                new Uri($"https://youtube.com/watch?v={idNumber}"),
                new Uri($"https://soundcloud.com/track/{idNumber}")
            },
            Duration = TimeSpan.FromSeconds(9999.GetRandom(55)),
            Creator = new NetworkObject()
            {
                Id = new Uri($"https://example.com/artist/{idNumber}"),
                ObjectType = ["Person"],
                Name = $"Artist {idNumber}",
                Summary = $"A brief summary of the career of Artist {idNumber}"
            },
            Tag = tag
        };

        _logger.LogInformation(JsonSerializer.Serialize(result));

        return result;
    }
}
