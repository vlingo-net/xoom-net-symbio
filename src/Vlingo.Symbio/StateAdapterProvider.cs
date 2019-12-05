// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;

namespace Vlingo.Symbio
{
    public class StateAdapterProvider
    {
        internal static string InternalName = Guid.NewGuid().ToString();

        private readonly Dictionary<Type, object> _adapters;
        private readonly Dictionary<string, object> _namedAdapters;
        private readonly IStateAdapter<object, TextState> _defaultTextStateAdapter;
        
        public static StateAdapterProvider Instance(World world)
        {
            // TODO: Refactor when https://github.com/vlingo-net/vlingo-net-actors/issues/75 is done merged and deployed
            try
            {
                return world.ResolveDynamic<StateAdapterProvider>(InternalName);
            }
            catch (Exception)
            {
                return new StateAdapterProvider(world);
            }
        }

        public StateAdapterProvider(World world) : this() => world.RegisterDynamic(InternalName, this);
        
        public StateAdapterProvider()
        {
            _adapters = new Dictionary<Type, object>();
            _namedAdapters = new Dictionary<string, object>();
            _defaultTextStateAdapter = new DefaultTextStateAdapter();
        }
        
        public void RegisterAdapter<TState, TRawState>(IStateAdapter<TState, TRawState> adapter) where TRawState : IState
        {
            _adapters.Add(typeof(TState), adapter);
            _namedAdapters.Add(typeof(TState).FullName, adapter);
        }
        
        public void RegisterAdapter<TState, TRawState>(TState stateType, IStateAdapter<TState, TRawState> adapter, Action<TState, IStateAdapter<TState, TRawState>> consumer) where TRawState : IState
        {
            _adapters.Add(stateType!.GetType(), adapter);
            _namedAdapters.Add(stateType.GetType().Name, adapter);
            consumer(stateType, adapter);
        }

        public TRawState AsRaw<TState, TRawState>(string id, TState state, int stateVersion) where TRawState : IState =>
            AsRaw<TState, TRawState>(id, state, stateVersion, Metadata.NullMetadata());

        public TRawState AsRaw<TState, TRawState>(string id, TState state, int stateVersion, Metadata metadata) where TRawState : IState
        {
            var adapter = Adapter<TState, TRawState>();
            if (adapter != null)
            {
                return adapter.ToRawState(state, stateVersion, metadata);
            }

            return (TRawState) (object) _defaultTextStateAdapter.ToRawState(id, state!, stateVersion, metadata);
        }

        public TState FromRaw<TState, TRawState>(TRawState state) where TRawState : IState
        {
            var adapter = NamedAdapter<TState, TRawState>(state);
            if (adapter != null)
            {
                return adapter.FromRawState(state);
            }
            
            return (TState) _defaultTextStateAdapter.FromRawState((TextState)(object)state);
        }
        
        private IStateAdapter<TState, TRawState>? Adapter<TState, TRawState>() where TRawState : IState
        {
            if (!_adapters.ContainsKey(typeof(TState)))
            {
                return null;
            }
            var adapter = (IStateAdapter<TState, TRawState>) _adapters[typeof(TState)];
            return adapter;
        }

        private IStateAdapter<TState, TRawState>? NamedAdapter<TState, TRawState>(TRawState state) where TRawState : IState
        {
            if (!_namedAdapters.ContainsKey(state.Type))
            {
                return null;
            }
            var adapter = (IStateAdapter<TState, TRawState>) _namedAdapters[state.Type];
            return adapter;
        }
    }
}