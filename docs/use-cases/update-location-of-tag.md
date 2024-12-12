# Updating the Location of a Collection of Existing Action Items

* For business-specific scenarios, see our [MetroTransit Use-Case Studies](./MetroTransit/README.md).
* For updating the Location of a single existing ActionItem, see [Update Location of ActionItem](./update-location-of-actionitem.md).
* For assigning a Location to a new ActionItem, see [Create ActionItem](./create-actionitem.md).

## Updating the Location

To update the location, we need to describe an *Activity* of type *Update*, that specifies the *Tag* to search for, and the location we wish to assign to *ActionItems* tagged with that identifier.

Every *Activity* requires an *Actor* that is performing the task and an *Object*, in this case, the *Object* is the *Place* identifying the location where the work is to be performed. *Update Activities* also require a Target, in this case, the identity of a *Tag* that decorates the *ActionItems* we want assigned to be worked on at the specified location.

**Note**: This action only results in a change when the ActionItem has not yet been started. If an *ActionItem* is already in progress, completed, or cancelled, the ActionItem will not be modified using this method.

The following c# code snippet demonstrates the update of a location to a collection of *ActionItems* that are all tagged for "White Glove Service" using the *id* `https://example.com/specialcare/WhiteGlove`:

```csharp
var activity = new UpdateLocationActivityBuilder()
    .CorrelationId(Guid.NewGuid())
    .Actor(new ActorBuilder()
        .Id(NetworkIdentity.From(baseUri, "Service", Guid.NewGuid().ToString()))
        .ActorType(Enumerations.ActorType.Application)
        .Name($"{this.GetType().Name}.{nameof(PublishAValidLocationAssignmentByTagMessage)}"))
    .Location(new LocationBuilder()
        .Id(NetworkIdentity.From(baseUri, "Location", "SWRegionalHQ"))
        .Name("The company's SouthWest Regional Headquarters"))
    .Target(new ObjectIdentifierBuilder()
        .Id(NetworkIdentity.From(baseUri, "SpecialCare", "WhiteGlove"))
        .Name("White Glove Service")
        .AddObjectType("tag"))
    .Build();

var client = _services.GetRequiredService<ApiClient.Activities>();
var response = await client.Publish(activity);
```

We use the builder pattern with the *UpdateLocationActivityBuilder* to create a single *Activity* that describes the addition of the *Place* object representing the location, to the *ActionItem*. The *Actor* in this case is a service that is doing the location assignment, identified by the URI `https://example.com/services/example-service-1`. The *Location* and *ActionItem* are both specified only by their *@type* and *Id* properties. The *Activity* is then published using the *ApiClient*.

Note that **the *@type* field in the *Target* is critical** here. This is what let's the system know that it is looking for a specific *ActionItem* to add the location to. If the *Target @Type* is not specified correctly, the system may not be able to determine what to do with the *Location* object.

The *name* property in the *Target* and *Location* objects are both optional and are ignored by the system. They are available for human readability and are not used in the processing of the *Activity*.

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
        "id": "https://example.com/service/8384f24d-8990-4ac2-8126-0a29c1fc1b36",
        "name": "Activities_PublishSinglular_Should.PublishAValidLocationAssignmentByTagMessage"
    },
    "bienoblige:correlationId": "urn:uid:eadd6202-83a2-438b-b113-ac86898f3ddb",
    "id": "https://bienoblige.com/activity/b0d2b5ae-6b6e-484b-ada0-bcd359299946",
    "object": {
        "@type": [ "Place" ],
        "id": "https://example.com/location/SWRegionalHQ",
        "name": "The company's SouthWest Regional HQ"
    },
    "published": "2024-12-12T01:02:48.199055+00:00",
    "target": {
        "@type": [ "tag" ],
        "id": "https://example.com/specialcare/WhiteGlove"
    }
}
```
