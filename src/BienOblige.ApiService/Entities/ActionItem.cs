﻿using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BienOblige.ApiService.Entities;

public class ActionItem(string id, string title, string content)
{
    public string? Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;

    public Execution.Aggregates.ActionItem AsAggregate()
    {
        ArgumentNullException.ThrowIfNull(this.Id);
        return new Execution.Aggregates.ActionItem(
            ValueObjects.NetworkIdentity.From(this.Id),
            Execution.ValueObjects.Title.From(this.Title),
            Execution.ValueObjects.Content.From(this.Content))
        {  };
    }

    public static ActionItem From(Execution.Aggregates.ActionItem item)
    {
        return new ActionItem(item.Id.Value.ToString(), item.Title.Value, item.Content.Value);
    }

}
