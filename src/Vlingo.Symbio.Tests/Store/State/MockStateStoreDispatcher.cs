// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Concurrent;
using System.Collections.Generic;
using Vlingo.Actors.TestKit;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Tests.Store.State
{
    public class MockStateStoreDispatcher<TEntry, TState> : IDispatcher<Dispatchable<TEntry, TState>> where TEntry : IEntry where TState : IState
    {
        private AccessSafely _access = AccessSafely.AfterCompleting(0);
        
        private readonly IConfirmDispatchedResultInterest _confirmDispatchedResultInterest;
        private IDispatcherControl _control;
        private readonly Dictionary<string, TState> _dispatched = new Dictionary<string, TState>();
        private readonly ConcurrentQueue<TEntry> _dispatchedEntries = new ConcurrentQueue<TEntry>();
        private readonly AtomicBoolean _processDispatch = new AtomicBoolean(true);
        private int _dispatchAttemptCount;

        public MockStateStoreDispatcher(IConfirmDispatchedResultInterest confirmDispatchedResultInterest)
        {
            _confirmDispatchedResultInterest = confirmDispatchedResultInterest;
        }

        public void ControlWith(IDispatcherControl control) => _control = control;

        public void Dispatch(Dispatchable<TEntry, TState> dispatchable)
        {
            _dispatchAttemptCount++;
            if (_processDispatch.Get())
            {
                var dispatchId = dispatchable.Id;
                _access.WriteUsing("dispatched", dispatchId, new DispatchInternal(dispatchable.TypedState<TState>(), dispatchable.Entries));
                _control.ConfirmDispatched(dispatchId, _confirmDispatchedResultInterest);
            }
        }

        public AccessSafely AfterCompleting(int times)
        {
            _access = AccessSafely.AfterCompleting(times)
                .WritingWith<string, DispatchInternal>("dispatched", (id, dispatch) =>
                {
                    _dispatched[id] = dispatch.State;
                    foreach (var entry in dispatch.Entries)
                    {
                        _dispatchedEntries.Enqueue(entry);
                    }
                })

                .ReadingWith<string, TState>("dispatchedState", id => _dispatched[id])
                .ReadingWith("dispatchedStateCount", () => _dispatched.Count)

                .ReadingWith("dispatchedEntries", () =>  _dispatchedEntries)
                .ReadingWith("dispatchedEntriesCount", () => _dispatchedEntries.Count)
                
                .WritingWith<bool>("processDispatch", flag => _processDispatch.Set(flag))
                .ReadingWith("processDispatch", () => _processDispatch.Get())

                .ReadingWith("dispatchAttemptCount", () => _dispatchAttemptCount)

                .ReadingWith("dispatched", () => _dispatched);

            return _access;
        }

        public State<TState> Dispatched(string id) => _access.ReadFrom<string, State<TState>>("dispatchedState", id);
        
        public int DispatchedCount() => _access.ReadFrom<Dictionary<string, object>>("dispatched").Count;
        
        public void DispatchUnconfirmed() => _control.DispatchUnconfirmed();

        public class DispatchInternal
        {
            public IEnumerable<TEntry> Entries { get; }
            public TState State { get; }

            public DispatchInternal(TState state, IEnumerable<TEntry> entries)
            {
                State = state;
                Entries = entries;
            }
        }
    }
}