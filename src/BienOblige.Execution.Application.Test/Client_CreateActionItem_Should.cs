using BienOblige.ValueObjects;
using BienOblige.Exceptions;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Test.Extensions;
using BienOblige.Execution.Application.Test.Mocks;
using BienOblige.Execution.Builders;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace BienOblige.Execution.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_CreateActionItem_Should
{
    IServiceProvider _services;

    public Client_CreateActionItem_Should(ITestOutputHelper outputHelper)
    {
        _services = new ServiceCollection()
            .UseExecutionClient()
            .UseMockRepositories()
            .AddLogging(b => b.AddXUnit(outputHelper))
            .BuildServiceProvider();
    }

    [Fact]
    public async Task ThrowIfNoActionItemIsSupplied()
    {
        List<ActionItem> items = new();
        var userId = (null as NetworkIdentity).CreateRandom();
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentException>(() 
            => target.CreateActionItem(items, userId, correlationId));
    }

    [Fact]
    public async Task ThrowIfNoActionItemListIsSupplied()
    {
        List<ActionItem>? items = null;
        var userId = (null as NetworkIdentity).CreateRandom();
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(()
            => target.CreateActionItem(items!, userId, correlationId));
    }


    [Fact]
    public async Task ThrowIfNoUserIdIsSupplied()
    {
        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        NetworkIdentity? userId = null;
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() 
            => target.CreateActionItem(new[] { item }, userId!, correlationId));
    }

    [Fact]
    public async Task ThrowIfTheUserIdIsInvalid()
    {
        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var target = _services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<InvalidIdentifierException>(() 
            => target.CreateActionItem(new[] { item }, NetworkIdentity.From(string.Empty.GetRandom()), correlationId));
    }

    [Fact]
    public async Task SuccessfullyCreateTheActionItem()
    {
        var userId = (null as NetworkIdentity).CreateRandom();
        var item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var mockRepo = _services.GetRequiredService<ICreateActionItems>() as MockActionItemCreator;
        mockRepo!.SetupCreateActionItem(new[] { item }, userId, correlationId);

        var target = _services.GetRequiredService<Client>();
        var id = await target.CreateActionItem(new[] { item }, userId, correlationId);

        mockRepo!.VerifyAll();
    }
}