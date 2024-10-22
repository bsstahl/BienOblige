using BienOblige.ValueObjects;
using BienOblige.Execution.Aggregates;
using BienOblige.Execution.Application.Interfaces;

namespace BienOblige.Execution.Data.Kafka
{
    public class ActionItemRepository : ICreateActionItems, IGetActionItems
    {
        public Task<NetworkIdentity> Create(ActionItem item, NetworkIdentity userId, string correlationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(NetworkIdentity id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionItem?> Get(NetworkIdentity id)
        {
            throw new NotImplementedException();
        }
    }
}
