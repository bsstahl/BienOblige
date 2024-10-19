# MomentumMap: Ubiquitious Language

## Task Management Nouns

The entities involved in task management are:

* **Executor** - a person that is responsible for performing tasks.
  * Represented by Activity Streams 2.0 (AS2) objects of type "Person"
* **Supervisor** - A person with the ability to manually assign tasks to executors.
  * Represented by Activity Streams 2.0 (AS2) objects of type "Person".
* **Location** - a place where tasks are performed
  * Represented by Activity Streams 2.0 (AS2) objects of type "Place".
* **Task** - a unit of work that will be performed by an executor.
  * The event object describes the process of performing the task
  * Represented by an extension to the Activity Streams 2.0 (AS2) "Object" type called "Task"
* **Task Type** - a category of tasks that share common characteristics
  * These categories are defined by the consuming applications and are identified by URI
  * The URI may be used as a filter/sort criteria when listing tasks
* **Status** - the current state of completion of a task.
  * Options include "Not Done", "Done", and "Cancelled"

* **Update** - A change made to any portion of a task, including its creation, assignment, or change of status.
  * Represented by Activity Streams 2.0 (AS2) objects of type "Activity"
  * Each *Update* object describes a change made to the task. By applying all updates to a *Task* entity, starting with its creation event, the current state of the *Task* can be determined.

## Task Management Verbs

* **Assign** - The act of identifying an *executor* responsible for performing the task and/or the *location* at which the work is to be performed.
* **Create** - The act of defining a new task that needs to be performed.

## Verbs Performed on Nouns

Executors may be assigned tasks by:

* An *Application*, perhaps based on an optimization algorithm
* A Supervisor of type "Person"
* Themselves by selecting the task to be performed from a list

Tasks are:

* Created by an *Actor* such as a *Person* or *Application*

Tasks may be:

* Assigned to an *Actor* such as *Group* or *Person*
* Performed against a *Target* entity
* Performed utilizing additional *Resource* entities
* Performed at a *Work Location*
