# Bien Oblige: Ubiquitious Language

## ActionItem Management Nouns

> Note: *AS2* refers to the *Activity Streams 2.0* protocol.

The entities involved in *ActionItem* management are:

* **ActionItem** - a unit of work that will be performed by an *executor*.
  * Events describe modifications to the item including the process of performing the item
    * i.e. Created, Assigned, Completed, Cancelled, etc
  * Represented by an extension to the AS2 "Object" type called "ActionItem"
  * Fields
    * Standard AS2 *Object* fields
      * *attributedTo* (optional) - The **Executor** of the ActionItem
      * *content* - A detailed description of the work to be performed. If not supplied, the *name* field should be used as the content.
      * *context* (optional) - The context in which the work needs to be performed. Generally this refers to the reason for the work (i.e. an Audit, Customer Sale, etc.)
      * *endTime* (optional) - The date and time by which the work needs to be completed
      * *generator* (optional) - The **Actor** (Person, Application or Service) that created the ActionItem
      * *id* - The unique identifier for the ActionItem
      * *location* (optional) - The **Location** where the work is to be performed
      * *name* - A short description of the work to be performed that can be used like a title
      * *published* (optional) - The date and time of the creation of the **ActionItem**. Defaults to the *published* date and time on the Activity object on creation.
      * *summary* (optional) - A complete summary of the **ActionItem** including all relevant details. Defaults to the current UTC date and time on creation.
      * *tag* (optional) - a collection of objects representing characteristics of the work, used for searching and sorting. These objects will usually be custom types, although standard AS2 objects such as *person* may be used.
      * *type* - An array holding both the custom object type ("bienoblige:ActionItem") and the standard fallback AS2 type ("Object")
      * *updated* (optional) - The date and time of the last update to the **ActionItem**. Defaults to the *published* date and time on creation.
    * Standard fields on the *ActionItem* extension
      * *origin* (optional) - A reference to the original **ActionItem** that led to its creation. This is used to trace the lineage of work. Typically, this field is omitted or null, but it is populated when a new **ActionItem** is generated from an existing one, such as when a quality check fails on the original **ActionItem** and rework is required.
      * *target* (optional) - The entity on which the work is to be performed
    * Custom *ActionItem* fields
      * *bienoblige:completionMethod* (optional) - Indicates the means by which the work is to be marked as completed. Defaults to "bienoblige:Manual" on creation.
      * *bienoblige:effort* (optional) - an object indicating the expected workload for the work
      * *bienoblige:exceptions* (optional) - a collection of exceptions that have been raised on the *ActionItem*. This will be null or empty on creation and will be populated as exceptions are raised.
      * *bienoblige:executorRequirements* (optional) - a collection of objects representing the skills or other requirements needed by the **Executor** to perform the work.
      * *bienoblige:parent* (optional) - the parent ActionItem to which this ActionItem is a child. Allows a hierarchy of work to be created.
      * *bienoblige:prerequisites* (optional) - a collection of objects representing other **ActionItems** that must be completed before this work can be started.
      * *bienoblige:priority* (optional) - a value indicating the importance of the work.
      * *bienoblige:status* (optional) - the current state within the lifecycle of the *ActionItem*. Defaults to `https://bienoblige.com/ns/status#Incomplete` on creation.

> TODO: Among the *content*, *name* and *summary* fields, which should be required and which should be optional? It seems like only one of them should be required, but which one? The *summary* is the most complete...

* **Executor** - a person that is responsible for performing activities.
  * Represented by AS2 objects of type "Person"
* **Exception** - an additional status on an *ActionItem* that indicates a significant problem that likely needs user attention. The following exceptions are known, but there may be many others down the road.
  * Invalid Request - A request was made to place this *ActionItem* in an invalid state. Also occurs when an attempt to create a new *ActionItem* with the same Id as the existing one is made. The exception would be placed on the existing *ActionItem* and no additional *ActionItem* would be created.
  * Timeframe Exceeded - The *ActionItem* was not completed within a certain time frame of the due date and the *ActionItem* was not marked as complete or with a completion method that includes closing when *Expired*.
  * Asset Unavailable - The *ActionItem* requires an asset that is not available. This could be because the asset is not at the location where the work is to be performed, or the asset is broken and is not available for the work.
