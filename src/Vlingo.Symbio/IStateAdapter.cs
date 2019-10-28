// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio
{
    /// <summary>
    /// Adapts the native state to the raw <see cref="State{T}"/>, and the raw <see cref="State{T}"/> to the native state.
    /// </summary>
    /// <typeparam name="TSource">The native type of the state</typeparam>
    /// <typeparam name="TState">the raw <see cref="State{T}"/> of the state</typeparam>
    public interface IStateAdapter<TSource, TState>
    {
        /// <summary>
        /// Gets the state type's version, as in the number of times the type has been defined and redefined.
        /// </summary>
        int TypeVersion { get; }
        
        /// <summary>
        /// Gets the <typeparamref name="TSource"/> native state instance.
        /// </summary>
        /// <param name="raw">The <see cref="State{T}"/> instance from which the native state is derived</param>
        /// <returns><typeparamref name="TSource"/></returns>
        TSource FromRawState(State<TState> raw);
        
        /// <summary>
        /// Converts to <typeparamref name="TOtherState"/> native state instance.
        /// </summary>
        /// <param name="raw">the <see cref="State{T}"/> instance from which the native state is derived</param>
        /// <typeparam name="TOtherState">The state type to which to convert</typeparam>
        /// <returns>Converted state instance to <typeparamref name="TOtherState"/></returns>
        State<TOtherState> FromRawState<TOtherState>(State<TState> raw);

        /// <summary>
        /// Gets the <typeparamref name="TState"/> raw <see cref="State{T}"/> instance of the <typeparamref name="TSource"/> instance.
        /// </summary>
        /// <param name="id">The string identity of the state</param>
        /// <param name="state">The <see cref="State{T}"/> native state instance</param>
        /// <param name="stateVersion">The int state version</param>
        /// <param name="metadata">The <see cref="Metadata"/> for this state</param>
        /// <returns><typeparamref name="TState"/></returns>
        State<TState> ToRawState(string id, TSource state, int stateVersion, Metadata metadata);
        
        /// <summary>
        /// Gets the <typeparamref name="TState"/> raw <see cref="State{T}"/> instance of the <typeparamref name="TSource"/> instance.
        /// </summary>
        /// <param name="state">The <see cref="State{T}"/> native state instance</param>
        /// <param name="stateVersion">The int state version</param>
        /// <param name="metadata">The <see cref="Metadata"/> for this state</param>
        /// <returns><typeparamref name="TState"/></returns>
        State<TState> ToRawState(TSource state, int stateVersion, Metadata metadata);
        
        /// <summary>
        /// Gets the <typeparamref name="TState"/> raw <see cref="State{T}"/> instance of the <typeparamref name="TSource"/> instance.
        /// </summary>
        /// <param name="state">The <see cref="State{T}"/> native state instance</param>
        /// <param name="stateVersion">The int state version</param>
        /// <returns><typeparamref name="TState"/></returns>
        State<TState> ToRawState(TSource state, int stateVersion);
    }
    
    public abstract class StateAdapter<TSource, TState> : IStateAdapter<TSource, TState>
    {
        public abstract int TypeVersion { get; }

        public abstract TSource FromRawState(State<TState> raw);

        public abstract State<TOtherState> FromRawState<TOtherState>(State<TState> raw);
        
        public virtual State<TState> ToRawState(string id, TSource state, int stateVersion, Metadata metadata) =>
            throw new InvalidOperationException("Must override.");

        public virtual State<TState> ToRawState(TSource state, int stateVersion, Metadata metadata) =>
            ToRawState(State<TState>.NoOp, state, stateVersion, metadata);

        public virtual State<TState> ToRawState(TSource state, int stateVersion) =>
            ToRawState(state, stateVersion, Metadata.NullMetadata());
    }
}