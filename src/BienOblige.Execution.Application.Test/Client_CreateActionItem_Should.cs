using BienOblige.ValueObjects;
using BienOblige.Exceptions;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Extensions;
using BienOblige.Execution.Application.Interfaces;
using BienOblige.Execution.Application.Test.Extensions;
using BienOblige.Execution.Application.Test.Mocks;
using BienOblige.Execution.Builders;
using BienOblige.Execution.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;

namespace BienOblige.Execution.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_CreateActionItem_Should
{
    public Client_CreateActionItem_Should(ITestOutputHelper outputHelper)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", "BienOblige.Execution.Application.Test")
            .WriteTo.Xunit(outputHelper)
            .MinimumLevel.Verbose()
            .CreateLogger();
    }

    [Fact]
    public async Task ThrowIfNoActionItemIsSupplied()
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseExecutionClient()
            .UseMockRepositories()
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
            .UseExecutionClient()
            .UseMockRepositories()
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
            .UseExecutionClient()
            .UseMockRepositories()
            .BuildServiceProvider();

        ActionItem? item = new ActionItemBuilder()
            .UseRandomValues()
            .Build();
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<InvalidIdentifierException>(() => target.CreateActionItem(item, NetworkIdentity.From(string.Empty.GetRandom()), correlationId));
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
            .UseExecutionClient()
            .UseMockRepositories()
            .BuildServiceProvider();

        var mockRepo = services.GetRequiredService<ICreateActionItems>() as MockActionItemCreator;
        mockRepo!.SetupCreateActionItem(item, userId, correlationId);

        var target = services.GetRequiredService<Client>();
        var id = await target.CreateActionItem(item, userId, correlationId);

        mockRepo!.VerifyAll();
    }
}