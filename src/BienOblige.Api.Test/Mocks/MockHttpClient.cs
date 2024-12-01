using BienOblige.Api.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BienOblige.Api.Test.Mocks;

[ExcludeFromCodeCoverage]
internal class MockHttpClient : HttpClient
{
    private MockHttpMessageHandler _mockHttpMessageHandler;

    public IEnumerable<HttpContent> Requests => _mockHttpMessageHandler.RequestContent;

    public string JsonRequestMessages
    {
        get
        {
            var result = new List<string>();
            foreach (var request in this.Requests)
            {
                var task = request.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
                task.Wait();
                result.Add(task.Result);
            }
            return $"{string.Join(",", result)}";
        }
    }

    public IEnumerable<Activity> ActivityRequests
            => JsonSerializer.Deserialize<IEnumerable<Activity>>(this.JsonRequestMessages)
                ?? throw new ArgumentNullException(nameof(Activity));


    public MockHttpClient(MockHttpMessageHandler httpMessageHandler) 
        : base(httpMessageHandler.Object)
    {
        _mockHttpMessageHandler = httpMessageHandler;
        base.BaseAddress = new Uri("http://bienoblige.com/api/v1");
    }

}
