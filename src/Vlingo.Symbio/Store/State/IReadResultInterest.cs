// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common;

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    /// Defines the result of reading the state with the specific id to the store.
    /// </summary>
    public interface IReadResultInterest
    {
        /// <summary>
        /// Implemented by the interest of a given State Store for read operation results.
        /// </summary>
        /// <param name="outcome">The <see cref="IOutcome{TFailure, TSuccess}"/> of the read</param>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> native state that was read, or the empty null if not found</param>
        /// <param name="stateVersion">The int version of the state that was read, or -1 if not found</param>
        /// <param name="metadata">The <see cref="Metadata"/> of the state that was read, or null if not found</param>
        /// <param name="object">the object passed to <code>Read()</code> that is sent back to the receiver</param>
        /// <typeparam name="TState">The native state type</typeparam>
        void ReadResultedIn<TState>(IOutcome<StorageException, Result> outcome, string? id, TState state, int stateVersion, Metadata? metadata, object? @object);
    }
}