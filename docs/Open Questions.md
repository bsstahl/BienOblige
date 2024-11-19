# Open Questions Relating to the Project

## Activity Streams / Activity Pub questions

* How should we be leveraging the `Context` field in AS2 objects?
  * For the `Activity`, it should indicate the relevant namespaces defining the objects
  * For the `ActionItem`, it should indicate what?
    * That it is a `bienoblige:ActionItem`?
    * That it is specific to **this instance** of the system?
* Should we be using the `startTime` field in AS2 objects?
* Should we be using the `duration` field in AS2 objects or should we stick to the more generic extension where we can specify different types of credit?
  * Should we use both? Wouldn't that be confusing?
* Link support: Many AS2 types support either Link or Object types. Since this system uses AS2 mostly for *output*, especially now, I'm thinking we ignore the Link type and just use Object types. This will simplify the system and make it easier to understand. It could however make it very difficult to implement Links later, and it violates the pricinple of compatibility with AS2.
  * In practice, this means that fields like `Target` and `attributedTo` will accept AS2 `Object` types, but not `Link` types.
* How do w handle the situation where the children of a task are all closed, but in different ways (some completed, some cancelled). Should the parent task be considered complete or cancelled or neither? I'm leaning towards "complete" because it represents a roll-up task anyway and there is no more work to do.
* When is it best to use a URI like ```bienoblige:completionMethod#AllChildrenCompleted``` vs one like ```https://bienoblige.com/ns/completionMethod#AllChildrenCompleted```? Are they interchangeable?
