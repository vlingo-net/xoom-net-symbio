// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    /// The reader for a given <see cref="IStateStore"/>, which is provided by its w <code>EntryReader()</code> method.
    /// The <see cref="IEntry{T}"/> instances are appended by the <see cref="IStateStore"/> <code>Write(...)</code> methods.
    /// This reads sequentially over all <see cref="IEntry{T}"/> instances in the entire storage, from the
    /// first written <see cref="IEntry{T}"/> to the current last written <see cref="IEntry{T}"/>, and is prepared to read
    /// all newly appended <see cref="IEntry{T}"/> instances beyond that point when they become available.
    /// </summary>
    /// <typeparam name="TEntry">the concrete type of <see cref="IEntry{T}"/> stored and read, which maybe be string, byte[], or object</typeparam>
    public interface IStateStoreEntryReader<TEntry> : IEntryReader<TEntry> where TEntry : IEntry
    {
    }
}