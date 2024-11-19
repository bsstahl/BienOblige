using BienOblige.ActivityStream.Enumerations;
using BienOblige.Execution.Data.Kafka.Test.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Xunit.Abstractions;

namespace BienOblige.Execution.Data.Kafka.Test;

[ExcludeFromCodeCoverage, Collection("UnitTests")]
public class Activity_Ctor_Should
{
    private readonly Microsoft.Extensions.Logging.ILogger _logger;

    public Activity_Ctor_Should(ITestOutputHelper output)
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output).SetMinimumLevel(LogLevel.Trace))
            .BuildServiceProvider();

        _logger = services.GetRequiredService<ILogger<Activity_Ctor_Should>>();
    }

    [Fact]
    public void ProperlyDeserializeAMinimalCreateMessageAsAnActivity()
    {
        string jsonLd = @"{'@context':['https://www.w3.org/ns/activitystreams',{'bienoblige':'https://bienoblige.com/ns'},{'schema':'https://schema.org'}],'id':'urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8','@type':'Create','actor':{'id':'https://example.com/users/1234567','@type':'Person'},'object':{'id':'https://bienoblige.com/actionitems/5e4ea9cb-07f5-4259-bd8b-56dcc5a31d11','@type':['bienoblige:ActionItem','Object'],'content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','name':'Top off fuel tank of 1C6RD6KT4CS332867','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.'},'published':'2024-09-20T10:00:00Z'}"
            .Replace('\'', '"');

        var actual = new Messages.Activity(jsonLd);

        //var actualContext = actual.Context.ToList();
        //Assert.Equal(3, actualContext.Count);
        //Assert.Equal("https://www.w3.org/ns/activitystreams", actualContext.Single(c => !c.HasKey).Name.Value);
        //Assert.Equal("https://bienoblige.com/ns", actualContext.Single(c => c.HasKey && c!.Key!.Value == "bienoblige").Name.Value);
        //Assert.Equal("https://schema.org", actualContext.Single(c => c.HasKey && c!.Key!.Value == "schema").Name.Value);

        Assert.Equal("Create", actual.ActivityType);

        Assert.Equal("urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8", actual.CorrelationId);
        Assert.Equal("https://example.com/users/1234567", actual.Actor.Id);
        Assert.Equal(ActorType.Person.ToString(), actual.Actor.Type);
        Assert.Equal(DateTimeOffset.Parse("2024-09-20T10:00:00Z"), actual.Published);

        _logger.LogDebug("{@Actual}", actual);
        _logger.LogDebug("{@Serialized}", JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ProperlyDeserializeACompleteCreateMessageAsAnActivity()
    {
        string jsonLd = @"{'@context':['https://www.w3.org/ns/activitystreams',{'bienoblige':'https://bienoblige.com/ns'},{'schema':'https://schema.org'}],'id':'urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8','@type':'Create','actor':{'id':'https://example.com/users/1234567','@type':'Person'},'object':{'@context':'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4','id':'https://bienoblige.com/actionitems/5e4ea9cb-07f5-4259-bd8b-56dcc5a31d11','@type':['bienoblige:ActionItem','Object'],'attributedTo':{'id':'https://example.com/users/1234567','@type':'Person'},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{'id':'https://example.com/requirements/localDl'},{'id':'https://example.com/requirements#cdl'}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{'id':'https://example.com/applications/321321','@type':'Application'},'location':{'id':'https://example.com/places/33433','@type':'Place'},'name':'Top off fuel tank','origin':'https://bienoblige.com/actionitems/e5fd78c1-10c7-4a01-a932-42855bce5525','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'},{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}],'target':{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'},'updated':'2024-09-20T10:00:00Z'},'published':'2024-09-20T10:00:00Z'}"
            .Replace('\'', '"');

        var actual = new Messages.Activity(jsonLd);

        //var actualContext = actual.Context.ToList();
        //Assert.Equal(3, actualContext.Count);
        //Assert.Equal("https://www.w3.org/ns/activitystreams", actualContext.Single(c => !c.HasKey).Name.Value);
        //Assert.Equal("https://bienoblige.com/ns", actualContext.Single(c => c.HasKey && c!.Key!.Value == "bienoblige").Name.Value);
        //Assert.Equal("https://schema.org", actualContext.Single(c => c.HasKey && c!.Key!.Value == "schema").Name.Value);

        Assert.Equal(ActivityType.Create.ToString(), actual.ActivityType);

        Assert.Equal("urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8", actual.CorrelationId);
        Assert.Equal("https://example.com/users/1234567", actual.Actor.Id);
        Assert.Equal(ActorType.Person.ToString(), actual.Actor.Type);
        Assert.Equal(DateTimeOffset.Parse("2024-09-20T10:00:00Z"), actual.Published);

        _logger.LogDebug("{@Actual}", actual);
        _logger.LogDebug("{@Serialized}", JsonSerializer.Serialize(actual));
    }
}