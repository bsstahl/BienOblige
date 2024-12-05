# Open Questions Relating to the Project

## Activity Streams / Activity Pub questions

* Should we be using the `startTime` field in AS2 objects?
* Link support: Many AS2 types support either Link or Object types. Since this system uses AS2 mostly for *output*, especially in MVP, I'm thinking we ignore the Link type and just use Object types. This will simplify the system and make it easier to understand. It could however make it very difficult to implement Links later, and it may violate the pricinple of compatibility with AS2.
  * In practice, this means that fields like `Target` and `attributedTo` will accept AS2 `Object` types, but not `Link` types.
* How do we handle the situation where the children of a task are all closed, but in different ways (some completed, some cancelled). Should the parent task be considered complete or cancelled or neither? I'm leaning towards "complete" because it represents a roll-up task anyway and there is no more work to do.
* When is it best to use a URI like ```bienoblige:completionMethod#AllChildrenCompleted``` vs one like ```https://bienoblige.com/ns/completionMethod#AllChildrenCompleted```? Are they interchangeable?
* What is the best practice for rework?
  * Create a new task that references the old task?
* Is there a completion method that represents the fact that the dependent task was completed so the task it is dependent on is no longer necessary?
