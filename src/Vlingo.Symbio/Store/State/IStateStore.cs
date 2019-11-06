// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common;

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    /// The basic State Store interface, defining standard dispatching and control types.
    /// </summary>
    /// <typeparam name="TEntry">The specific type of <see cref="IEntry{TEntry}"/> that will be read</typeparam>
    /// <typeparam name="TState">The specific type of <see cref="State{T}"/> that will be used by the reader and writer</typeparam>
    public interface IStateStore<TState, TEntry> : IStateStoreReader, IStateStoreWriter<TState>
    {
        /// <summary>
        /// Answer the <see cref="IStateStoreEntryReader{TEntry}"/> identified by the <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The string name of the reader</param>
        /// <returns><see cref="ICompletes{T}"/></returns>
        ICompletes<IStateStoreEntryReader<IEntry<TEntry>>> EntryReader(string name);
    }
}