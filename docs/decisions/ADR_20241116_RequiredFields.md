# Making the "Name" Field Required for ActionItems

## Decision Group

* TD - Technology Decision
* Proposed 2024-11-16

## Problem/Issue

There's a need to determine which fields describing an ActionItem should be required versus optional. This decision impacts how users interact with and understand ActionItems within a system that complies with ActivityPub standards.

## Decision

Based on guidance from Evan Prodromou's book on ActivityPub, and in compliance with Activity Streams 2.0 (AS2) requirements, the **"name"** field is made required for ActionItems. This decision aligns with AS2's requirement for each object to have either a "name" or "summary" property. The "name" field serves as a clear identifier or title for the ActionItem, essential for user understanding and system clarity.

## Considered Alternatives

* Making the "content" field required
* Making the "summary" field required
* No field required

## Arguments

### Pro

* **Compliance with AS2:** Ensuring the "name" field is required aligns with Activity Streams 2.0 requirements, which mandate the presence of a "name" or "summary" property.
* **Clarity and Accessibility:** The "name" field provides a straightforward identifier or title, which is crucial for user interaction and understanding.
* **Flexibility for Summaries:** Keeping the "summary" field optional allows for the incorporation of AI-generated summaries without making it a necessity.

### Con

* **Potential Limitation for Content Description:** By not making the "content" field required, there might be cases where ActionItems lack detailed descriptions or narratives.
* **Less Flexibility in Required Fields:** Making a field required could limit how users might want to structure their ActionItems, depending on their specific use case.

## Related Decisions

N/A

## Extends

N/A

## Related Requirements

N/A
