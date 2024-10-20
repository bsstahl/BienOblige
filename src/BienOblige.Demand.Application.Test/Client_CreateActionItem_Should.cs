using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Extensions;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Application.Test.Extensions;
using BienOblige.Demand.Application.Test.Mocks;
using BienOblige.Demand.Builders;
using BienOblige.Demand.Exceptions;
using BienOblige.Demand.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace BienOblige.Demand.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_CreateActionItem_Should
{
    [Fact]
    public void ThrowIfNoActionItemIsSupplied()
    {
        var services = new ServiceCollection()
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        ActionItem? item = null;
        var userId = (null as NetworkIdentity).CreateRandom();

        var target = services.GetRequiredService<Client>();
        Assert.Throws<ArgumentNullException>(() => target.CreateActionItem(item!, userId));
    }

    [Fact]
    public void ThrowIfNoUserIdIsSupplied()
    {
        var services = new ServiceCollection()
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        NetworkIdentity? userId = null;

        var target = services.GetRequiredService<Client>();
        Assert.Throws<ArgumentNullException>(() => target.CreateActionItem(item, userId!));
    }

    [Fact]
    public void ThrowIfTheUserIdIsInvalid()
    {
        var services = new ServiceCollection()
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();

        var target = services.GetRequiredService<Client>();
        Assert.Throws<InvalidIdentifierException>(() => target.CreateActionItem(item, NetworkIdentity.From(string.Empty.GetRandom())));
    }

    [Fact]
    public void ThrowIfActionItemIdentityAlreadyExists()
    {
        var existingActionItem = new ActionItemBuilder()
                .UseRandomValues()
                .Build();

        var services = new ServiceCollection()
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<IGetActionItems>() as MockActionItemReader;
        mockRepo!.SetupExistingActionItem(existingActionItem);

        var userId = (null as NetworkIdentity).CreateRandom();

        var target = services.GetRequiredService<Client>();
        Assert.Throws<DuplicateIdentifierException>(() => target.CreateActionItem(existingActionItem, userId));
    }

    [Fact]
    public void ReturnTheDuplicatedIdInTheException()
    {
        var existingActionItem = new ActionItemBuilder()
                .UseRandomValues()
                .Build();

        var services = new ServiceCollection()
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<IGetActionItems>() as MockActionItemReader;
        mockRepo!.SetupExistingActionItem(existingActionItem);

        var userId = (null as NetworkIdentity).CreateRandom();

        var target = services.GetRequiredService<Client>();
        DuplicateIdentifierException? actualException = null;
        try
        {
            target.CreateActionItem(existingActionItem, userId);
        }
        catch (DuplicateIdentifierException ex)
        {
            actualException = ex;
        }    

        Assert.Equal(existingActionItem.Id, actualException?.Id);
    }

    [Fact]
    public void SuccessfullyCreateTheActionItem()
    {
        var userId = (null as NetworkIdentity).CreateRandom();
        var item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();

        var services = new ServiceCollection()
            .UseDemandClient()
            .UseMockActionItemRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<ICreateActionItems>() as MockActionItemCreator;
        mockRepo!.SetupCreateActionItem(item, userId);

        var target = services.GetRequiredService<Client>();
        var id = target.CreateActionItem(item, userId);

        mockRepo!.VerifyAll();
    }
}