using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Data.Kafka
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
