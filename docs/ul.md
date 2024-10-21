# Bien Oblige: Ubiquitious Language

## ActionItem Management Nouns

The entities involved in *ActionItem* management are:

* **Executor** - a person that is responsible for performing activities.
  * Represented by Activity Streams 2.0 (AS2) objects of type "Person"
* **Supervisor** - A person with the ability to manually assign ActionItems to executors.
  * Represented by Activity Streams 2.0 (AS2) objects of type "Person".
* **Location** - a place where ActionItems are performed
  * Represented by Activity Streams 2.0 (AS2) objects of type "Place".
* **ActionItem** - a unit of work that will be performed by an executor.
  * The event object describes the process of performing the item
  * Represented by an extension to the Activity Streams 2.0 (AS2) "Object" type called "ActionItem"
* **ActionItem Type** - a category of ActionItems that share common characteristics
  * These categories are defined by the consuming applications and are identified by URI
  * The URI may be used as a filter/sort criteria when listing ActionItems
* **Status** - the current state of completion of an ActionItem.
  * Options include "Not Done", "Done", and "Cancelled"

* **Update** - A change made to any portion of an ActionItem, including its creation, assignment, or change of status.
  * Represented by Activity Streams 2.0 (AS2) objects of type "Activity"
  * Each *Update* object describes a change made to the ActionItem. By applying all updates to an *ActionItem* entity, starting with its creation event, the current state of the *ActionItem* can be determined.

## ActionItem Management Verbs

* **Assign** - The act of identifying an *executor* responsible for performing the *ActionItem* and/or the *location* at which the work is to be performed.
* **Create** - The act of defining a new *ActionItem* that needs to be performed.

## Verbs Performed on Nouns

Executors may be assigned an *ActionItem* by:

* An *Application*, perhaps based on an optimization algorithm
* A Supervisor of type "Person"
* Themselves by selecting the ActionItem to be performed from a list

ActionItems are:

* Created by an *Actor* such as a *Person* or *Application*

ActionItems may be:

* Assigned to an *Actor* such as *Group* or *Person*
* Performed against a *Target* entity
* Performed utilizing additional *Resource* entities
* Performed at a *Work Location*
