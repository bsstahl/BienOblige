using BienOblige.ValueObjects;
using BienOblige.Exceptions;
using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Extensions;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Application.Test.Extensions;
using BienOblige.Demand.Application.Test.Mocks;
using BienOblige.Demand.Builders;
using BienOblige.Demand.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace BienOblige.Demand.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_CreateActionItem_Should
{
    public Client_CreateActionItem_Should(ITestOutputHelper outputHelper)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", "BienOblige.Demand.Application.Test")
            .WriteTo.Xunit(outputHelper)
            .MinimumLevel.Verbose()
            .CreateLogger();
    }

    [Fact]
    public async Task ThrowIfNoActionItemIsSupplied()
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        ActionItem? item = null;
        var userId = (null as NetworkIdentity).CreateRandom();
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() => target.CreateActionItem(item!, userId, correlationId));
    }

    [Fact]
    public async Task ThrowIfNoUserIdIsSupplied()
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        NetworkIdentity? userId = null;
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() => target.CreateActionItem(item, userId!, correlationId));
    }

    [Fact]
    public async Task ThrowIfTheUserIdIsInvalid()
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<InvalidIdentifierException>(() => target.CreateActionItem(item, NetworkIdentity.From(string.Empty.GetRandom()), correlationId));
    }

    [Fact]
    public async Task ThrowIfActionItemIdentityAlreadyExists()
    {
        var existingActionItem = new ActionItemBuilder()
                .UseRandomValues()
                .Build();

        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<IGetActionItems>() as MockActionItemReader;
        mockRepo!.SetupExistingActionItem(existingActionItem);

        var userId = (null as NetworkIdentity).CreateRandom();
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<DuplicateIdentifierException>(() => target.CreateActionItem(existingActionItem, userId, correlationId));
    }

    [Fact]
    public async Task ReturnTheDuplicatedIdInTheException()
    {
        var existingActionItem = new ActionItemBuilder()
                .UseRandomValues()
                .Build();

        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<IGetActionItems>() as MockActionItemReader;
        mockRepo!.SetupExistingActionItem(existingActionItem);

        var userId = (null as NetworkIdentity).CreateRandom();
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        DuplicateIdentifierException? actualException = null;
        try
        {
            await target.CreateActionItem(existingActionItem, userId, correlationId);
        }
        catch (DuplicateIdentifierException ex)
        {
            actualException = ex;
        }    

        Assert.Equal(existingActionItem.Id, actualException?.Id);
    }

    [Fact]
    public async Task SuccessfullyCreateTheActionItem()
    {
        var userId = (null as NetworkIdentity).CreateRandom();
        var item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<ICreateActionItems>() as MockActionItemCreator;
        mockRepo!.SetupCreateActionItem(item, userId, correlationId);

        var target = services.GetRequiredService<Client>();
        var id = await target.CreateActionItem(item, userId, correlationId);

        mockRepo!.VerifyAll();
    }
}