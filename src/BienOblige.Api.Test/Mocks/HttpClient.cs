using BienOblige.Api.Entities;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace BienOblige.Api.Test.Mocks;

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
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent("{'status':'success'}")
                });

            return handler.Object;
        }
    }

}
