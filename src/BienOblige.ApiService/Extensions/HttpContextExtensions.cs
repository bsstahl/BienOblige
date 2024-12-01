using BienOblige.Api.Entities;
using BienOblige.ApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Text;
using System.Text.Json;

namespace BienOblige.ApiService.Extensions;

public static class HttpContextExtensions
{
    public static async Task<string> GetRequestBody(this HttpContext context)
    {
        string result = string.Empty;

        context.Request.EnableBuffering(); // Enable buffering so that we can read the request body multiple times
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            context.Request.Body.Position = 0;
            result = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0; // Reset position for downstream middleware
        }

        return result;
    }

    public static async Task<string> GetResponseBody(this HttpContext context)
    {
        string result = string.Empty;

        using (var reader = new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true))
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            result = await reader.ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
        }

        return result;
    }

    public static async Task<IEnumerable<Activity>?> GetActivitiesCollection(this HttpContext context)
    {
        // TODO: Exception frames are a scalability killer. Try to avoid them.
        IEnumerable<Activity>? activities = null;
        try
        {
            context.Request.Body.Position = 0;
            activities = await context.Request.ReadFromJsonAsync<IEnumerable<Activity>>();
            context.Request.Body.Position = 0;
        }
        catch (JsonException)
        { }

        return activities;
    }

    public static async Task<Activity?> GetSingularActivity(this HttpContext context)
    {
        // TODO: Exception frames are a scalability killer. Try to avoid them.
        Activity? activity = null;
        try
        {
            context.Request.Body.Position = 0;
            activity = await context.Request.ReadFromJsonAsync<Activity>();
            context.Request.Body.Position = 0;
        }
        catch (JsonException)
        { }

        return activity;
    }

    public static async Task WriteRequestErrorResponse(this HttpContext context, string message, string body)
    {
        var details = new ProblemDetails()
        {
            Title = "Invalid Request",
            Detail = message + $" Payload: {body}",
            Status = 400,
            Instance = context.Request.Path
        };

        await context.WriteResponse(details);

        //var (stream, length) = CreateStream(details);

        //context.Response.StatusCode = 400;
        //context.Response.ContentType = "application/problem+json";
        //context.Request.Body = stream;
        //context.Request.ContentLength = length;
        //context.Request.EnableBuffering();
        //context.Request.Body.Position = 0;

        //return Task.CompletedTask;
    }

    public static void RewriteRequest(this HttpContext context, List<Activity>? activities)
    {
        // Rewrite the request body with changes
        var (stream, length) = CreateStream<IEnumerable<Activity>>(activities);
        context.Request.Body = stream;
        context.Request.ContentLength = length;
        context.Request.EnableBuffering();
        context.Request.Body.Position = 0;
    }

    public static async Task WriteResponse<T>(this HttpContext context, T responseData)
    {
        var payload = JsonSerializer.Serialize(responseData);
        await context.Response.WriteAsync(payload);

        //var (stream, length) = CreateStream(publicationResult);
        //context.Response.Body = stream;
        //context.Response.ContentLength = length;
    }

    //public static async Task<IEnumerable<PublicationResult>> GetPublicationResults(this HttpContext context)
    //{
    //    // Get the HTTP Response and deserialize it into a collection of PublicationResult objects
    //    var responseBody = await context.GetResponseBody();
    //    return JsonSerializer.Deserialize<IEnumerable<PublicationResult>>(responseBody) ?? Array.Empty<PublicationResult>();
    //}

    private static (Stream Stream, int Length) CreateStream<T>(T value)
    {
        var payload = JsonSerializer.Serialize(value);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);
        var stream = new MemoryStream(payloadBytes);
        stream.Flush();
        stream.Position = 0;
        return (stream, payloadBytes.Length);
    }
}
