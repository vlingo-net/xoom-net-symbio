// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Symbio.Store.Dispatch;
using Vlingo.Xoom.Actors.TestKit;

namespace Vlingo.Xoom.Symbio.Tests.Store.Dispatch;

public class MockDispatcher : IDispatcher
{
    private AccessSafely _access;

    private readonly IConfirmDispatchedResultInterest _confirmDispatchedResultInterest;
    private IDispatcherControl _control;
    private readonly List<Dispatchable> _dispatched = new List<Dispatchable>();
    private readonly AtomicBoolean _processDispatch = new AtomicBoolean(true);
    private int _dispatchAttemptCount;

    public MockDispatcher(IConfirmDispatchedResultInterest confirmDispatchedResultInterest)
    {
        _confirmDispatchedResultInterest = confirmDispatchedResultInterest;
        _access = AfterCompleting(0);
    }

    public void ControlWith(IDispatcherControl control) => _control = control;

    public void Dispatch(Dispatchable dispatchable)
    {
        _dispatchAttemptCount++;
        if (_processDispatch.Get())
        {
            var dispatchId = dispatchable.Id;
            _access.WriteUsing("dispatched", dispatchable);
            _control.ConfirmDispatched(dispatchId, _confirmDispatchedResultInterest);
        }
    }
        
    public AccessSafely AfterCompleting(int times)
    {
        _access = AccessSafely.AfterCompleting(times)
            .WritingWith<Dispatchable>("dispatched", action => _dispatched.Add(action))
            .ReadingWith("dispatched", () => _dispatched)

            .WritingWith<bool>("processDispatch", s => _processDispatch.Set(s))
            .ReadingWith("processDispatch", () => _processDispatch.Get())
            .ReadingWith("dispatchAttemptCount", () => _dispatchAttemptCount);

        return _access;
    }
        
    public int DispatchedCount()
    {
        var dispatched = _access.ReadFrom<List<Dispatchable>>("dispatched");
        return dispatched.Count;
    }

    public List<Dispatchable> GetDispatched() => _access.ReadFrom<List<Dispatchable>>("dispatched");
}