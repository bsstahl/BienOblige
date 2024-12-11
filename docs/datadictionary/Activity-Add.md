# Add Activity Data Dictionary

## Activity View

*Add Activities* are used to add entities to one or more **ActionItems** in the **Bien Oblige** system. Currently, they are only used to add **Place** entities representing the location the work is to be performed. In the future however, they may be used to add other types of entities as well.

### References

* If you are looking for information on how to create an **ActionItem**, please see [Create Activity](Activity-Create.md).
* For a complete view of the usage of the *Activity Streams 2.0* (AS2) *Activity* object within **Bien Oblige**, see the [Activity Data Dictionary](Activity.md).

### Implementation

There are 2 ways to add a location to **ActionItems** using an *Add Activity*:

1. Add the location to a single **ActionItem** by specifying a *target* with the *@type* value set to `[ "bienoblige:ActionItem", "Object" ]` and the *id* set to the URI of the **ActionItem**. The *object* field should contain the **Place** entity representing the location. If there is no **ActionItem** with the specified Id, the system will create a new **ActionItem** with the specified Id and an *exception* node indicating that an attempt was made to perform a location assignment on a non-existent **ActionItem**.

2. Add the location to multiple **ActionItems** by specifying a *target* with the *@type* value set to `Tag` and the *id* set to the Id of the **Tag**. The *object* field should contain the **Place** entity representing the location. The system will update all **ActionItems** containing a *tag* with that id, to the specified location. If there are no **ActionItems** with the specified **Tag**, the system will make no updates and provide no additional feedback.

TODO: What should happen in either case if an **ActionItem** already has a location? Update the location to the new value, add the value to the collection, or add an exception? Probably add to the collection.

The fields used for the *Add Activity* to add a location to an **ActionItem** are as follows:

* Standard AS2 *Activity* fields
  * *actor* - The AS2 *Actor* object (generally a specific Person, Application or Service) that has requested the update.
  * *context* - Specifies relevant namespaces to ensure clarity and consistency, enabling extensibility by allowing custom and additional standard namespaces for internal objects.
  * *id* - The unique identifier of the **Activity** in the form of a URI. This is generally specified as a [Uniform Resource Name (URN)](https://en.wikipedia.org/wiki/Uniform_Resource_Name) since it is only being used as a tracker rather than a locator.
  * *object* - The AS2 *Place* object that is being identified as the location of the work.
  * *published* (optional) - The date and time of the creation of the **ActionItem**. Defaults to the current date and time in UTC.
  * *target* - The **ActionItem** or **Tag** identifying the **ActionItem(s)** to which the location is to be added. The required fields of this *target* include the *@type* and *id* fields.
  * *@type* - A string value containing the standard AS2 Activity type name "Add".

* Custom *Activity* fields
  * *bienoblige:correlationId* - A unique identifier for the **Activity** that can be used to correlate it with other activities or objects in the system.
