# Bien Oblige Data Dictionary

All **Bien Oblige** messages come in the form of an [Activity](Activity.md) object. The **Activity** object contains an **Object** field that represents the entity being acted upon. The **Object** is the entity that is being created, updated, or deleted, which is usually represented by a custom **Bien Oblige** entity called an [ActionItem](ActionItem.md). The **ActionItem** contains the details of the task to be performed.

There are different types of **Activity** objects that can be used to interact with the **Bien Oblige** system. The most basic type is the [Create Activity](Activity-Create.md), which is used to create new **ActionItems**. Other types of **Activity** objects are used to update or otherwise modify the state of **ActionItem** entities.

## Customizing Entities

Client systems are welcome to add custom properties to the **Bien Oblige** entities using their own extensions. These additional properties will be ingored by the system, but will be carried along with the entities as they travel throughout the process. This allows for the addition of custom metadata that may be useful to the client system. Client systems may also utilize fields within the AS2 standard that are marked as *unused* in **Bien Oblige** for their own extensions. However, fields that are marked as *reserved* should not be used in this way as those may conflict with future **Bien Oblige** functionality.

It is generally considered best-practice to extend the entities rather than simply utilizing existing properties unless the existing properties are a perfect fit for the client's needs according to the specification. Extending AS2 does however require the use of the **Context** field to define the new properties contexts, and to then include the appropriate **Bien Oblige** entity types, and AS2 fallback types, as part of the object's *type* field.

## Standard Bien Oblige Entities

* [ActionItem](ActionItem.md)
* [Activity](Activity.md)
  * [Add Activity](Activity-Add.md)
  * [Create Activity](Activity-Create.md)
  