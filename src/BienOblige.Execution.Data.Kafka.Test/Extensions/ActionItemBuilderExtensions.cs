﻿using BienOblige.Execution.Builders;

namespace BienOblige.Execution.Data.Kafka.Test.Extensions;

[ExcludeFromCodeCoverage]
internal static class ActionItemBuilderExtensions
{
    internal static ActionItemBuilder UseRandomValues(this ActionItemBuilder builder)
    {
        var idValue = Guid.NewGuid().ToString();
        return builder
            .Id($"https://example.org/{idValue}")
            .Title($"Title of task {idValue}");
    }
}