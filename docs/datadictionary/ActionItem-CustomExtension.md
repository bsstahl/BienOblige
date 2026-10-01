# Extending the ActionItem Object

This sample demonstrates how to extend the **ActionItem** object by adding custom fields and domain-specific data elements while maintaining full **Activity Streams 2.0 (AS2)** and **JSON-LD** compliance.

## Overview

In the **Bien Oblige** system, tasks and work units are represented by the [ActionItem](ActionItem.md) entity. Client applications often need to attach domain-specific metadata or business logic properties to these tasks—such as work order numbers, estimated repair costs, compliance flags, or external system references.

Bien Oblige supports entity customization through standard AS2 and JSON-LD extension mechanisms:
- Custom properties added to an ActionItem are preserved by Bien Oblige as the entity travels through its lifecycle.
- Extensions follow AS2 best practices: defining a custom namespace in the `@context`, declaring a type hierarchy in `@type`, and prefixing custom properties with the namespace prefix.

---

## Best Practices for AS2 Compliance

### 1. Specifying Context (`@context`)
AS2 relies on JSON-LD contexts to map property and type names to URIs. When introducing custom fields or types:
- Define a custom namespace prefix and URI (e.g., `"metrotransit": "https://metrotransit.com/ns"`).
- Per [ADR 20241205_Context](../decisions/ADR_20241205_Context.md), declare custom contexts at the top-level **Activity** object's `@context` array alongside the standard namespaces (`https://www.w3.org/ns/activitystreams`, `bienoblige`, and `schema`). This makes the namespace available to all child objects, including the ActionItem.

### 2. Type Hierarchy (`@type`)
AS2 supports multiple types for an object using an array. When extending `ActionItem`:
- Specify the most specific custom type first (e.g., `"metrotransit:MaintenanceActionItem"`).
- Include `"bienoblige:ActionItem"` to ensure Bien Oblige services recognize and process the entity as an ActionItem.
- Include the standard AS2 fallback `"Object"` at the end of the array.

Example:
```json
"@type": [
  "metrotransit:MaintenanceActionItem",
  "bienoblige:ActionItem",
  "Object"
]
```

### 3. Namespace-Prefixed Property Names
To avoid collisions with standard AS2 or Bien Oblige properties:
- Prefix every custom property name with the namespace prefix (e.g., `metrotransit:workOrderNumber`, `metrotransit:estimatedCost`).

---

## Sample Scenario: MetroTransit Maintenance ActionItem

In this scenario, a municipal transit authority (**MetroTransit**) issues a maintenance task for a bus pneumatic braking system. They require custom fields on the ActionItem:

| Field | Type | Description | Example |
|---|---|---|---|
| `metrotransit:workOrderNumber` | `string` | Unique tracking identifier in the fleet maintenance system | `"WO-2025-0842"` |
| `metrotransit:estimatedCost` | `decimal` | Projected repair/service cost in USD | `450.00` |
| `metrotransit:requiresCertification` | `boolean` | Indicates whether certified technician sign-off is required | `true` |
| `metrotransit:regulatoryAuthority` | `string` | Oversight agency governing this maintenance procedure | `"DOT"` |

---

## C# Implementation

### 1. Defining the Custom Extension Class

Inherit from `BienOblige.Api.Entities.ActionItem` and annotate custom properties with `[JsonPropertyName]` using the namespace prefix:

```csharp
using BienOblige.Api.Entities;
using System.Text.Json.Serialization;

namespace MetroTransit.Entities;

/// <summary>
/// A custom extension of the Bien Oblige ActionItem representing vehicle maintenance tasks.
/// </summary>
public class MaintenanceActionItem : ActionItem
{
    public const string NamespacePrefix = "metrotransit";
    public const string NamespaceUri = "https://metrotransit.com/ns";
    public const string ObjectType = "metrotransit:MaintenanceActionItem";

    [JsonPropertyName("metrotransit:workOrderNumber")]
    public string? WorkOrderNumber { get; set; }

    [JsonPropertyName("metrotransit:estimatedCost")]
    public decimal? EstimatedCost { get; set; }

    [JsonPropertyName("metrotransit:requiresCertification")]
    public bool RequiresCertification { get; set; }

    [JsonPropertyName("metrotransit:regulatoryAuthority")]
    public string? RegulatoryAuthority { get; set; }

    public MaintenanceActionItem()
    {
        // AS2 type hierarchy: most specific type first, followed by Bien Oblige ActionItem, and AS2 Object
        Type = new[]
        {
            ObjectType,
            "bienoblige:ActionItem",
            "Object"
        };
    }
}
```

