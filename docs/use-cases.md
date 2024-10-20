# Bien Oblige: Use Cases

* **Create Task** - A client system can add a new task
* **Assign Task** - A client system can assign a task to a user
* **Complete Task** - A client system can indicate that a task has been completed
* **Cancel Task** - A client system can indicate that a task will not be completed
* **Comment on Task** - Add a comment to a task
  * Execution Notes (pre-completion)
  * Completion Notes (post-completion)
  * Cancellation Notes
* **List Tasks** - A client system can retrieve a list of tasks, subject to specific criteria:
  * Location
  * Executor
  * Status
  * Unassigned

## Additional Features/Cases

* Alarms/Alerts - How would these be implemented? 
  * Just fire an event?
  * Webhook?
* Notifications - Is publication sufficient here?
* Exceptions - asynchronous (eventually-consistent) exception management
  * i.e. if a task entity somehow ends-up in an invalid state
* Other dates/times
  * Must start by
  * Hide until
* Tags
  * Search/filter criteria
* Recurrance
* Task Hierarchies
  * Parent/Child relationships
* Flag/Report
  * Inappropriate or illegal content
  
## Extensibility Options

* Additional task statuses?
* Skills required (labor type)
  * Search/filter criteria
* Relationship types
  * Other than parent/child
* WebHooks
  * Task Created
  * Task Updates
  * Alarms/Alerts

## Notes

* RBAC is the responsibility of the client application. For example, this system will not prevent someone who is not a Supervisor from assigning a task, it will merely record that the client system requested the assignment to be made per that user.
