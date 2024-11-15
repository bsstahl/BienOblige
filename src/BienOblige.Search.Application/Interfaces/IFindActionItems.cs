using BienOblige.Execution.Aggregates;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Search.Application.Interfaces;

internal interface IFindActionItems
{
    Task<IEnumerable<ActionItem>> GetByTarget(NetworkIdentity targetId, string targetType);
    Task<IEnumerable<ActionItem>> GetGraph(NetworkIdentity parentId);
    Task<IEnumerable<ActionItem>> GetAll();
}
