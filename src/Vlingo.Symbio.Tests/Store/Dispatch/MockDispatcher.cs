// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors.TestKit;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Tests.Store.Dispatch
{
    public class MockDispatcher<T, TEntry, TState> : IDispatcher<Dispatchable<TEntry, TState>> where TEntry : IEntry<T> where TState : class, IState
    {
        private AccessSafely _access;

        private readonly IConfirmDispatchedResultInterest _confirmDispatchedResultInterest;
        private IDispatcherControl _control;
        private readonly List<Dispatchable<TEntry, TState>> _dispatched = new List<Dispatchable<TEntry, TState>>();
        private readonly AtomicBoolean _processDispatch = new AtomicBoolean(true);
        private int _dispatchAttemptCount;

        public MockDispatcher(IConfirmDispatchedResultInterest confirmDispatchedResultInterest)
        {
            _confirmDispatchedResultInterest = confirmDispatchedResultInterest;
            _access = AfterCompleting(0);
        }

        public void ControlWith(IDispatcherControl control) => _control = control;

        public void Dispatch(Dispatchable<TEntry, TState> dispatchable)
        {
            _dispatchAttemptCount++;
            if (_processDispatch.Get())
            {
                var dispatchId = dispatchable.Id;
                _access.WriteUsing("dispatched", dispatchable);
                _control.ConfirmDispatched(dispatchId, _confirmDispatchedResultInterest);
            }
        }
        
        public AccessSafely AfterCompleting(int times) {
            _access = AccessSafely.AfterCompleting(times)
                .WritingWith<Dispatchable<TEntry, TState>>("dispatched", action => _dispatched.Add(action))
                .ReadingWith("dispatched", () => _dispatched)

                .WritingWith<bool>("processDispatch", s => _processDispatch.Set(s))
                .ReadingWith("processDispatch", () => _processDispatch.Get())
                .ReadingWith("dispatchAttemptCount", () => _dispatchAttemptCount);

            return _access;
        }
        
        public int DispatchedCount()
        {
            var dispatched = _access.ReadFrom<List<Dispatchable<TEntry, TState>>>("dispatched");
            return dispatched.Count;
        }

        public List<Dispatchable<TEntry, TState>> GetDispatched()
        {
            return _access.ReadFrom<List<Dispatchable<TEntry, TState>>>("dispatched");
        }
    }
}