# Activity Data Dictionary

## Activity Types

The [W3C Activity Streams 2.0 Vocabulary](https://w3c.github.io/activitystreams/vocabulary/) defines a number of standard **Activity** types. **Bien Oblige** uses a subset of these types to interact with the system. The table below lists the **Activity** types used by **Bien Oblige** and provides a brief description of each.

| Activity Type | Description |
| ------ | ------ |
| Accept | Reserved to indicate acceptance or acknowledgment of an **ActionItem** assignment |
| [Add](Activity-Add.md) | Reserved for adding a *location*, and potentially other elements, to an **ActionItem** |
| [Announce](Activity-Announce.md) | Publishes a notification to listeners containing the state of the **ActionItem** |
| Arrive | Unused |
| Block | Unused |
| [Create](Activity-Create.md) | Used to create new **ActionItems** and potentially other entities in the future |
| Delete | Reserved for future use on entities other than **ActionItems** |
| Dislike | Unused |
| Flag   | Reserved to indicate a problem with the **ActionItem** that needs to be addressed -- Results in an **Exception** being added to the **ActionItem** |
| Follow | Reserved to indicate that the account should be notified of updates to the **ActionItem** |
| Ignore | Unused |
| Invite | Reserved to assign a user or group as the **Executor** of an **ActionItem** |
| Join   | Unused |
| Leave  | Unused |
| Like   | Unused |
| Listen | Unused |
| Move   | Unused |
| Offer  | Unused |
| Question | Reserved for adding a comment or question to the **ActionItem** |
| Reject | Reserved to indicate that the user or group does not accept the assignment to an **ActionItem** |
| Read   | Unused |
| Remove | Reserved for removing a *location*, and potentially other elements in the future, from an **ActionItem** |
| TentativeReject | Unused |
| TentativeAccept | Unused |
| Travel | Unused |
| Undo   | Reserved for future use |
| [Update](Activity-Update.md) | Used to replace the contents of an **ActionItem** node(s) with the specified contents, removing the previous contents  |
| View   | Reserved for possible future use as a "soft-lock" to suggest the possibility that the task is already being worked on |

### Custom Bien Oblige Activity Types

| Activity Type | Description |
| ------ | ------ |
| [Release](Activity-Release.md) | Used to release a set of **ActionItems** for processing -- performs the equivalent of a batch *Update* from *Draft* to *Incomplete* status on a task hierarchy and an *Announce* on the parent task of the hierarchy |

## Activity View

The fields of an *Activity* are the same as those of an *Activity Streams 2.0* (AS2) *Object* with a few standard extensions defined in the [W3C Activity Streams 2.0 Vocabulary](https://w3c.github.io/activitystreams/vocabulary/). However, the usage of these fields varies depending on the *type* of the **Activity**. The use of these fields within the various **Activity** types utilized by **Bien Oblige** can be found in the table below. For the specifics of each **Activity** type, see the appropriate document as referenced in the table.

### Standard AS2 Activity Fields

| Field Name | Activity Type Usage |
| ------ | ------ |
| **actor** (required) | [Add](Activity-Add.md) [Create](Activity-Create.md) |
| attachment | Unused |
| attributedTo | Unused |
| audience | Unused |
| bcc | Unused |
| bto | Unused |
| cc | Unused |
| content | Unused |
| **context** (required) | [Add](Activity-Add.md) [Create](Activity-Create.md) |
| duration | Unused |
| endTime | Unused |
| generator | Unused |
| icon | Unused |
| **id** (required) | [Add](Activity-Add.md) [Create](Activity-Create.md) |
| image | Unused |
| inReplyTo | Unused |
| instrument | Unused |
| location | Unused |
| mediaType | Unused |
| name | Unused |
| **object** (required) | [Add](Activity-Add.md) [Create](Activity-Create.md) |
| origin | Unused |
| preview | Unused |
| **published** | [Add](Activity-Add.md) [Create](Activity-Create.md) |
| result | Unused |
| startTime | Unused |
| summary | Unused |
| tag | Unused |
| **target** | [Add](Activity-Add.md) |
| to | Unused |
| **type** (required) | [Add](Activity-Add.md) [Create](Activity-Create.md) |
| updated | Unused |
| url | Unused |

### Custom BienOblige Activity Fields

| Field Name | Activity Type Usage |
| ------ | ------ |
| **bienoblige:correlationId** (required) | [Add](Activity-Add.md) [Create](Activity-Create.md) |
