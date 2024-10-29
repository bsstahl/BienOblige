# ADR: Adoption of "ActionItem" as the Entity Name for Tasks

## Decision Group

TD - Technology Decision
Proposed | 2024-10-20

## Problem/Issue

In the development of *BienOblige*, a naming conflict arises due to the term "task" being a reserved word in C# (and many other programming languages). Using "task" as an entity name could lead to confusion, potential errors, and reduced code readability.

## Decision

After careful consideration, the term "ActionItem" has been chosen to represent entities previously referred to as "tasks" within the *BienOblige* project. This decision aligns with the project's goal to maintain clear, effective communication in the codebase while avoiding reserved keywords that could complicate development.

## Considered Alternatives

- Task
- Chore
- Assignment
- Job
- Objective
- Duty
- Activity
- Milestone
- Step
- Undertaking

## Arguments

### Pro

- **Clarity**: "ActionItem" clearly conveys the nature of the entity as something that needs action, making it intuitive for developers and users alike.
- **Avoidance of Reserved Keywords**: By not using "task", we circumvent potential issues related to reserved words in programming languages.
- **Consistency with Business Terminology**: The term is in alignment with common business jargon, where "action items" are tasks or activities that need to be completed.

### Con

- **Verbosity**: "ActionItem" is longer than "task", which could lead to slightly more verbose code. However, this is considered a minor trade-off for the clarity and avoidance of reserved keywords.

## Related decisions

N/A

## Extends

N/A

## Related requirements

N/A
