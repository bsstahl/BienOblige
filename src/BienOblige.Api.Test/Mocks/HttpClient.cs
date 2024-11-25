using BienOblige.Api.Entities;
using BienOblige.Api.ValueObjects;
using Moq;
using Moq.Protected;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class HttpClient : System.Net.Http.HttpClient
{
    private static readonly List<HttpRequestMessage> _requests = new();
    public IEnumerable<HttpRequestMessage> Requests => _requests;

    public string JsonRequestMessages
    {
        get
        {
            var result = new List<string>();
            foreach (var request in _requests)
            {
                var task = request.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
                task.Wait();
                result.Add(task.Result);
            }
            return $"[{string.Join(",", result)}]";
        }
    }

    public IEnumerable<Activity> ActivityRequests
            => JsonSerializer.Deserialize<IEnumerable<Activity>>(this.JsonRequestMessages)
                ?? throw new ArgumentNullException(nameof(Activity));


    public HttpClient() : base(MessageHandler)
    {
        base.BaseAddress = new Uri("http://bienoblige.com/api/v1");
    }

    private static HttpMessageHandler MessageHandler
    {
        get
        {
            var handler = new Mock<HttpMessageHandler>();

            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    _requests.Add(request);
                })
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var a = JsonSerializer.Deserialize<NetworkObjectProxy>(request.Content?.ReadAsStringAsync()?.Result ?? "{}");
                    var ai = JsonSerializer.Deserialize<NetworkObjectProxy>(a?.ExtensionData["object"].ToString() ?? "{}");

                    if (ai.ExtensionData.TryGetValue("shouldThrow", out var shouldThrowValue))
                        ThrowException(shouldThrowValue);

                    var shouldFail = ai.ExtensionData.TryGetValue("shouldFail", out var shouldFailValue)
                        ? bool.TryParse(shouldFailValue.ToString(), out var shouldFailParsed) ? shouldFailParsed : false
                        : false;
                    var statusCode = shouldFail ? HttpStatusCode.BadRequest : HttpStatusCode.Accepted;
                    var content = shouldFail
                        ? new StringContent("An error has occurred")
                        : new StringContent(JsonSerializer.Serialize(
                            new Messages.ActivityPublicationResponse(
                                NetworkIdentity.New().ToString(), 
                                ai?.ExtensionData["id"].ToString())));

                    return new HttpResponseMessage()
                    {
                        StatusCode = statusCode,
                        Content = content
                    };
                });

            return handler.Object;
        }
    }

    public static void ThrowException(object? exceptionTypes)
    {
        var exceptionTypeNames = exceptionTypes?.ToString()?.Split(';');
        var outerExceptionName = exceptionTypeNames?.First();
        
        if (!string.IsNullOrWhiteSpace(outerExceptionName))
        {
            Exception? innerException = null;
            var innerNames = exceptionTypeNames?.Skip(1);
            if (innerNames?.Any() ?? false)
            {
                var innerExceptionName = innerNames.First();
                var innerExceptionType = Type.GetType(innerExceptionName);

                if ((innerExceptionType is not null) && typeof(Exception).IsAssignableFrom(innerExceptionType))
                    innerException = (Exception)Activator.CreateInstance(innerExceptionType)!;
            }

            Type? exceptionType = Type.GetType(outerExceptionName);
            if ((exceptionType is not null) && typeof(Exception).IsAssignableFrom(exceptionType))
                throw (Exception)Activator.CreateInstance(exceptionType, "This is a test exception", innerException)!;
        }
    }

    private class NetworkObjectProxy
    {
        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; } = new();
    }
}
