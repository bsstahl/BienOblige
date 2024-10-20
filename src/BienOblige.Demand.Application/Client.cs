using BienOblige.Demand.Aggregates;
using BienOblige.Demand.Application.Interfaces;
using BienOblige.Demand.Exceptions;
using BienOblige.Demand.ValueObjects;

namespace BienOblige.Demand.Application;

public class Client
{
    private readonly IGetActionItems _actionItemReader;
    private readonly ICreateActionItems _actionItemCreator;

    public Client(IGetActionItems actionItemReader, ICreateActionItems actionItemCreator)
    {
        _actionItemReader = actionItemReader;
        _actionItemCreator = actionItemCreator;
    }

    //public ActionItem FindActionItem(NetworkIdentity id)
    //{
    //    throw new NotImplementedException();
    //}

    public NetworkIdentity CreateActionItem(ActionItem item, NetworkIdentity userId)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(userId);
        return _actionItemReader.Exists(item.Id) 
            ? throw new DuplicateIdentifierException(item.Id) 
            : _actionItemCreator.Create(item, userId);
    }

    public void CancelActionItem(string id, string userId)
    {
        throw new NotImplementedException();
    }

}
