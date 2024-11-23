using BienOblige.Api.Entities;

namespace BienOblige.Api.Interfaces;

/// <summary>
/// Represents an entity that can be the target of a Task.
/// </summary>
/// <remarks>
/// Properties of the Activity Streams 2.0 `Object` type that can be used include:
/// 
/// attachment | attributedTo | audience | content | context | name | endTime | generator | 
/// icon | image | inReplyTo | location | preview | published | replies | startTime | summary | 
/// tag | updated | url | to | bto | cc | bcc | mediaType | duration
/// </remarks>
public interface IActionItemTarget
{
    NetworkObject AsNetworkObject();
}
