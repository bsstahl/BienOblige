# Bien Oblige: Features

## High-Level Feature Summary

* **ActionItem Creation and Assignment**: Create *ActionItems* and assign them to executors, groups or locations.
* **Status Tracking**: Monitor the progress of *ActionItems* by status, location, executor, group, etc.
* **Location-Based Management**: Organize *ActionItems* by location to streamline operations and resource allocation.
* **Updates and Revisions**: Apply updates to *ActionItems* to track changes and maintain an accurate history of activities.

## Detailed Features - MVP Phase

* *ActionItems* can be created either individually, or in hierarchical sets related by the entity they are acting on.
  * Projects or other high-level activities can be broken down into smaller *ActionItems* that can be assigned to individuals. These *ActionItems* roll-up to the main activity and can be used to track progress over a broader scope. There is no functional limit to the depth of this hierarchy, only practical ones.
* *ActionItems* are annotated with a *status* value to indicate the current state of completion of the *ActionItem*.
  * The default *status* is *Incomplete* which indicates that the work has not yet been completed.
  * *ActionItems* can be closed by moving them to a *Complete*, *Cancelled*, or *Skipped* status.
  * *ActionItems* can be created in a *Draft* status to indicate that they are not yet ready for work to be started.
* *ActionItems* can be assigned to individuals or groups
* *ActionItems* can be assigned to be performed at a location.
  * Locations can be modified as needed to adjust to the current location of the entity being acted on
* *ActionItems* can be defined with a array of *completionMethod* identifiers that indicate how the *ActionItem* is to be considered complete.
  * The default *completionMethod* is *Manual* which requires the *ActionItem* to be marked as complete by an *Actor*.
  * Parent *ActionItems* can be closed automatically when **all** child *ActionItems* are closed.
  * Parent *ActionItems* can be closed automatically when **any** child *ActionItem* is closed.
  * Child *ActionItems* can be closed automatically when their parent *ActionItem* is closed.
* *ActionItems* can be manually closed by the **executor** when the work is complete via a client application.
  * All Parent and Child *ActionItems* are appropriately updated when a *ActionItem* is closed depending on the *completionMethods* specified.

## Detailed Features - Future Phases

* An *ActionItem* can be placed into a status of *InProgress* to indicate that work is being done on the item.
  * The delta between the date/time the item was closed and the date/time it was set to *InProgress* can be used to calculate the actual time spent on the item.
* A collection of *prerequisites* for each **ActionItems** can be specified to indicate that the work should not be started until one or more other *ActionItems* have been completed.
* An *ActionItem* can be annotated with an *attributedTo* entity to indicate the reason for its creation. This can be used to track the source of the work, such as when the *ActionItem* was created as a result of the closure, or non-closure, of another *ActionItem*. In such cases, the *attributedTo* entity would be the *ActionItem* that caused the new *ActionItem* to be created.
* A collection of *executorRequirement* objects can be specified to indicate the skills or other necessary qualifications required by the **executor** to perform a *ActionItem*.
* Tags of any object type can be added to a *ActionItem* to allow for customizable searching and filtering. This can be used to specify a *type* for the *ActionItem* or to add other metadata that is not part of the standard *ActionItem* fields but may be a valuable search criteria.
* *ActionItems* can be assigned an *endTime* to indicate when they are due.
* *ActionItems* can be closed automatically when their *endTime* has passed.
* *ActionItems* can be annotated with a collection of *Lifecycle Event Handlers* that represent custom code to be executed when the *ActionItem* is created, updated, or marked as complete. These will probably be implemented using WebHooks but this is not yet defined.
* *ActionItems* can be annotated with a collection of *Attachment* objects that represent documentation for the item. These may be of any media type and can be used to provide additional information about the work to be performed.
* *ActionItems* can be annotated with an *effort* object to indicate the expected workload for the *ActionItem*.
  * The *effort* object can include a *type* URI to define its nature, allowing for adaptable representation across different systems.
  * Standard *effort* types include *Complexity*, *Credit*, *Time*, and *StoryPoints*.
  * Custom *effort* types can be defined as needed.
* *ActionItems* can be annotated with a *priority* value to indicate the importance of the work.
* *ActionItems* can be annotated with a collection of *Exception* objects to indicate significant problems that likely need user attention.
