# ActionItem Data Dictionary

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

