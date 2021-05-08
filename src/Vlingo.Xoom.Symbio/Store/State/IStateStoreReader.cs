// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Vlingo.Xoom.Symbio.Store.State
{
    /// <summary>
    /// Defines the reader of the <see cref="IStateStore"/>.
    /// </summary>
    public interface IStateStoreReader
    {
        /// <summary>
        /// Read the state identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="interest">the <see cref="IReadResultInterest"/> to which the result is dispatched</param>
        /// <typeparam name="TState">The type of the state to read.</typeparam>
        void Read<TState>(string id, IReadResultInterest interest);

        /// <summary>
        /// Read the state identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">the string unique identity of the state to read</param>
        /// <param name="interest">The <see cref="IReadResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">an object that will be sent to the <see cref="IReadResultInterest"/> when the read has succeeded or failed</param>
        /// <typeparam name="TState">The type of the state to read.</typeparam>
        void Read<TState>(string id, IReadResultInterest interest, object? @object);

        /// <summary>
        /// Read the state identified by <code>id</code> within <paramref name="bundles"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="bundles"><see cref="IEnumerable{T}"/> defining the states to read</param>
        /// <param name="interest">The <see cref="IReadResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">an object that will be sent to the <see cref="IReadResultInterest"/> when the read has succeeded or failed</param>
        /// <typeparam name="TState">The type of the state to read.</typeparam>
        void ReadAll<TState>(IEnumerable<TypedStateBundle> bundles, IReadResultInterest interest, object? @object);
    }
}