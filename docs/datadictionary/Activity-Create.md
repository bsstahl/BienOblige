# Create Activity Data Dictionary

## Activity View

Create Activities are used to, as the name implies, create entities in the **Bien Oblige** system. Currently, they are only used to create **ActionItem** entities. In the future however, they may be used to create other types of entities as well.

### References

For a complete view of the usage of the *Activity Streams 2.0* (AS2) *Activity* object within **Bien Oblige**, see the [Activity Data Dictionary](Activity.md).

### Implementation

The fields used for the *Create Activity* to add a new **ActionItem** to the system are as follows:

* Standard AS2 *Activity* fields
  * *actor* - The AS2 *Actor* object (generally a specific Person, Application or Service) that has requested the creation of the entity.
  * *context* (optional) - Specifies relevant namespaces to ensure clarity and consistency, enabling extensibility by allowing custom and additional standard namespaces for internal objects.
  * *id* - The unique identifier of the *Activity* in the form of a URI. This is generally specified as a [Uniform Resource Name (URN)](https://en.wikipedia.org/wiki/Uniform_Resource_Name) since it is only being used as a tracker rather than a locator.
  * *object* - The AS2 *Object* object that is being created. Generally this is where the **ActionItem** will be specified.
  * *published* (optional) - The date and time of the creation of the **ActionItem**. Defaults to the *published* date and time on the *Activity* object on creation.
  * *summary* (optional) - A complete summary of the **Activity** including all relevant details. This may be LLM generated or may be provided by the client.
  * *tag* (optional) - a collection of objects representing characteristics of the **Activity**, used for searching and sorting. These objects will usually be custom types, although standard AS2 objects such as *Person* may be used.
  * *@type* - A string value containing the standard AS2 Activity type name "Create".

* Custom *Activity* fields
  * *bienoblige:correlationId* - A unique identifier for the **Activity** that can be used to correlate it with other activities or objects in the system.
