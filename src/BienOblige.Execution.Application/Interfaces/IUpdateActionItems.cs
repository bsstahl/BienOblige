﻿using BienOblige.ActivityStream.Aggregates;
using BienOblige.ActivityStream.ValueObjects;

namespace BienOblige.Execution.Application.Interfaces;

public interface IUpdateActionItems
{
    Task<NetworkIdentity> Update(ActionItem changes, Actor actor, string correlationId);
}
