// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    /// A repository for over an <see cref="IStateStore{T}"/>
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry</typeparam>
    public interface IStateStoreRepository<TEntry> : IRepository where TEntry : IEntry
    {
        IStateStore<TEntry> StateStore { get; }
    }
}