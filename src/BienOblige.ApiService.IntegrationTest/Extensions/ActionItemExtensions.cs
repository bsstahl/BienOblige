using BienOblige.Api.Entities;
using System.Net.Http.Json;

namespace BienOblige.ApiService.IntegrationTest.Extensions;

public static class ActionItemExtensions
{
    public static JsonContent AsJsonContent(this ActionItem actionItem)
    {
        return JsonContent.Create(new
        {
            Id = actionItem.Id,
            Name = actionItem.Name,
            Content = actionItem.Content
        });
    }
}
