# ActionItem Data Dictionary

## ActionItem View

The fields of an **ActionItem** are defined as follows:

* Standard AS2 *Object* fields
  * *attributedTo* (optional) - The **Executor** of the ActionItem. If the action item was not performed (ie marked complete as a result of a parent being marked complete), this field will hold a reference to the **ActionItem** that triggered the action.
  * *content* - (optional) A detailed description of the work to be performed.
  * *context* (optional) - ??? TODO: Evaluate this field usage.
  * *endTime* (optional) - The date and time by which the work needs to be completed
  * *generator* (optional) - The **Actor** (Person, Application or Service) that created the ActionItem
  * *id* - The unique identifier for the ActionItem
  * *location* (optional) - The **Location** where the work is to be performed
  * *name* - A short description of the work to be performed that can be used like a title
  * *published* (optional) - The date and time of the creation of the **ActionItem**. Defaults to the *published* date and time on the Activity object on creation.
  * *summary* (optional) - A complete summary of the **ActionItem** including all relevant details.
  * *tag* (optional) - a collection of objects representing characteristics of the work, used for searching and sorting. These objects will usually be custom types, although standard AS2 objects such as *person* may be used.
  * *target* (optional) - An object representing the entity on which the work is to be performed
  * *type* - An array holding both the custom object type ("bienoblige:ActionItem") and the standard fallback AS2 type ("Object")
  * *updated* (optional) - The date and time of the last update to the **ActionItem**. Defaults to the *published* date and time on creation.
* Custom *ActionItem* fields
  * *bienoblige:exceptions* (optional) - a collection of exceptions that have been raised on the *ActionItem*. This will be null or empty on creation and will be populated as exceptions are raised. Exceptions may be marked as "resolved" but cannot be removed.
  * *bienoblige:executorRequirements* (optional) - a collection of objects representing the skills or other requirements needed by the **Executor** to perform the work.
  * *bienoblige:parent* (optional) - the parent ActionItem to which this ActionItem is a child. Allows a hierarchy of work to be created.
  * *bienoblige:prerequisites* (optional) - a collection of objects representing other **ActionItems** that must be completed before this work can be started. If specified, this field *must* indicate one or more **ActionItem** entities within the current instance or an exception will be created (warning) and the field will be ignored. In the future, there may be the capability to define prerequisites across instances, but this capability is not yet defined.
  * *bienoblige:origin* (optional) - A reference to the original **ActionItem** that led to its creation. This is used to trace the lineage of work. Typically, this field is omitted or null, but it is populated when a new **ActionItem** is generated from an existing one, such as when a quality check fails on the original **ActionItem** and rework is required.
  * *bienoblige:priority* (optional) - a value indicating the importance of the work.
  * *bienoblige:status* (optional) - the current state within the lifecycle of the *ActionItem*. Defaults to `https://bienoblige.com/ns/status#Incomplete` on creation.
  * *bienoblige:effort* (optional) - an object specifying the type and amount of effort required to complete the work. Prior to the completion of the task, this may be a projected or standard value. After the task is completed, it may be modified to an actual value but that is not required. Whether the value represents a standard, expected, or actual value is identified within the object.
  * *bienoblige:updatedBy* (optional) - a reference to the **Actor** that last updated the **ActionItem**. This is used to track the history of changes to the work. If supplied, it should correspond to the date and time specified in the *updated* field if there is one.
* Custom Lifecycle Event Handlers
  * *bienoblige:completionMethod* (optional) - An array of URIs that represent the means by which the work is to be marked as completed. Defaults to "bienoblige:Manual" on creation. Any values not part of the `bienoblige` namespace are assumed to represent custom code.
  * *bienoblige:onCreate* (optional) - An array of URIs that represent custom code to be executed when the **ActionItem** is created. These will probably be implemented using WebHooks but this is not yet defined.
  * *bienoblige:onUpdate* (optional) - An array of URIs that represent custom code to be executed when the **ActionItem** is updated. These will probably be implemented using WebHooks but this is not yet defined.
  * *bienoblige:onComplete* (optional) - An array of URIs that represent custom code to be executed when the **ActionItem** is marked as complete. These will probably be implemented using WebHooks but this is not yet defined.

## Activity Streams 2.0 (AS2) Object View

The standard fields of an AS2 `object` are defined per the [W3C Activity Streams 2.0 Vocabulary](https://w3c.github.io/activitystreams/vocabulary/). The use of these fields within the context of an **ActionItem** is as follows:

* **attachment** - Currently unused, reserved for future document attachments to an **ActionItem**
* **attributedTo** - The **Executor** of the **ActionItem**
* audience - Unused
* **content** - A detailed description of the work to be performed.
* **context** - ???
* **name** - The Title of the **ActionItem**
* **endTime** - The date and time by which the work needs to be completed (aka "due date" or "need by date")
* **generator** - The creator of the **ActionItem**, an AS2 *Actor* object
* icon - Unused
* **id** - The unique identifier of the **ActionItem** in the form of a URI
* image - Unused
* inReplyTo - Unused
* **location** - The location where the work is to be performed
* preview - Unused
* **mediaType** - Currently unused, reserved for future use
* **published** - The date and time of the creation of the **ActionItem**
* replies - Unused
* **startTime** - Currently unused, reserved for future use
* **summary** - A summary of the task including context and content. This field is not required and may be added separately from the creation of the **ActionItem**, perhaps using a language model to generate the summary
* **tag** - A collection of searchable objects that describe and classify the **ActionItem**.
* **updated** - The date and time of the last update to the **ActionItem**. If no changes have been made, this field may be null or omitted, or it may be the same as the *published* date and time.
* url - Unused
* to - Unused
* bto - Unused
* cc - Unused
* bcc - Unused
* duration - Unused and should never be used to avoid confusion with the extensions that measure effort
