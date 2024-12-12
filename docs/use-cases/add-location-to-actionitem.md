# Adding a Location to an Existing Action Item

* For business-specific scenarios, see our [MetroTransit Use-Case Studies](./MetroTransit/README.md).
* For adding a Location to a collection of ActionItems, see [Add Location to Tag](./add-location-to-tag.md).
* For assigning a Location to a new ActionItem, see [Create ActionItem](./create-actionitem.md).

## Adding the Location

To add the location, we need to describe an *Activity* of type *Add*, that specifies the *ActionItem* being modified and the location we wish to assign it to.

Every *Activity* requires an *Actor* that is performing the task and an *Object*, in this case, the *Object* is the *Place* identifying the location where the work is to be performed. *Add Activities* also require a Target, in this case, the identity of an *ActionItem* that the location is being added to.

TODO: What should happen if this ActionItem is not in a "to be done" like state. If the ActionItem is already in progress, completed, or cancelled, should the location still be updated or should the behavior be most consistent with the add location by tag functionality?

The following c# code snippet demonstrates the addition of a location to a single *ActionItem*:

```csharp
var activity = new AddLocationActivityBuilder()
    .CorrelationId(Guid.NewGuid())
    .Actor(new ActorBuilder()
        .Id("https://example.com/services/example-service-1")
        .ActorType(Enumerations.ActorType.Service))
    .Location(new LocationBuilder()
        .Id(NetworkIdentity.From(baseUri, "Location", "Phoenix"))
        .Name("The company's Phoenix AZ location"))
    .Target(new ObjectIdentifierBuilder()
        .Id(NetworkIdentity.From(baseUri, "ActionItem", "fd1cf331-c12d-4840-a197-ea2b08ddd240"))
        .AddObjectType("bienoblige:ActionItem")
        .AddObjectType("Object"))
    .Build();

var client = _services.GetRequiredService<ApiClient.Activities>();
var response = await client.Publish(activity);
```

We use the builder pattern with the *AddLocationActivityBuilder* to create a single *Activity* that describes the addition of the *Place* object representing the location, to the *ActionItem*. The *Actor* in this case is a service that is doing the location assignment, identified by the URI `https://example.com/services/example-service-1`. The *Location* and *ActionItem* are both specified only by their *@type* and *Id* properties. The *Activity* is then published using the *ApiClient*.

Note that **the *@type* field in the *Target* is critical** here. This is what let's the system know that it is looking for a specific *ActionItem* to add the location to. If the *Target @Type* is not specified correctly, the system may not be able to determine what to do with the *Location* object.

This code results in the following *Json* document being published by the *ApiClient* to the **Bien Oblige** API:

```json
{
    "@context": [
        "https://www.w3.org/ns/activitystreams",
        { "bienoblige": "https://bienoblige.com/ns" },
        { "schema": "https://schema.org" }
    ],
    "@type": "Add",
    "actor": {
        "@type": "Application",
        "id": "https://example.com/application/3aad2511-fe7f-45f8-bdb8-4abe2ba8873f",
        "name": "Activities_PublishSinglular_Should.PublishAValidLocationAssignmentMessage"
    },
    "bienoblige:correlationId": "urn:uid:af56b4f5-7421-487a-a55c-133b0efce228",
    "id": "https://bienoblige.com/activity/1c91b71f-a0c8-4f54-9f22-4d81269d4585",
    "object": {
        "@type": [ "Place" ],
        "id": "https://example.com/location/94f6cf65-568f-49b7-a501-95e5e9371c6a",
        "name": "The company's Phoenix AZ location"
    },
    "published": "2024-12-11T13:37:26.8772485+00:00",
    "target": {
        "@type": [ "bienoblige:ActionItem", "Object" ],
        "id": "https://example.com/actionitem/fd1cf331-c12d-4840-a197-ea2b08ddd240"
    }
}
```
