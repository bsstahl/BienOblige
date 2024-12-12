# Updating the Status of an Existing Action Item

* For business-specific scenarios, see our [MetroTransit Use-Case Studies](./MetroTransit/README.md).
* For updating the Status of a collection of existing ActionItems, see [Update Status of Tag](./update-status-of-tag.md).
* For assigning a Status to a new ActionItem, see [Create ActionItem](./create-actionitem.md).

## Updating the Status

To update the status, we need to describe an *Activity* of type *Update*, that specifies the *ActionItem* to search for, and the status we wish to assign to that *ActionItem*.

Every *Activity* requires an *Actor* that is performing the task and an *Object*. In this case, the *Object* is the identifier of the new status. *Update Activities* also require a Target, in this case, the identity of an *ActionItem* for which the status is being updated.

The following c# code snippet demonstrates the addition of a status to a single *ActionItem*:

```csharp
var activity = new UpdateStatusActivityBuilder()
    .CorrelationId(Guid.NewGuid())
    .Actor(new ActorBuilder()
        .Id("https://example.com/services/example-service-1")
        .ActorType(Enumerations.ActorType.Service))
    .Status(Status.Incomplete)
    .Target(new ObjectIdentifierBuilder()
        .Id(NetworkIdentity.From(baseUri, "ActionItem", "fd1cf331-c12d-4840-a197-ea2b08ddd240"))
        .AddObjectType("bienoblige:ActionItem")
        .AddObjectType("Object"))
    .Build();

var client = _services.GetRequiredService<ApiClient.Activities>();
var response = await client.Publish(activity);
```

We use the builder pattern with the *AddStatusActivityBuilder* to create a single *Activity* that describes the addition of the *Place* object representing the status, to the *ActionItem*. The *Actor* in this case is a service that is doing the status assignment, identified by the URI `https://example.com/services/example-service-1`. The *Status* and *ActionItem* are both specified only by their *@type* and *Id* properties. The *Activity* is then published using the *ApiClient*.

Note that **the *@type* field in the *Target* is critical** here. This is what let's the system know that it is looking for a specific *ActionItem* to add the status to. If the *Target @Type* is not specified correctly, the system may not be able to determine what to do with the *Status* object.

This code results in the following *Json* document being published by the *ApiClient* to the **Bien Oblige** API:

```json
{
    "@context": [
        "https://www.w3.org/ns/activitystreams",
        { "bienoblige": "https://bienoblige.com/ns" },
        { "schema": "https://schema.org" }
    ],
    "@type": "Update",
    "actor": {
        "@type": "Service",
        "id": "https://example.com/services/example-service-1"
    },
    "bienoblige:correlationId": "urn:uid:af56b4f5-7421-487a-a55c-133b0efce228",
    "id": "https://bienoblige.com/activity/1c91b71f-a0c8-4f54-9f22-4d81269d4585",
    "object": {
        "@type": [ "bienoblige:status", "Object" ],
        "id": "https://bienoblige.com/ns/status#Incomplete"
    },
    "published": "2024-12-11T13:37:26.8772485+00:00",
    "target": {
        "@type": [ "bienoblige:ActionItem", "Object" ],
        "id": "https://example.com/actionitem/fd1cf331-c12d-4840-a197-ea2b08ddd240"
    }
}
```
