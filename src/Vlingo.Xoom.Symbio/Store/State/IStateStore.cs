// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.State;

/// <summary>
/// The basic State Store interface, defining standard dispatching and control types.
/// </summary>
public interface IStateStore : IStateStoreReader, IStateStoreWriter
{
    /// <summary>
    /// Answer the <see cref="IStateStoreEntryReader"/> identified by the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The string name of the reader</param>
    /// <typeparam name="TEntry">The specific type of <see cref="IEntry{TEntry}"/> that will be read</typeparam>
    /// <returns><see cref="ICompletes{T}"/></returns>
    ICompletes<IStateStoreEntryReader> EntryReader<TEntry>(string name) where TEntry : IEntry;
}