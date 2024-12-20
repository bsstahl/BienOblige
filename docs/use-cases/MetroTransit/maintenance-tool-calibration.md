# Use Case: Tool Calibration for MetroTransit Mechanics

This use case demonstrates the use of Bien Oblige to create and manage a single task.

## MetroTransit Business Objective

To ensure that mechanics' tools are properly calibrated and ready for use.

For a description of the MetroTransit maintenance system and actors, see the [MetroTransit Use Case Overview](./README.md).

## Simple Task Creation

Each week, MetroTransit mechanics are required to calibrate their digital torque wrenches to ensure they are accurately measuring the rotational force used to tighten critical parts on the busses. Calibrations are good for 10 days, after which, the tool cannot be used on MetroTransit busses.

The task is created by iteslef with no prerequisites, and is completed manually by the mechanic. It is created by a process owned by the maintenance technology team known as *Compliance Studio*. Since the current iteration of **Bien Oblige** does not yet support recurring tasks, this service is a requirement. Once recurring tasks are supported, the need for this service will be re-evaluated.

### Task Details

* **Weekly Digital Torque Wrench Calibration** - A task assigned to mechanics to perform a weekly calbration on a critical tool.
  * Prerequisites: None
  * Completion method: Manual
  * EndTime: 10 days from the completion of the last calibration
  * Executor: The mechanic to whom the wrench is assigned
  * Target: The digital torque wrench assigned to that mechanic

### Task Creation Code

The following *c#* code lives in the *Compliance Studio* service and demonstrates the creation of the task. The calibration tasks are created with the appropriate completion times, and are assigned to the mechanic assigned to that tool. Since there is no custom target object in the **Bien Oblige** system that is appropriate for a tool at this time, a generic object is used to describe the wrench. This task does not have to have its Id specified in the code, as it it can be generated by the system.

```csharp
var activity = new ActivityBuilder()
    .CorrelationId(Guid.NewGuid())
    .ActivityType(Enumerations.ActivityType.Create)
    .Actor(new ActorBuilder()
        .Id(complianceStudioServiceId)
        .ActorType(Enumerations.ActorType.Service)
        .Name("Compliance Studio"))
    .ActionItem(new ActionItemBuilder()
        .Id(Guid.NewGuid())
        .Name("Weekly Digital Torque Wrench Calbration")
        .Content("Calibrate the digital torque wrench to a tolerance of 0.5 in-lbs", "text/plain")
        .Audience(new ActorBuilder()
            .Id(NetworkIdentity.From(baseUrl, "user", "JaneWrencher"))
            .ActorType(Enumerations.ActorType.Person)
            .Name("Jane Wrencher"))
        .Target(new ObjectBuilder()
            .Id(NetworkIdentity.From(baseUrl, "Tool", "digi-torque-124"))
            .AddObjectType("Object")
            .Name("Digital Torque Wrench #124")))
    .Build();

var client = _services.GetRequiredService<ApiClient.Activities>();
var response = await client.Publish(activity);
```

This code results in the creation of the JSON payload shown below. This structure is posted to the **Bien Oblige** *inbox* to create the task. Unspectified Identifiers are assigned by the system using the base URL of that instance. For the *MetroTransit* instance, the base URL is **`https://metrotransit.com`**.

```json
{
    "@context": [ "https://www.w3.org/ns/activitystreams", 
        { "bienoblige": "https://bienoblige.com/ns" }, 
        { "schema":"https://schema.org" } ],
    "@type": "Create",
    "actor": {
        "@type": "Service",
        "id": "https://metrotransit.com/service/compliance-studio",
        "name": "Compliance Studio"
    },
    "bienoblige:correlationId": "urn:uid:ea0a94f1-91f0-4ae6-842a-3eb9081bf9f7",
    "id": "https://metrotransit.com/activity/95347f00-50ca-4874-a688-f80837f08503",
    "object": {
        "@type": [ "bienoblige:ActionItem", "Object" ],
        "audience": { 
            "@type": [ "Person" ],
            "id": "https://metrotransit.com/user/JaneWrencher",
            "name": "Jane Wrencher"
        },
        "bienoblige:completionMethods": [ "Manual" ],
        "bienoblige:target": {
            "@type": [ "Object" ],
            "id": "https://metrotransit.com/tool/digi-torque-124",
            "name": "Digital Torque Wrench #124"
        },
        "content": "Calibrate the digital torque wrench to a tolerance of 0.5 in-lbs",
        "id": "https://metrotransit.com/ActionItem/a313751c-ac5d-4b47-a797-bc5da49574ea",
        "mediaType": "text/plain",
        "name": "Weekly Digital Torque Wrench Calbration",
        "published": "2024-12-03T16:14:31.7669947+00:00"
    },
    "published": "2024-12-03T16:14:31.7669947+00:00"
}
```
