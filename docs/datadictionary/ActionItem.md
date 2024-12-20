# ActionItem Data Dictionary

## ActionItem View

An **ActionItem** is a task or unit of work that needs to be performed. It is created by an **Actor** and may be assigned to an **Executor** that represents an individual or group. The **ActionItem** contains all the details of the work to be performed, including the location, due date, and any prerequisites or exceptions that may apply.

### References

An **ActionItem** is a custom entity that extends the [Activity Streams 2.0](https://www.w3.org/TR/activitystreams-core/) *Object*.

### Implementation

The fields of an **ActionItem** are defined as follows:

* Standard AS2 *Object* fields
  * *attributedTo* (optional) - The entity that created the need for this work. This will usually be empty, however it can identify another *ActionItem* that led to the creation of this one. For example, if an inspection task fails, it might trigger the creation of a maintenance task. The maintenance task may have the inspection task in this field.
  * *audience* - (optional) The **Executor** of the ActionItem. This may be an individual or a group. If not supplied, the work is not yet associated with a particular user or community. This value may be assigned later using the **invite** *Activity*.
  * *content* - (optional) A detailed description of the work to be performed. The Mime content type of this field is specified in the *mediaType* field.
  * *context* (optional) - Unused at this level unless there are additional contexts needed for the **ActionItem** beyond than those specified in the [Activity](Activity.md) object. It is considered best-practice to make all contexts available at the **Activity** level and not to use this field within the **ActionItem**, however it is conceivable that there might be circumstances that arise which require the use of this field at this level.
  * *endTime* (optional) - The date and time by which the work needs to be completed
  * *generator* (optional) - The **Actor** (Person, Application or Service) that created the ActionItem
  * *id* - The unique identifier for the ActionItem
  * *location* (optional) - The **Location** where the work is to be performed
  * *mediaType* (optional) - The Mime content type of the *content* field. Defaults to `text/plain` if not specified. Currently, `text/plain` is the only type guaranteed to be supported by clients, but it is recommended that clients at least support `text/html` and `text/markdown` as well.
  * *name* - A short description of the work to be performed that can be used like a title
  * *published* (optional) - The date and time of the creation of the **ActionItem**. Defaults to the *published* date and time on the Activity object on creation.
  * *summary* (optional) - A complete summary of the **ActionItem** including all relevant details.
  * *tag* (optional) - a collection of objects representing characteristics of the work, used for searching and sorting. These objects will usually be custom types, although standard AS2 objects such as *person* may be used.
  * *type* - An array holding both the custom object type ("bienoblige:ActionItem") and the standard fallback AS2 type ("Object")
  * *updated* (optional) - The date and time of the last update to the **ActionItem**. Defaults to the *published* date and time on creation.
* Custom *ActionItem* fields
  * *bienoblige:exceptions* (optional) - a collection of exceptions that have been raised on the *ActionItem*. This will be null or empty on creation and will be populated as exceptions are raised. Exceptions may be marked as "resolved" but cannot be removed.
  * *bienoblige:executorRequirements* (optional) - a collection of objects representing the skills or other requirements needed by the **Executor** to perform the work.
  * *bienoblige:parent* (optional) - the parent ActionItem to which this ActionItem is a child. Allows a hierarchy of work to be created.
  * *bienoblige:prerequisites* (optional) - a collection of URIs representing other **ActionItems** that must be completed before this work can be started. If specified, this field *must* indicate one or more **ActionItem** entities within the current instance or an exception will be created (warning) and the field will be ignored. In the future, there may be the capability to define prerequisites across instances, but this capability is not yet defined.
  * *bienoblige:priority* (optional) - a value indicating the importance of the work.
  * *bienoblige:status* (optional) - the current state within the lifecycle of the *ActionItem*. Defaults to `https://bienoblige.com/ns/status#Incomplete` on creation.
  * *bienoblige:target* (optional) - An object representing the entity on which the work is to be performed
  * *bienoblige:effort* (optional) - an object specifying the type and amount of effort required to complete the work. Prior to the completion of the task, this may be a projected or standard value. After the task is completed, it may be modified to an actual value but that is not required. Whether the value represents a standard, expected, or actual value is identified within the object.
  * *bienoblige:updatedBy* (optional) - a reference to the **Actor** that last updated the **ActionItem**. This is used to track the history of changes to the work. If supplied, it should correspond to the date and time specified in the *updated* field if there is one.
* Custom Lifecycle Event Handlers
  * *bienoblige:completionMethod* (optional) - An array of URIs that represent the means by which the work is to be marked as completed. Defaults to "bienoblige:Manual" on creation. Any values not part of the `bienoblige` namespace are assumed to represent custom code.
  * *bienoblige:onCreate* (optional) - An array of URIs that represent custom code to be executed when the **ActionItem** is created. These will probably be implemented using WebHooks but this is not yet defined.
  * *bienoblige:onUpdate* (optional) - An array of URIs that represent custom code to be executed when the **ActionItem** is updated. These will probably be implemented using WebHooks but this is not yet defined.
  * *bienoblige:onComplete* (optional) - An array of URIs that represent custom code to be executed when the **ActionItem** is marked as complete. These will probably be implemented using WebHooks but this is not yet defined.

## Activity Streams 2.0 (AS2) Object View

The standard fields of an AS2 *object* are defined per the [W3C Activity Streams 2.0 Vocabulary](https://w3c.github.io/activitystreams/vocabulary/). The use of these fields within the context of an **ActionItem** is as follows:

* **attachment** - Currently unused, reserved for future document attachments to an **ActionItem**
* **attributedTo** - The object that caused the **ActionItem** to be created.
* **audience** - The **Executor** of the **ActionItem**
* bcc - Unused
* bto - Unused
* cc - Currently unused, reserved for future use
* **content** - A detailed description of the work to be performed.
* **context** - The schema definitions in which objects in the messages are defined.
* duration - Unused and should never be used to avoid confusion with the extensions that measure effort
* **name** - The Title of the **ActionItem**
* **endTime** - The date and time by which the work needs to be completed (aka "due date" or "need by date")
* **generator** - The creator of the **ActionItem**, an AS2 *Actor* object
* icon - Currently unused, reserved for future use
* **id** - The unique identifier of the **ActionItem** in the form of a URI
* image - Unused
* inReplyTo - Unused
* **location** - The location where the work is to be performed
* preview - Currently unused, reserved for future use
* **mediaType** - Currently unused, reserved for future use
* **published** - The date and time of the creation of the **ActionItem**
* replies - Currently unused, reserved for future use
* **startTime** - Currently unused, reserved for future use
* **summary** - A summary of the task including context and content. This field is not required and may be added separately from the creation of the **ActionItem**, perhaps using a language model to generate the summary
* **tag** - A collection of searchable objects that describe and classify the **ActionItem**.
* to - Currently unused, reserved for future use
* **updated** - The date and time of the last update to the **ActionItem**. If no changes have been made, this field may be null or omitted, or it may be the same as the *published* date and time.
* url - Unused
