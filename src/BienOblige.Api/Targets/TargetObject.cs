using BienOblige.Api.Entities;
using BienOblige.Api.Interfaces;

namespace BienOblige.Api.Targets;

/// <summary>
/// A generic representation of an ActionItem target. If a more specific target type is 
/// available that will serve the purpose, it should be used instead.
/// </summary>
public class TargetObject : NetworkObject, IActionItemTarget
{
    public NetworkObject AsNetworkObject()
    {
        return this;
    }
}
