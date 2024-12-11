using BienOblige.Api.Builders;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using TestHelperExtensions;
using Xunit.Abstractions;

namespace BienOblige.ApiService.Test;

[ExcludeFromCodeCoverage]
public class ValidateActivityCollection_InvokeAsync_Should
{
    IServiceProvider _services;

    public ValidateActivityCollection_InvokeAsync_Should(ITestOutputHelper output)
    {
        var config = new ConfigurationBuilder()
            .Build();

        _services = new ServiceCollection()
            .AddLogging(b => b.AddXUnit(output).SetMinimumLevel(LogLevel.Trace))
            .AddSingleton<IConfiguration>(config)
            .AddSingleton<Middleware.ValidateActivityCollection>()
            .AddSingleton(Mock.Of<Microsoft.AspNetCore.Http.RequestDelegate>())
            .BuildServiceProvider();
    }

    [Fact]
    public async Task ResultInAValidRequest_SingularActivity()
    {
        var logger = _services.GetRequiredService<ILogger<ValidateActivityCollection_InvokeAsync_Should>>();
        using (logger.BeginScope(new Dictionary<string, object>()
            {
                {  "Method", "BienOblige.ApiService.Test.ValidateActivityCollection_InvokeAsync_Should.ReturnAValidResponseStream" }
            }))
        {
            var target = _services.GetRequiredService<Middleware.ValidateActivityCollection>();

            var mockContext = new Mock<HttpContext>();
            var mockRequest = new Mock<HttpRequest>();
            var mockResponse = new Mock<HttpResponse>();

            var context = mockContext.Object;
            var request = mockRequest.Object;
            var response = mockResponse.Object;

            mockRequest.SetupAllProperties();
            mockResponse.SetupAllProperties();
            mockContext.SetupAllProperties();

            mockRequest.SetupGet(req => req.HttpContext).Returns(context);
            mockResponse.SetupGet(res => res.HttpContext).Returns(context);

            var activity = new CreateActionItemActivityBuilder()
                .Actor(new ActorBuilder()
                    .ActorType(Api.Enumerations.ActorType.Application)
                    .Id(Guid.NewGuid())
                    .Name($"{this.GetType().Name}.{nameof(ResultInAValidRequest_SingularActivity)}"))
                .CorrelationId("urn:uid:af680c89-9562-43b0-a84a-9f16ca1e7cf9")
                .ActionItem(new ActionItemBuilder()
                    .Id(Guid.NewGuid())
                    .Name("Title of this ActionItem")
                    .Content("This is the content of the ActionItem describing the task to be completed.", "text/plain"))
                .Build();

            request.Body = JsonSerializer.Serialize(activity).ToStream();
            request.ContentType = "application/json";
            request.ContentLength = request.Body.Length;

            //List<PublicationResult> publicationResults = [new PublicationResult(activity)];
            //response.Body = JsonSerializer.Serialize(publicationResults).ToStream();
            context.Items = new Dictionary<object, object?>();

            mockContext.SetupGet(c => c.Request).Returns(request);
            // mockContext.SetupGet(c => c.Response).Returns(response);
            context.Request.EnableBuffering();

            // Act
            await target.InvokeAsync(context);

            // Assert
            string actualRequestBody;
            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                actualRequestBody = await reader.ReadToEndAsync();
                logger.LogTrace("Actual Response Body: {ActualRequestBody}", actualRequestBody);
            };

            var actual = JsonSerializer.Deserialize<IEnumerable<Api.Entities.Activity>>(actualRequestBody);
            var actualCorrelationId = actual?.Single()?.CorrelationId?.ToString();
            Assert.Equal("urn:uid:af680c89-9562-43b0-a84a-9f16ca1e7cf9", actualCorrelationId);
        }
    }
}
