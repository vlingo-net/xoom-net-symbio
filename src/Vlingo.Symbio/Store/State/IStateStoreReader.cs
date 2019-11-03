// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    /// Defines the reader of the <see cref="IStateStore{TState, TEntry}"/>.
    /// </summary>
    /// <typeparam name="TState">The type of the state to read.</typeparam>
    public interface IStateStoreReader<TState>
    {
        /// <summary>
        /// Read the state identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="interest">the <see cref="IReadResultInterest"/> to which the result is dispatched</param>
        void Read(string id, IReadResultInterest interest);

        /// <summary>
        /// Read the state identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">the string unique identity of the state to read</param>
        /// <param name="interest">The <see cref="IReadResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">an object that will be sent to the <see cref="IReadResultInterest"/> when the read has succeeded or failed</param>
        void Read(string id, IReadResultInterest interest, object? @object);
    }
}