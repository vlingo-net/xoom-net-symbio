// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
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
    public class MockStateStoreDispatcher<TEntry, TState> : IDispatcher<Dispatchable<TEntry, TState>>
    {
        private AccessSafely _access = AccessSafely.AfterCompleting(0);
        
        private IConfirmDispatchedResultInterest _confirmDispatchedResultInterest;
        private IDispatcherControl _control;
        private Dictionary<string, object> _dispatched = new Dictionary<string, object>();
        private ConcurrentQueue<IEntry<TEntry>> _dispatchedEntries = new ConcurrentQueue<IEntry<TEntry>>();
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
                    _dispatched.Add(id, dispatch.State);
                    foreach (var entry in dispatch.Entries)
                    {
                        _dispatchedEntries.Enqueue(entry);
                    }
                })

                .ReadingWith<string, object>("dispatchedState", (id) => _dispatched[id])
                .ReadingWith("dispatchedStateCount", () => _dispatched.Count)

                .ReadingWith("dispatchedEntries", () =>  _dispatchedEntries)
                .ReadingWith("dispatchedEntriesCount", () => _dispatchedEntries.Count)
                
                .ReadingWith<bool, object>("processDispatch", (flag) =>
                {
                    _processDispatch.Set(flag);
                    return null; // The return could be avoided if is fixed https://github.com/vlingo-net/vlingo-net-actors/issues/68
                })
                .ReadingWith("processDispatch", () => _processDispatch.Get())

                .ReadingWith("dispatchAttemptCount", () => _dispatchAttemptCount)

                .ReadingWith("dispatched", () => _dispatched);

            return _access;
        }

        public State<TState> Dispatched(string id) => _access.ReadFrom<string, State<TState>>("dispatchedState", id);
        
        public int DispatchedCount() => _access.ReadFrom<Dictionary<string, object>>("dispatched").Count;
        
        public class DispatchInternal
        {
            public IEnumerable<IEntry<TEntry>> Entries { get; }
            public State<TState> State { get; }

            public DispatchInternal(State<TState> state, IEnumerable<IEntry<TEntry>> entries)
            {
                State = state;
                Entries = entries;
            }
        }
    }
}