using BienOblige.ActivityStream.Enumerations;
using BienOblige.ActivityStream.ValueObjects;
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
        // TODO: Restore context node
        //string jsonLd = @"{'@context':['https://www.w3.org/ns/activitystreams',{'bienoblige':'https://bienoblige.com/ns'},{'schema':'https://schema.org'}],'id':'urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8','@type':'Create','actor':{'id':'https://example.com/users/1234567','@type':'Person'},'object':{'@context':'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4','id':'https://bienoblige.com/actionitems/5e4ea9cb-07f5-4259-bd8b-56dcc5a31d11','@type':['bienoblige:ActionItem','Object'],'attributedTo':{'id':'https://example.com/users/1234567','@type':'Person'},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{'id':'https://example.com/requirements/localDl'},{'id':'https://example.com/requirements#cdl'}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{'id':'https://example.com/applications/321321','@type':'Application'},'location':{'id':'https://example.com/places/33433','@type':'Place'},'name':'Top off fuel tank','attributedTo':'https://bienoblige.com/actionitems/e5fd78c1-10c7-4a01-a932-42855bce5525','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'},{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}],'target':{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'},'updated':'2024-09-20T10:00:00Z'},'published':'2024-09-20T10:00:00Z'}"
        //    .Replace('\'', '"');

        string jsonLd = @"{'id':'urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8','bienoblige:correlationId':'urn:uid:b93b0118-972b-481c-a10f-ded725a72e32', '@type':'Create','actor':{'id':'https://example.com/users/1234567','@type':'Person'},'object':{'@context':'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4','id':'https://bienoblige.com/actionitems/5e4ea9cb-07f5-4259-bd8b-56dcc5a31d11','@type':['bienoblige:ActionItem','Object'],'attributedTo':{'id':'https://example.com/users/1234567','@type':'Person'},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{'id':'https://example.com/requirements/localDl'},{'id':'https://example.com/requirements#cdl'}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{'id':'https://example.com/applications/321321','@type':'Application'},'location':[{'id':'https://example.com/places/33433','@type':'Place'}],'name':'Top off fuel tank','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'},{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}],'target':{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'},'updated':'2024-09-20T10:00:00Z'},'published':'2024-09-20T10:00:00Z'}"
            .Replace('\'', '"');
        _logger.LogDebug("{@Json}", jsonLd);

        var actual = JsonSerializer.Deserialize<Messages.Activity>(jsonLd);

        //var actualContext = actual.Context.ToList();
        //Assert.Equal(3, actualContext.Count);
        //Assert.Equal("https://www.w3.org/ns/activitystreams", actualContext.Single(c => !c.HasKey).Name.Value);
        //Assert.Equal("https://bienoblige.com/ns", actualContext.Single(c => c.HasKey && c!.Key!.Value == "bienoblige").Name.Value);
        //Assert.Equal("https://schema.org", actualContext.Single(c => c.HasKey && c!.Key!.Value == "schema").Name.Value);

        Assert.Equal("Create", actual!.Type);

        Assert.Equal("urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8", actual.Id);
        Assert.Equal("urn:uid:b93b0118-972b-481c-a10f-ded725a72e32", actual.CorrelationId);
        Assert.Equal("https://example.com/users/1234567", actual.Actor.Id);
        Assert.Equal(ActorType.Person.ToString(), actual.Actor.Type);
        Assert.Equal(DateTimeOffset.Parse("2024-09-20T10:00:00Z"), actual.Published);

        _logger.LogDebug("{@Actual}", actual);
        _logger.LogDebug("{@Serialized}", JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ProperlyDeserializeACompleteCreateMessageAsAnActivity()
    {
        // TODO: Restore context node
        //string jsonLd = @"{'@context':['https://www.w3.org/ns/activitystreams',{'bienoblige':'https://bienoblige.com/ns'},{'schema':'https://schema.org'}],'id':'urn:uid:E22CA55E-5F20-4899-A0DF-DACAE99F00B8','@type':'Create','actor':{'id':'https://example.com/users/1234567','@type':'Person'},'object':{'@context':'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4','id':'https://bienoblige.com/actionitems/5e4ea9cb-07f5-4259-bd8b-56dcc5a31d11','@type':['bienoblige:ActionItem','Object'],'attributedTo':{'id':'https://example.com/users/1234567','@type':'Person'},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{'id':'https://example.com/requirements/localDl'},{'id':'https://example.com/requirements#cdl'}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{'id':'https://example.com/applications/321321','@type':'Application'},'location':{'id':'https://example.com/places/33433','@type':'Place'},'name':'Top off fuel tank','attributedTo':'https://bienoblige.com/actionitems/e5fd78c1-10c7-4a01-a932-42855bce5525','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'},{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}],'target':{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'},'updated':'2024-09-20T10:00:00Z'},'published':'2024-09-20T10:00:00Z'}"
        //    .Replace('\'', '"');

        var id = $"urn:uid:{Guid.NewGuid().ToString()}";
        var correlationId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var actorId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var actionItemId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var attributedToId = $"urn:uid:{Guid.NewGuid().ToString()}";

        string jsonLd = $"{{'id':'{id}','bienoblige:correlationId': '{correlationId}','@type':'Create','actor':{{'id':'{actorId}','@type':'Application'}},'object':{{'@context':'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4','id':'{actionItemId}','@type':['bienoblige:ActionItem','Object'],'attributedTo':{{'id':'{attributedToId}','@type':'Person'}},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3}},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{{'id':'https://example.com/requirements/localDl'}},{{'id':'https://example.com/requirements#cdl'}}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{{'id':'https://example.com/applications/321321','@type':'Application'}},'location':[{{'id':'https://example.com/places/33433','@type':'Place'}}],'name':'Top off fuel tank','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'}},{{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}}],'target':{{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'}},'updated':'2024-09-20T10:00:00Z'}},'published':'2024-09-20T10:00:00Z'}}"
            .Replace('\'', '"');

        var actual = JsonSerializer.Deserialize<Messages.Activity>(jsonLd);

        //var actualContext = actual.Context.ToList();
        //Assert.Equal(3, actualContext.Count);
        //Assert.Equal("https://www.w3.org/ns/activitystreams", actualContext.Single(c => !c.HasKey).Name.Value);
        //Assert.Equal("https://bienoblige.com/ns", actualContext.Single(c => c.HasKey && c!.Key!.Value == "bienoblige").Name.Value);
        //Assert.Equal("https://schema.org", actualContext.Single(c => c.HasKey && c!.Key!.Value == "schema").Name.Value);

        Assert.Equal(ActivityType.Create.ToString(), actual!.Type);

        Assert.Equal(id, actual.Id);
        Assert.Equal(correlationId, actual.CorrelationId);
        Assert.Equal(actorId, actual.Actor.Id);
        Assert.Equal(ActorType.Application.ToString(), actual.Actor.Type);
        Assert.Equal(DateTimeOffset.Parse("2024-09-20T10:00:00Z"), actual.Published);

        _logger.LogDebug("{@Actual}", actual);
        _logger.LogDebug("{@Serialized}", JsonSerializer.Serialize(actual));
    }

    [Fact]
    public void ReturnUnknownPropertiesInExtension()
    {
        var id = $"urn:uid:{Guid.NewGuid().ToString()}";
        var correlationId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var actorId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var actionItemId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var attributedToId = $"urn:uid:{Guid.NewGuid().ToString()}";

        var expectedPropertyName = "extraPropertyName";
        var expectedPropertyValue = string.Empty.GetRandom();

        string jsonLd = $"{{'id':'{id}','bienoblige:correlationId': '{correlationId}','{expectedPropertyName}':'{expectedPropertyValue}', '@type':'Create','actor':{{'id':'{actorId}','@type':'Application'}},'object':{{'@context':'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4','id':'{actionItemId}','@type':['bienoblige:ActionItem','Object'],'attributedTo':{{'id':'{attributedToId}','@type':'Person'}},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3}},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{{'id':'https://example.com/requirements/localDl'}},{{'id':'https://example.com/requirements#cdl'}}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{{'id':'https://example.com/applications/321321','@type':'Application'}},'location':[{{'id':'https://example.com/places/33433','@type':'Place'}}],'name':'Top off fuel tank','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'}},{{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}}],'target':{{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'}},'updated':'2024-09-20T10:00:00Z'}},'published':'2024-09-20T10:00:00Z'}}"
            .Replace('\'', '"');
        _logger.LogDebug("{@Json}", jsonLd);
        
        var actual = JsonSerializer.Deserialize<Messages.Activity>(jsonLd);
        _logger.LogDebug("{@Actual}", actual);

        Assert.Equal(expectedPropertyValue, actual!.AdditionalProperties[expectedPropertyName].ToString());
    }

    [Fact]
    public void PersistUnknownPropertiesThrough()
    {
        var id = $"urn:uid:{Guid.NewGuid().ToString()}";
        var correlationId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var actorId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var actionItemId = $"urn:uid:{Guid.NewGuid().ToString()}";
        var attributedToId = $"urn:uid:{Guid.NewGuid().ToString()}";

        var expectedPropertyName = "extraPropertyName";
        var expectedPropertyValue = string.Empty.GetRandom();

        string jsonLd = $"{{'@context': {{'Value': [{{'HasKey': true, 'Key': {{ 'Value': 'example'}}, 'Name': {{'Value' : 'https://example.com/CustomerSale/25a4b838-ff12-42e4-b518-89d6ef7eafb4'}} }}]}},'id':'{id}','bienoblige:correlationId': '{correlationId}','{expectedPropertyName}':'{expectedPropertyValue}', '@type':'Create','actor':{{'id':'{actorId}','@type':'Application'}},'object':{{'id':'{actionItemId}','@type':['bienoblige:ActionItem','Object'],'attributedTo':{{'id':'{attributedToId}','@type':'Person'}},'bienoblige:completionMethod':'https://bienoblige.com/ns/completionmethod#Manual','bienoblige:effort':{{'@type':'bienoblige:StoryPoints','unitCode':'PTS','value':3}},'bienoblige:exceptions':[],'bienoblige:executorRequirements':[{{'id':'https://example.com/requirements/localDl'}},{{'id':'https://example.com/requirements#cdl'}}],'bienoblige:parent':'https://bienoblige.com/actionitems/85b6a2cd-31e5-4d95-9291-225ce51b6d15','bienoblige:prerequisites':[{{'id':'https://bienoblige.com/actionitems/854dc0a9-adeb-4b6e-9699-5a2c5a0dab3a'}}],'bienoblige:priority':'https://bienoblige.com/ns/priority#Medium','bienoblige:status':'https://bienoblige.com/ns/status#Incomplete','content':'Ensure the vehicle with VIN: <i>1C6RD6KT4CS332867</i> is fueled to at or near capacity.','endTime':'2024-10-02T20:45:00Z','generator':{{'id':'https://example.com/applications/321321','@type':'Application'}},'location':[{{'id':'https://example.com/places/33433','@type':'Place'}}],'name':'Top off fuel tank','published':'2024-09-20T10:00:00Z','summary':'We need to make certain that the vehicle with VIN 1C6RD6KT4CS332867 is fueled to at or near capacity in preparation for sale to a customer.','tag':[{{'@type':'https://example.com/tags/ProjectPhase','id':'https://example.com/projectphases#exploration'}},{{'@type':'https://example.com/tags/TechStack','id':'https://example.com/techstack#dotnet'}}],'target':{{'@id':'https://example.com/vehicles/1C6RD6KT4CS332867','@type':'schema:Car','name':'2022 Ram 1500','vehicleIdentificationNumber':'1C6RD6KT4CS332867'}},'updated':'2024-09-20T10:00:00Z'}},'published':'2024-09-20T10:00:00Z'}}"
            .Replace('\'', '"');
        _logger.LogDebug("{@Json}", jsonLd);

        var deserialized = JsonSerializer.Deserialize<Messages.Activity>(jsonLd);
        var serialized = JsonSerializer.Serialize(deserialized);
        var actual = JsonSerializer.Deserialize<Messages.Activity>(serialized);

        _logger.LogDebug("{@Actual}", serialized);

        Assert.Equal(expectedPropertyValue, actual!.AdditionalProperties[expectedPropertyName].ToString());
    }
}