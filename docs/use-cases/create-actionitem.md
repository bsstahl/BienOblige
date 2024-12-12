# Creating an Action Item

For business-specific scenarios, see our [MetroTransit Use-Case Studies](./MetroTransit/README.md).

## Creating The Simplest Possible ActionItem

To create a simple action item, we need to describe an *Activity* of type *Create*, that specifies the *ActionItem* being created. Every *Activity* requires an *Actor* that is performing the task and an *Object*, in this case an *ActionItem* on which the *Activity* is being performed.

The following c# code snippet demonstrates the creation of a simple *ActionItem*:

```csharp
var activity = new CreateActionItemActivityBuilder()
    .Actor(new ActorBuilder()
        .Id("https://example.com/services/example-service-1")
        .ActorType(Enumerations.ActorType.Service))
    .ActionItem(new ActionItemBuilder()
        .Name("The Simplest Possible Action Item")
        .Content("This is the content of the simplest possible Action Item", "text/plain"))
    .Build();

var client = _services.GetRequiredService<ApiClient.Activities>();
var response = await client.Publish(activity);
```

We use the builder pattern with the *CreateActionItemActivityBuilder* to create a single *Activity* that describes the creation of the *ActionItem*. The *Actor* in this case is a service that is creating the *ActionItem*, identified by the URI `https://example.com/services/example-service-1`. The *ActionItem* has values for the *name* and *content* fields, which is the minimum information required for creation. Ids and other default information, such as Object types, publication date, and the default *completionMethod*, will be filled-in by the system. The *Activity* is then published using the *ApiClient*.

This code results in the following *Json* document being published by the *ApiClient* to the **Bien Oblige** API:

```json
{
    "@context": [
        "https://www.w3.org/ns/activitystreams",
        { "bienoblige": "https://bienoblige.com/ns" },
        { "schema": "https://schema.org" }
    ],
    "@type": "Create",
    "actor": {
        "@type": "Service",
        "id": "https://example.com/services/example-service-1",
        "name": null
    },
    "bienoblige:correlationId": "https://example.com/Activity/723f8bc6-e599-4e7d-a2d8-9099fc07797b",
    "id": "https://example.com/activity/bd6389d6-8387-4e1f-bbd9-65e14b526758",
    "object": {
        "@type": ["bienoblige:ActionItem","Object"],
        "bienoblige:completionMethods": ["Manual"],
        "content": "This is the content of the simplest possible Action Item",
        "id": "https://example.com/actionitem/1a3ff803-d53b-42b9-9f73-6bd4b9a32198",
        "mediaType": "text/plain",
        "name": "The Simplest Possible Action Item",
        "published": "2024-12-05T14:18:23.6853353+00:00"
    },
    "published": "2024-12-05T14:18:23.6853353+00:00"
}
```

This example shows the creation of a single *Activity* representing the creation of a single *ActionItem*. For more complex scenarios, such as creating multiple related tasks, or even a hierarchy of such tasks, we would use a similar pattern with the *CreateActionItemActivitiesBuilder* to describe the entire set of tasks to be created. For an example of this, see the [MetroTransit Bus Nightly Procedures case study](./MetroTransit/bus-nightly-procedures.md).
