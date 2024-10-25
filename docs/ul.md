# Bien Oblige: Ubiquitious Language

## ActionItem Management Nouns

> Note: *AS2* refers to the *Activity Streams 2.0* protocol.

The entities involved in *ActionItem* management are:

* **ActionItem** - a unit of work that will be performed by an *executor*.
  * Events describe modifications to the item including the process of performing the item
    * i.e. Created, Assigned, Completed, Cancelled, etc
  * Represented by an extension to the AS2 "Object" type called "ActionItem"
  * TODO: Describe each field in the ActionItem object and its intended usage
* **Executor** - a person that is responsible for performing activities.
  * Represented by AS2 objects of type "Person"
* **Location** - a place where ActionItems are performed
  * Represented by AS2 objects of type "Place".
  * Standard
    * https://bienoblige.com/ns/location/office
    * https://bienoblige.com/ns/location/home
  * Custom
    * https://example.com/ns/location/12345
* **ActionItem Type** - an optional collection of category Ids representing common characteristics of the *ActionItems* they are associated with.
  * These categories are defined by the consuming applications and are identified by URI
  * The URI may be used as a filter/sort criteria when listing ActionItems
* **Status** - the current state of completion of an ActionItem.
  * Standard
    * https://bienoblige.com/ns/status#notdone
    * https://bienoblige.com/ns/status#inprogress
    * https://bienoblige.com/ns/status#done
    * https://bienoblige.com/ns/status#cancelled
  * Custom
    * https://example.com/ns/status#awaitingapproval
* **Effort** - Quantifies the expected workload for an ActionItem
  * Includes a type URI to define its nature, allowing for adaptable representation across different systems.
    * Standard
      * https://bienoblige.com/ns/efforttype#CreditBased
      * https://bienoblige.com/ns/efforttype#ComplexityLevel
      * https://bienoblige.com/ns/efforttype#TimeEstimate
    * Custom
      * https://example.com/ns/efforttype#12345
* **Update** - A change made to any portion of an ActionItem, including its creation, assignment, or change of status.
  * Represented by AS2 objects of type "Activity"
  * Each *Update* object describes a change made to the ActionItem. By applying all updates to an *ActionItem* entity, starting with its creation event, the current state of the *ActionItem* can be determined. See [Event Sourcing](https://learn.microsoft.com/en-us/azure/architecture/patterns/event-sourcing).

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

