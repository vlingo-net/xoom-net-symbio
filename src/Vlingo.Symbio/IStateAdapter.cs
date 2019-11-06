// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio
{
    public interface IStateAdapter
    {
        object ToRawState<T>(T state, int stateVersion, Metadata metadata);
    }
    
    /// <summary>
    /// Adapts the native state to the raw <see cref="State{T}"/>, and the raw <see cref="State{T}"/> to the native state.
    /// </summary>
    /// <typeparam name="TState">The native type of the state</typeparam>
    /// <typeparam name="TRawState">the raw <see cref="State{T}"/> of the state</typeparam>
    public interface IStateAdapter<TState, TRawState> : IStateAdapter
    {
        /// <summary>
        /// Gets the state type's version, as in the number of times the type has been defined and redefined.
        /// </summary>
        int TypeVersion { get; }
        
        /// <summary>
        /// Gets the <typeparamref name="TState"/> native state instance.
        /// </summary>
        /// <param name="raw">The <see cref="State{T}"/> instance from which the native state is derived</param>
        /// <returns><typeparamref name="TState"/></returns>
        TState FromRawState(State<TRawState> raw);
        
        /// <summary>
        /// Converts to <typeparamref name="TOtherState"/> native state instance.
        /// </summary>
        /// <param name="raw">the <see cref="State{T}"/> instance from which the native state is derived</param>
        /// <typeparam name="TOtherState">The state type to which to convert</typeparam>
        /// <returns>Converted state instance to <typeparamref name="TOtherState"/></returns>
        TOtherState FromRawState<TOtherState>(State<TRawState> raw);

        /// <summary>
        /// Gets the <typeparamref name="TRawState"/> raw <see cref="State{T}"/> instance of the <typeparamref name="TState"/> instance.
        /// </summary>
        /// <param name="id">The string identity of the state</param>
        /// <param name="state">The <see cref="State{T}"/> native state instance</param>
        /// <param name="stateVersion">The int state version</param>
        /// <param name="metadata">The <see cref="Metadata"/> for this state</param>
        /// <returns><typeparamref name="TRawState"/></returns>
        State<TRawState> ToRawState(string id, TState state, int stateVersion, Metadata metadata);
        
        /// <summary>
        /// Gets the <typeparamref name="TRawState"/> raw <see cref="State{T}"/> instance of the <typeparamref name="TState"/> instance.
        /// </summary>
        /// <param name="state">The <see cref="State{T}"/> native state instance</param>
        /// <param name="stateVersion">The int state version</param>
        /// <param name="metadata">The <see cref="Metadata"/> for this state</param>
        /// <returns><typeparamref name="TRawState"/></returns>
        State<TRawState> ToRawState(TState state, int stateVersion, Metadata metadata);
        
        /// <summary>
        /// Gets the <typeparamref name="TRawState"/> raw <see cref="State{T}"/> instance of the <typeparamref name="TState"/> instance.
        /// </summary>
        /// <param name="state">The <see cref="State{T}"/> native state instance</param>
        /// <param name="stateVersion">The int state version</param>
        /// <returns><typeparamref name="TRawState"/></returns>
        State<TRawState> ToRawState(TState state, int stateVersion);
    }
    
    public abstract class StateAdapter<TState, TRawState> : IStateAdapter<TState, TRawState>
    {
        public abstract int TypeVersion { get; }

        public abstract TState FromRawState(State<TRawState> raw);

        public abstract TOtherState FromRawState<TOtherState>(State<TRawState> raw);

        public virtual object ToRawState<T>(T state, int stateVersion, Metadata metadata) =>
            ToRawState((TState)(object)state!, stateVersion, metadata);

        public virtual State<TRawState> ToRawState(string id, TState state, int stateVersion, Metadata metadata) =>
            throw new InvalidOperationException("Must override.");

        public virtual State<TRawState> ToRawState(TState state, int stateVersion, Metadata metadata) =>
            ToRawState(State<TRawState>.NoOp, state, stateVersion, metadata);

        public virtual State<TRawState> ToRawState(TState state, int stateVersion) =>
            ToRawState(state, stateVersion, Metadata.NullMetadata());
    }
}