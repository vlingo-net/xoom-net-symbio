// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Vlingo.Symbio.Store.Dispatch
{
    /// <summary>
    /// Defines the interface through which basic abstract storage implementations
    /// delegate to the technical implementations. See any of the existing concrete
    /// implementations for details.
    /// </summary>
    public interface IDispatcherControlDelegate<TEntry, TState> where TEntry : IEntry where TState : class, IState
    {
        IEnumerable<Dispatchable<TEntry, TState>> AllUnconfirmedDispatchableStates { get; }

        void ConfirmDispatched(string dispatchId);

        void Stop();
    }
}