### 2. Creating and Publishing via `CreateActionItemActivityBuilder`

Use the fluent builder to create the Activity, attach the custom ActionItem, and declare the custom context namespace:

```csharp
using BienOblige.Api.Builders;
using BienOblige.Api.Entities;
using BienOblige.ApiClient;
using MetroTransit.Entities;

// Instantiate the custom ActionItem
var maintenanceTask = new MaintenanceActionItem
{
    Id = new Uri("https://metrotransit.com/ActionItem/33cce104-5387-4185-9ebd-9b6d2a2cb49d"),
    Name = "Annual Pneumatic Brake System Certification",
    Content = "Perform comprehensive diagnostic test and pressure tolerance certification of braking system",
    Audience = new Actor
    {
        Id = new Uri("https://metrotransit.com/user/JaneWrencher"),
        Type = new[] { "Person" },
        Name = "Jane Wrencher"
    },
    WorkOrderNumber = "WO-2025-0842",
    EstimatedCost = 450.00m,
    RequiresCertification = true,
    RegulatoryAuthority = "DOT",
    Target = new NetworkObject
    {
        Id = new Uri("https://metrotransit.com/bus/BUS-4022"),
        Type = new[] { "schema:Car", "Object" },
        Name = "MetroTransit Bus #4022"
    }
};

// Build the Create Activity with custom namespace context
var activity = new CreateActionItemActivityBuilder()
    .Id(new Uri("https://metrotransit.com/activity/d5c3b0c1-c471-4986-84e4-eb6e635d2a5e"))
    .Actor(new Uri("https://metrotransit.com/service/compliance-studio"), "Compliance Studio")
    .CorrelationId(new Uri("urn:uid:978fba99-8e26-44a6-9f34-1ab00d2acc1c"))
    .AddContext(MaintenanceActionItem.NamespacePrefix, MaintenanceActionItem.NamespaceUri)
    .ActionItem(maintenanceTask)
    .Build();

// Publish to Bien Oblige API inbox
var response = await apiClient.Publish(activity);
```

### 3. Alternative: Using `ActionItemBuilder` Fluent Extensions

If you prefer using the fluent builder without defining a new C# class:

```csharp
var activity = new CreateActionItemActivityBuilder()
    .Id(new Uri("https://metrotransit.com/activity/db9efbc6-beb5-4a04-b7be-ac5b504489f6"))
    .Actor(new Uri("https://metrotransit.com/service/compliance-studio"), "Compliance Studio")
    .AddContext("metrotransit", "https://metrotransit.com/ns")
    .ActionItem(item => item
        .Id(new Uri("https://metrotransit.com/ActionItem/44380b44-91a7-4582-8344-2971fb89ed99"))
        .AddObjectType("metrotransit:MaintenanceActionItem")
        .Name("Wheel Alignment Verification")
        .Content("Check front-axle wheel alignment following curb strike report", "text/plain")
        .AddAdditionalProperty("metrotransit:workOrderNumber", "WO-2025-0999")
        .AddAdditionalProperty("metrotransit:estimatedCost", 175.50m)
        .AddAdditionalProperty("metrotransit:requiresCertification", false)
        .AddAdditionalProperty("metrotransit:regulatoryAuthority", "Internal"))
    .Build();
```

### 4. Deserializing Network Objects to Custom Types

When receiving messages from the Bien Oblige stream or API, use the `AsActionItem<T>()` extension method to deserialize the generic `NetworkObject` into your strongly-typed subclass:

