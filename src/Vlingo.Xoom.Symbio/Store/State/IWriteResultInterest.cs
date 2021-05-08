// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.State
{
    public interface IWriteResultInterest
    {
        /// <summary>
        /// Implemented by the interest of a given State Store for write operation results.
        /// </summary>
        /// <param name="outcome">The <see cref="IOutcome{TFailure, TSuccess}"/> of the write</param>
        /// <param name="id">The string unique identity of the state to attempted write</param>
        /// <param name="state">the <typeparamref name="TState"/> native state that was possibly written</param>
        /// <param name="stateVersion">The int version of the state that was possibly written</param>
        /// <param name="sources">The <code>IEnumerable{Source{TSource}}</code> if any</param>
        /// <param name="object">The Object passed to write() that is sent back to the receiver</param>
        /// <typeparam name="TState">The native state type</typeparam>
        /// <typeparam name="TSource">The native source type</typeparam>
        void WriteResultedIn<TState, TSource>(IOutcome<StorageException, Result> outcome, string id, TState state, int stateVersion, IEnumerable<TSource> sources, object? @object);
    }
}