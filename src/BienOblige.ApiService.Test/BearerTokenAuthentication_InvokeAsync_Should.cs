using BienOblige.ApiService.Configuration;
using BienOblige.ApiService.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace BienOblige.ApiService.Test;

[ExcludeFromCodeCoverage]
public class BearerTokenAuthentication_InvokeAsync_Should
{
    private readonly ITestOutputHelper _output;

    public BearerTokenAuthentication_InvokeAsync_Should(ITestOutputHelper output)
    {
        _output = output;
    }

    private static BearerTokenAuthentication CreateTarget(
        RequestDelegate next,
        ILogger<BearerTokenAuthentication> logger,
        IEnumerable<string> validTokens)
    {
        var options = Options.Create(new BearerTokenAuthenticationOptions
        {
            ValidTokens = validTokens.ToList()
        });
        return new BearerTokenAuthentication(next, logger, options);
    }

    private ILogger<BearerTokenAuthentication> CreateLogger()
    {
        var services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(_output).SetMinimumLevel(LogLevel.Trace))
            .BuildServiceProvider();
        return services.GetRequiredService<ILogger<BearerTokenAuthentication>>();
    }

    [Fact]
    public async Task CallNextMiddleware_WhenTokenIsValid()
    {
        var nextCalled = false;
        RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };

        var target = CreateTarget(next, CreateLogger(), ["my-valid-token"]);

        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer my-valid-token";
        context.Response.Body = new System.IO.MemoryStream();

        await target.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task Return401_WhenAuthorizationHeaderIsMissing()
    {
        var nextCalled = false;
        RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };

        var target = CreateTarget(next, CreateLogger(), ["my-valid-token"]);

        var context = new DefaultHttpContext();
        context.Response.Body = new System.IO.MemoryStream();

        await target.InvokeAsync(context);

        Assert.False(nextCalled);
        Assert.Equal(401, context.Response.StatusCode);
    }

    [Fact]
    public async Task Return401_WhenTokenIsNotInValidList()
    {
        var nextCalled = false;
        RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };

        var target = CreateTarget(next, CreateLogger(), ["my-valid-token"]);

        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer wrong-token";
        context.Response.Body = new System.IO.MemoryStream();

        await target.InvokeAsync(context);

        Assert.False(nextCalled);
        Assert.Equal(401, context.Response.StatusCode);
    }

    [Fact]
    public async Task Return401_WhenNoValidTokensAreConfigured()
    {
        var nextCalled = false;
        RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };

        var target = CreateTarget(next, CreateLogger(), []);

        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer any-token";
        context.Response.Body = new System.IO.MemoryStream();

        await target.InvokeAsync(context);

        Assert.False(nextCalled);
        Assert.Equal(401, context.Response.StatusCode);
    }

    [Fact]
    public async Task CallNextMiddleware_WhenTokenIsValid_WithoutBearerPrefix()
    {
        var nextCalled = false;
        RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };

        var target = CreateTarget(next, CreateLogger(), ["my-valid-token"]);

        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "my-valid-token";
        context.Response.Body = new System.IO.MemoryStream();

        await target.InvokeAsync(context);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task CallNextMiddleware_WhenOneOfMultipleValidTokensIsPresented()
    {
        var nextCalled = false;
        RequestDelegate next = _ => { nextCalled = true; return Task.CompletedTask; };

        var target = CreateTarget(next, CreateLogger(), ["token-one", "token-two", "token-three"]);

        var context = new DefaultHttpContext();
        context.Request.Headers.Authorization = "Bearer token-two";
        context.Response.Body = new System.IO.MemoryStream();

        await target.InvokeAsync(context);

        Assert.True(nextCalled);
    }
}
