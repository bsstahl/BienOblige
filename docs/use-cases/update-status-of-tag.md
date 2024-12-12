# Updating the Status of a Collection of Existing Action Items

* For business-specific scenarios, see our [MetroTransit Use-Case Studies](./MetroTransit/README.md).
* For updating the Status of a single existing ActionItem, see [Update Status of ActionItem](./update-status-of-actionitem.md).
* For assigning a Status to a new ActionItem, see [Create ActionItem](./create-actionitem.md).

## Updating the Status

To update the status, we need to describe an *Activity* of type *Update*, that specifies the *Tag* to search for, and the status we wish to assign to *ActionItems* tagged with that identifier.

Every *Activity* requires an *Actor* that is performing the task and an *Object*, in this case, the *Object* is the *Place* identifying the status where the work is to be performed. *Update Activities* also require a Target, in this case, the identity of a *Tag* that decorates the *ActionItems* we want to update the status of.

**Note**: This action only results in a change when the ActionItem has not yet been started. If an *ActionItem* is already in progress, completed, or cancelled, the ActionItem will not be modified using this method.

The following c# code snippet demonstrates the update of a status to a collection of *ActionItems* that are all tagged for "White Glove Service" using the *id* `https://example.com/specialcare/WhiteGlove`:

```csharp
var activity = new UpdateStatusActivityBuilder()
    .CorrelationId(Guid.NewGuid())
    .Actor(new ActorBuilder()
        .Id(NetworkIdentity.From(baseUri, "Service", Guid.NewGuid().ToString()))
        .ActorType(Enumerations.ActorType.Service))
    .Status(Status.Incomplete)
    .Target(new ObjectIdentifierBuilder()
        .Id(NetworkIdentity.From(baseUri, "SpecialCare", "WhiteGlove"))
        .Name("White Glove Service")
        .AddObjectType("tag"))
    .Build();

var client = _services.GetRequiredService<ApiClient.Activities>();
var response = await client.Publish(activity);
```

We use the builder pattern with the *UpdateStatusActivityBuilder* to create a single *Activity* that describes the modification to the *Status* property of the *ActionItem*. The *Actor* in this case is a service that is doing the status assignment, identified by the URI `https://example.com/services/example-service-1`. The *Status* and *ActionItem* are both specified only by their *@type* and *Id* properties. The *Activity* is then published using the *ApiClient*.

Note that **the *@type* field in the *Target* is critical** here. This is what let's the system know that it is looking for a specific *ActionItem* to add the status to. If the *Target @Type* is not specified correctly, the system may not be able to determine what to do with the *Status* object.

The *name* property in the *target* and *object* are both optional and are ignored by the system. They are available for human readability and are not used in the processing of the *Activity*.

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
        "@type": "Application",
        "id": "https://example.com/service/8384f24d-8990-4ac2-8126-0a29c1fc1b36"
    },
    "bienoblige:correlationId": "urn:uid:eadd6202-83a2-438b-b113-ac86898f3ddb",
    "id": "https://bienoblige.com/activity/b0d2b5ae-6b6e-484b-ada0-bcd359299946",
    "object": {
        "@type": [ "bienoblige:status" ],
        "id": "https://bienoblige.com/ns/status#Incomplete"
    },
    "published": "2024-12-12T01:02:48.199055+00:00",
    "target": {
        "@type": [ "tag" ],
        "name": "White Glove Service",
        "id": "https://example.com/specialcare/WhiteGlove"
    }
}
```
