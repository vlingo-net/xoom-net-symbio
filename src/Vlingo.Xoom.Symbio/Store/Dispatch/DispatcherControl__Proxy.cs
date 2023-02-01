// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Actors;

namespace Vlingo.Xoom.Symbio.Store.Dispatch;

public class DispatcherControl__Proxy : IDispatcherControl
{
    private const string ConfirmDispatchedRepresentation1 =
        "ConfirmDispatched(string, Vlingo.Xoom.Symbio.Store.Dispatch.IConfirmDispatchedResultInterest)";

    private const string DispatchUnconfirmedRepresentation2 = "DispatchUnconfirmed()";
    private const string StopRepresentation3 = "Stop()";

    private readonly Actor _actor;
    private readonly IMailbox _mailbox;

    public DispatcherControl__Proxy(Actor actor, IMailbox mailbox)
    {
        _actor = actor;
        _mailbox = mailbox;
    }

    public void ConfirmDispatched(string dispatchId,
        IConfirmDispatchedResultInterest interest)
    {
        if (!_actor.IsStopped)
        {
            Action<IDispatcherControl> cons128873 = __ =>
                __.ConfirmDispatched(dispatchId, interest);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, ConfirmDispatchedRepresentation1);
            }
            else
            {
                _mailbox.Send(new LocalMessage<IDispatcherControl>(_actor,
                    cons128873, ConfirmDispatchedRepresentation1));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ConfirmDispatchedRepresentation1));
        }
    }

    public void DispatchUnconfirmed()
    {
        if (!_actor.IsStopped)
        {
            Action<IDispatcherControl> cons128873 = __ => __.DispatchUnconfirmed();
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, DispatchUnconfirmedRepresentation2);
            }
            else
            {
                _mailbox.Send(new LocalMessage<IDispatcherControl>(_actor,
                    cons128873, DispatchUnconfirmedRepresentation2));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, DispatchUnconfirmedRepresentation2));
        }
    }

    public void Stop()
    {
        if (!_actor.IsStopped)
        {
            Action<IDispatcherControl> cons128873 = __ => __.Stop();
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, StopRepresentation3);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IDispatcherControl>(_actor, cons128873,
                        StopRepresentation3));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StopRepresentation3));
        }
    }
}