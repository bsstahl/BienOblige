# Representing the Level of Effort for ActionItems

## Decision Group

* TD - Technology Decision
* Proposed | 2024-12-05

## Context

The project requires a flexible and comprehensive method to represent the effort required for an ActionItem. The AS2 standard `duration` field offers a singular perspective focused on time, which is not sufficient for our diverse needs, including complexity, credit, and story points.

## Decision

We have decided to utilize a custom extension, represented by the `bienoblige:effort` field within the ActionItem object, to quantify the expected workload. This approach allows for adaptable representation across different systems by including a type URI defining its nature. The decision avoids using the AS2 `duration` field now and in the future to prevent confusion.

## Considered Alternatives

* Utilizing the AS2 `duration` field for effort representation.
* A hybrid approach using both the AS2 `duration` field and our custom extension.

## Arguments

### Pro

* **Flexibility and Precision**: Our custom extension allows for a detailed and nuanced representation of effort, accommodating various types of measurements like complexity, credit, story points, and time.
* **Adaptability**: Including a type URI enables this system to adapt to different contexts and systems easily.
* **Clarity and Avoidance of Confusion**: Exclusively using a custom field for effort ensures clarity and avoids confusion that might arise from mixing AS2's `duration` with custom logic.

### Con

* **Increased Implementation Complexity**: Defining and maintaining a custom extension requires more effort and complexity than utilizing existing AS2 fields.
* **Integration Challenges**: Potential integration challenges with systems or tools that expect effort to be represented using the AS2 `duration` field.

## Related Decisions

N/A

## Extends

N/A

## Related Requirements

N/A
