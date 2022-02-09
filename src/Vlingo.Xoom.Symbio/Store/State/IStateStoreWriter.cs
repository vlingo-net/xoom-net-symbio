// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Vlingo.Xoom.Symbio.Store.State
{
    /// <summary>
    /// Defines the writer of the <see cref="IStateStore"/>.
    /// </summary>
    public interface IStateStoreWriter
    {
        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <typeparam name="TState">The concrete type of the state</typeparam>
        void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> along with appending <paramref name="sources"/>
        /// and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="sources">The <code>IEnumerable{Source{TSource}}</code> to append</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <typeparam name="TState">The concrete type of the state</typeparam>
        /// <typeparam name="TSource">The concrete type of the sources</typeparam>
        void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, IWriteResultInterest interest);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="metadata">The <see cref="Metadata"/> for the state</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> along with appending <paramref name="sources"/>
        /// and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="sources">The <code>IEnumerable{Source{TSource}}</code> to append</param>
        /// <param name="metadata">The <see cref="Metadata"/> for the state</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <typeparam name="TState">The concrete type of the state</typeparam>
        /// <typeparam name="TSource">The concrete type of the sources</typeparam>
        void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata, IWriteResultInterest interest);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object that will be sent to the <see cref="IWriteResultInterest"/> when the write has succeeded or failed</param>
        void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest, object @object);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> along with appending <paramref name="sources"/>
        /// and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="sources">The <code>IEnumerable{Source{TSource}}</code> to append</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object that will be sent to the <see cref="IWriteResultInterest"/> when the write has succeeded or failed</param>
        /// <typeparam name="TState">The concrete type of the state</typeparam>
        /// <typeparam name="TSource">The concrete type of the sources</typeparam>
        void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, IWriteResultInterest interest, object @object);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="metadata">The <see cref="Metadata"/> for the state</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object that will be sent to the <see cref="IWriteResultInterest"/> when the write has succeeded or failed</param>
        void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest, object? @object);

        /// <summary>
        /// Write the <paramref name="state"/> identified by <paramref name="id"/> along with appending <paramref name="sources"/>
        /// and dispatch the result to the <paramref name="interest"/>.
        /// </summary>
        /// <param name="id">The string unique identity of the state to read</param>
        /// <param name="state">The <typeparamref name="TState"/> typed state instance</param>
        /// <param name="stateVersion">The int version of the state</param>
        /// <param name="sources">The <code>IEnumerable{Source{TSource}}</code> to append</param>
        /// <param name="metadata">The <see cref="Metadata"/> for the state</param>
        /// <param name="interest">The <see cref="IWriteResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object that will be sent to the <see cref="IWriteResultInterest"/> when the write has succeeded or failed</param>
        /// <typeparam name="TState">The concrete type of the state</typeparam>
        /// <typeparam name="TSource">The concrete type of the sources</typeparam>
        void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata, IWriteResultInterest interest, object? @object);
    }
}