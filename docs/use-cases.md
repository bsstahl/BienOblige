# Bien Oblige: Use Cases

> Note: A client system is any service that interacts with the ActionItem Management System. Thus, for the purposes of this document, a User Interface client such as a Web UI or mobile app is considered a type of client system.

This document outlines the low-level capabilities of the system. For detailed studies of specific use cases, see [Detailed Use Cases](./use-cases/README.md).

## Subsystem Responsibilities

### Execution Subsystem

* **Create ActionItem** - A client system can add a new ActionItem
* **Cancel ActionItem** - A client system can indicate that a ActionItem no longer needs to be completed
  * Should include a required cancellation note
* **Assign ActionItem** - A client system can assign a ActionItem to a user
* **Complete ActionItem** - A client system can indicate that a ActionItem has been completed
* **Comment on ActionItem** - Add a comment to a ActionItem
  * Execution Notes (pre-completion)
  * Completion Notes (post-completion)

### Compliance Subsystem

* **Add Exception** - A client system or subsystem can indicate that a ActionItem has an exception
  * Should include a required exception note
  * Examples:
    * ActionItem not completed within a certain time frame of the due date
    * A request was made to modify an ActionItem in an invalid way
      * This type of exception is a result of the eventually-consistent nature of the system. See [Eventual Consistency](https://fosstodon.org/@Bsstahl/109406977184136386).
* **Resolve Exception** - A client system or subsystem can indicate that an exception has been resolved
  * Should include a required resolution note

* **Automate Exception Creation** - This subsystem should be able to automatically create exceptions based on specific criteria
  * ActionItem not completed within a certain time frame of the due date
* **Automate Reminders** - This subsystem should be able to automatically trigger reminders based on specific criteria. These reminders would be sent via the Notification Subsystem.
  * Scheduled reminder time passes

### Search Subsystem

* **List ActionItems** - A client system can retrieve a list of ActionItems, subject to specific criteria:
  * Location
  * Executor
  * Status
  * Unassigned
* **Full ActionItem Details** - A client system can request a list of all ActionItems related to a particular ActionItem.
  * The system will find the top-level item associated with that ActionItem, and then return that parent, and all of its decendants.
  * If there were ActionItems created from any of these items, those would be included as well.

### Notification Subsystem

* **Exception Raised** - Fires appropriate notifications when an exception is added to an ActionItem.
* **Reminder Triggered** - Fires appropriate notifications when a reminder is triggered.
* **Action Taken** - Fires appropriate notifications when an action is taken on an ActionItem.
  * ActionItem Completed
  * ActionItem Cancelled
  * ActionItem Exceptions Resolved

#### Notification Methods

* Primary
  * ActivityPub 2.0 Interfaces
* Secondary
  * Web-Hooks for integrations
  * eMail/Slack/etc for awareness

### Additional Features/Cases

* Other dates/times
  * Must start by
  * Hide until
* Recurrance
* Flag/Report
  * Inappropriate or illegal content
* Notes
  * Attach a collection of notes that can be added individually to an *ActionItem*.
* Follow
  * Users can subscribe to updates for a *Person*, *Group* or *ActionItem*.
* Accept/Reject
  * Users can accept or reject the assignment of an *ActionItem*.
* Custom Relationship types
  * Other than just parent/child

### Cases that will NOT be handled by this system

* RBAC for individual users is the responsibility of the client application. For example, this system will not prevent someone who is not a Supervisor from assigning an ActionItem, it will merely record that the client system requested the assignment to be made per that user.
  * This system *WILL* validate that the client system making the call has the appropriate permissions to perform the requested action.
* Workflow will not be managed by this system. If a client system needs to enforce a specific workflow, it will be responsible for doing so.
  * Example: The completion of ActionItem "A" triggers the creation of ActionItem "B"
  * The client system would need to create "B" when it is notified that "A" has been completed.
