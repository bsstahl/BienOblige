# Activity Data Dictionary

## Activity View

The fields of an *Activity* are the same as those of an *Activity Streams 2.0* (AS2) *Object* with a few standard extensions defined in the [W3C Activity Streams 2.0 Vocabulary](https://w3c.github.io/activitystreams/vocabulary/). However, the usage of these fields varies depending on the *type* of the **Activity**. The use of these fields within the various **Activity** types utilized by **Bien Oblige** can be found in the table below. For the specifics of each **Activity** type, see the appropriate document as referenced in the table.

### Standard AS2 Activity Fields

| Field Name | Activity Type Usage |
| ------ | ------ |
| **actor** (required) | [Create](Activity-Create.md) |
| attachment | Unused |
| attributedTo | Unused |
| audience | Unused |
| bcc | Unused |
| bto | Unused |
| cc | Unused |
| content | Unused |
| **context** (required) | [Create](Activity-Create.md) |
| duration | Unused |
| endTime | Unused |
| generator | Unused |
| icon | Unused |
| **id** (required) | [Create](Activity-Create.md) |
| image | Unused |
| inReplyTo | Unused |
| instrument | Unused |
| location | Unused |
| mediaType | Unused |
| name | Unused |
| **object** (required) | [Create](Activity-Create.md) |
| origin | Unused |
| preview | Unused |
| **published** | [Create](Activity-Create.md) |
| result | Unused |
| startTime | Unused |
| summary | Unused |
| tag | Unused |
| target | Unused |
| to | Unused |
| **type** (required) | [Create](Activity-Create.md) |
| updated | Unused |
| url | Unused |

### Custom BienOblige Activity Fields

| Field Name | Activity Type Usage |
| ------ | ------ |
| **bienoblige:correlationId** (required) | [Create](Activity-Create.md) |
