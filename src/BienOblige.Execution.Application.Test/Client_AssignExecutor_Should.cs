using BienOblige.ValueObjects;
using BienOblige.Execution.Application.Test.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit.Abstractions;
using BienOblige.Execution.Application.Extensions;

namespace BienOblige.Execution.Application.Test;

[ExcludeFromCodeCoverage]
public class Client_AssignExecutor_Should
{
    public Client_AssignExecutor_Should(ITestOutputHelper output)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", "BienOblige.Execution.Application.Test")
            .WriteTo.Xunit(output)
            .MinimumLevel.Verbose()
            .CreateLogger();
    }

    [Fact]
    public async Task ThrowIfNoExecutorIdSupplied()
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddSerilog())
            .UseExecutionClient()
            .UseMockRepositories()
            .BuildServiceProvider();

        var actionItemId = (null as NetworkIdentity).CreateRandom();
        NetworkIdentity? executorId = null;
        var userId = (null as NetworkIdentity).CreateRandom();
        var correlationId = Guid.NewGuid().ToString();

        var target = services.GetRequiredService<Client>();
        await Assert.ThrowsAsync<ArgumentNullException>(() => target.AssignExecutor(actionItemId, executorId!, userId, correlationId));
    }
}