* **Location** - a place where ActionItems are performed
  * Represented by AS2 objects of type "Place".
  * Standard
    * https://bienoblige.com/ns/location/office
    * https://bienoblige.com/ns/location/home
  * Custom
    * https://example.com/ns/location/12345
* **ActionItem Type** - an optional collection of category Ids representing common characteristics of the *ActionItems* they are associated with.
  * These categories are defined by the consuming applications and are identified by URI
  * The URI may be used as a filter/sort criteria when listing *ActionItems*
* **CompletionMethod** - an indication of the method by which the *ActionItem* is to be completed
  * "bienoblige:Manual" - the default method where the *ActionItem* is considered complete when specifically identified as such by a client system, usually as a result of user interaction.
  * "bienoblige:AllChildrenCompleted" - the *ActionItem* is considered complete when all of its child *ActionItems* are complete. There must be at least 1 child *ActionItem*. If there are no children, the *ActionItem* can only be closed manually. This method is used for work that is broken down into smaller tasks that must all be completed to consider the work done.
  * "bienoblige:Expired" - the *ActionItem* is considered complete when the *endTime* has passed. This method is used when the work needs to be completed by a specific date and time or it no longer has value.
  * "bienoblige:ExpiredOrAllChildrenCompleted" - the *ActionItem* is considered complete when all if its child *ActionItems* are complete or the *endTime* has passed. This method is used when the work is broken down into smaller tasks that all must be completed to consider the work done, but it all needs to be completed by a specific date and time or it no longer has value.
* **Status** - the current state of completion of an ActionItem.
  * Standard
    * https://bienoblige.com/ns/status#Incomplete - Work is not done
    * https://bienoblige.com/ns/status#InProgress - Work is being done
    * https://bienoblige.com/ns/status#Complete - Work is done
    * https://bienoblige.com/ns/status#Cancelled - Work will not be done
  * Custom
    * https://example.com/ns/status#AwaitingApproval
* **Effort** - Quantifies the expected workload for an ActionItem
  * Includes a type URI to define its nature, allowing for adaptable representation across different systems.
    * Standard
      * https://bienoblige.com/ns/efforttype#Complexity - An enumeration describing the level of complexity of the work
      * https://bienoblige.com/ns/efforttype#Credit - A positive numeric value indicating the amount of credit an *Executor* will receive for completing the work
      * https://bienoblige.com/ns/efforttype#Time - A 
      * https://bienoblige.com/ns/efforttype#StoryPoints
    * Custom
      * https://example.com/ns/efforttype#12345
* **Update** - A change made to any portion of an ActionItem, including its creation, assignment, or change of status.
  * Represented by AS2 objects of type "Activity"
  * Each *Update* object describes a change made to the ActionItem. By applying all updates to an *ActionItem* entity, starting with its creation event, the current state of the *ActionItem* can be determined. See [Event Sourcing](https://learn.microsoft.com/en-us/azure/architecture/patterns/event-sourcing).
* **Priority** - a value indicating the importance of the work
  * Standard
    * https://bienoblige.com/ns/priority#Low
    * https://bienoblige.com/ns/priority#Medium
    * https://bienoblige.com/ns/priority#High
  * Custom
    * https://example.com/ns/priority#WhiteGlove

## ActionItem Management Verbs

* **Create** - The act of defining a new *ActionItem* that needs to be performed.
  * Represented by the AS2 "Activity" type
    * [Sample ActionItem Create Message](./messages/actionitem_create.json)
    * The above message is almost certainly wrong in numerous ways. Consider it a version 0.1.
* **Update** - The act of modifying an existing *ActionItem*. Any change to state or metadata of an *ActionItem* is considered an *Update*.
  * **Assign** - The act of identifying an *executor* responsible for performing the *ActionItem* and/or the *location* at which the work is to be performed.
  * **ChangeStatus** - The act of moving an *ActionItem* through its lifecycle. i.e. from *Not Done* to *Done*.

## Context

ActionItems are:

* Created by an *Actor* such as a *Person* or *Application*

ActionItems may be:

* Assigned to an *Actor* such as *Person* or *Group*
* Performed against a *Target* entity
  * Multiple target entities should be represented using multiple tasks
* Performed at a *Location*
  * Multiple locations should be represented using multiple tasks

*Executors* may be assigned an *ActionItem* by:

* An *Application*, perhaps based on an optimization algorithm
* An authorized entity of type "Person"
  * Authorization is managed by the consuming system
* Themselves, generally by selecting the ActionItem to be performed from a list

