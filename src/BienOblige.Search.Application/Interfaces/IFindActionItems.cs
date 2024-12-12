using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Search.Application.Interfaces;

public interface IFindActionItems
{
    Task<IEnumerable<NetworkObject>> GetByTarget(NetworkIdentity targetId, string targetType);
    Task<IEnumerable<NetworkObject>> GetGraph(NetworkIdentity parentId);
    Task<IEnumerable<NetworkObject>> GetAll();
}
