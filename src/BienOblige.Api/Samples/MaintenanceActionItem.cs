using BienOblige.Api.Entities;
using System.Text.Json.Serialization;

namespace BienOblige.Api.Samples;

/// <summary>
/// A sample custom extension to the <see cref="ActionItem"/> entity for fleet and facility maintenance tasks.
/// Demonstrates extending the ActionItem with domain-specific properties, a custom Activity Streams 2.0
/// type hierarchy, and an associated namespace context for full AS2/JSON-LD compliance.
/// </summary>
public class MaintenanceActionItem : ActionItem
{
    /// <summary>
    /// The namespace URI defining the custom extension vocabulary.
    /// </summary>
    public const string ExtensionNamespace = "https://metrotransit.com/ns";

    /// <summary>
    /// The preferred prefix for the custom extension namespace.
    /// </summary>
    public const string ExtensionPrefix = "metrotransit";

    /// <summary>
    /// The qualified type name for this extended ActionItem.
    /// </summary>
    public const string MaintenanceObjectTypeName = "metrotransit:MaintenanceActionItem";

    /// <summary>
    /// The default object type array containing the specific custom type,
    /// followed by the Bien Oblige type and the AS2 fallback type.
    /// </summary>
    public static readonly string[] DefaultMaintenanceObjectTypes =
    [
        MaintenanceObjectTypeName,
        "bienoblige:ActionItem",
        "Object"
    ];

    public MaintenanceActionItem()
    {
        ObjectType = DefaultMaintenanceObjectTypes.ToList();
    }

    /// <summary>
    /// An external work order tracking identifier (e.g., from an ERP or CMMS system).
    /// </summary>
    [JsonPropertyName("metrotransit:workOrderNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? WorkOrderNumber { get; set; }

    /// <summary>
    /// The estimated cost to complete the maintenance work in US dollars.
    /// </summary>
    [JsonPropertyName("metrotransit:estimatedCost")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? EstimatedCost { get; set; }

    /// <summary>
    /// Indicates whether the task requires regulatory or technical certification upon completion.
    /// </summary>
    [JsonPropertyName("metrotransit:requiresCertification")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? RequiresCertification { get; set; }

    /// <summary>
    /// The regulatory or compliance authority governing this maintenance task (e.g., "DOT", "FTA", "OSHA").
    /// </summary>
    [JsonPropertyName("metrotransit:regulatoryAuthority")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RegulatoryAuthority { get; set; }
}