```csharp
using BienOblige.Api.Extensions;
using MetroTransit.Entities;

// Given an Activity received from the inbox or feed
Activity activity = ...;

if (activity.Object is NetworkObject networkObj &&
    networkObj.Type?.Contains("metrotransit:MaintenanceActionItem") == true)
{
    // Strongly-typed deserialization preserving all custom properties
    MaintenanceActionItem item = networkObj.AsActionItem<MaintenanceActionItem>();

    Console.WriteLine($"Work Order: {item.WorkOrderNumber}");
    Console.WriteLine($"Estimated Cost: ${item.EstimatedCost}");
    Console.WriteLine($"Requires Certification: {item.RequiresCertification}");
}
```

---

## JSON Message Samples

### 1. Complete Create Activity JSON Output

The following JSON message represents the output produced by publishing the `MaintenanceActionItem` using the C# sample code above:

```json
[
  {
    "@context": [
      "https://www.w3.org/ns/activitystreams",
      {
        "bienoblige": "https://bienoblige.com/ns"
      },
      {
        "schema": "https://schema.org"
      },
      {
        "metrotransit": "https://metrotransit.com/ns"
      }
    ],
    "id": "https://metrotransit.com/activity/d5c3b0c1-c471-4986-84e4-eb6e635d2a5e",
    "bienoblige:correlationId": "urn:uid:978fba99-8e26-44a6-9f34-1ab00d2acc1c",
    "@type": "Create",
    "actor": {
      "id": "https://metrotransit.com/service/compliance-studio",
      "@type": "Service",
      "name": "Compliance Studio"
    },
    "object": {
      "id": "https://metrotransit.com/ActionItem/33cce104-5387-4185-9ebd-9b6d2a2cb49d",
      "@type": [
        "metrotransit:MaintenanceActionItem",
        "bienoblige:ActionItem",
        "Object"
      ],
      "name": "Annual Pneumatic Brake System Certification",
      "content": "Perform comprehensive diagnostic test and pressure tolerance certification of braking system",
      "audience": {
        "id": "https://metrotransit.com/user/JaneWrencher",
        "@type": [
          "Person"
        ],
        "name": "Jane Wrencher"
      },
      "metrotransit:workOrderNumber": "WO-2025-0842",
      "metrotransit:estimatedCost": 450.00,
      "metrotransit:requiresCertification": true,
      "metrotransit:regulatoryAuthority": "DOT",
      "bienoblige:target": {
        "id": "https://metrotransit.com/bus/BUS-4022",
        "@type": [
          "schema:Car",
          "Object"
        ],
        "name": "MetroTransit Bus #4022"
      },
      "bienoblige:completionMethods": []
    },
    "target": null,
    "published": "2026-10-01T02:42:12.0119205+00:00"
  }
]
```

### 2. Standalone ActionItem Object JSON Representation

When inspected directly or stored as an entity, the serialized `MaintenanceActionItem` object appears as follows:

```json
{
  "id": "https://metrotransit.com/ActionItem/33cce104-5387-4185-9ebd-9b6d2a2cb49d",
  "@type": [
    "metrotransit:MaintenanceActionItem",
    "bienoblige:ActionItem",
    "Object"
  ],
  "name": "Annual Pneumatic Brake System Certification",
  "content": "Perform comprehensive diagnostic test and pressure tolerance certification of braking system",
  "audience": {
    "id": "https://metrotransit.com/user/JaneWrencher",
    "@type": [
      "Person"
    ],
    "name": "Jane Wrencher"
  },
  "metrotransit:workOrderNumber": "WO-2025-0842",
  "metrotransit:estimatedCost": 450.00,
  "metrotransit:requiresCertification": true,
  "metrotransit:regulatoryAuthority": "DOT",
  "bienoblige:target": {
    "id": "https://metrotransit.com/bus/BUS-4022",
    "@type": [
      "schema:Car",
      "Object"
    ],
    "name": "MetroTransit Bus #4022"
  },
  "bienoblige:completionMethods": []
}
```

---

## Related References

- [ActionItem Data Dictionary](ActionItem.md)
- [Activity Data Dictionary](Activity.md)
- [Create Activity Data Dictionary](Activity-Create.md)
- [ADR 20241205: Context Specification](../decisions/ADR_20241205_Context.md)
- [W3C Activity Streams 2.0 Core Specification](https://www.w3.org/TR/activitystreams-core/)
