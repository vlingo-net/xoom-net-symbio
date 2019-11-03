// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Actors;

namespace Vlingo.Symbio
{
    public class StateAdapterProvider
    {
        internal static string InternalName = Guid.NewGuid().ToString();

        private readonly Dictionary<Type, object> _adapters;
        private readonly Dictionary<string, object> _namedAdapters;
        private readonly IStateAdapter<object, string> _defaultTextStateAdapter;
        
        public static StateAdapterProvider Instance(World world) =>
            world.ResolveDynamic<StateAdapterProvider>(InternalName) ?? new StateAdapterProvider(world);
        
        public StateAdapterProvider(World world) : this() => world.RegisterDynamic(InternalName, this);
        
        public StateAdapterProvider()
        {
            _adapters = new Dictionary<Type, object>();
            _namedAdapters = new Dictionary<string, object>();
            _defaultTextStateAdapter = new DefaultTextStateAdapter();
        }
        
        public void RegisterAdapter<TSource, TState>(IStateAdapter<TSource, TState> adapter)
        {
            _adapters.Add(typeof(TSource), adapter);
            _namedAdapters.Add(nameof(TSource), adapter);
        }
        
        public void RegisterAdapter<TSource, TState>(TSource stateType, IStateAdapter<TSource, TState> adapter, Action<TSource, IStateAdapter<TSource, TState>> consumer)
        {
            _adapters.Add(stateType!.GetType(), adapter);
            _namedAdapters.Add(stateType.GetType().Name, adapter);
            consumer(stateType, adapter);
        }

        public State<TNewState> AsRaw<TState, TNewState>(string id, TState state, int stateVersion) =>
            AsRaw<TState, TNewState>(id, state, stateVersion, Metadata.NullMetadata());

        public State<TNewState> AsRaw<TState, TNewState>(string id, TState state, int stateVersion, Metadata metadata)
        {
            var  adapter = Adapter<TState, TNewState>();
            if (adapter != null)
            {
                return adapter.ToRawState(state, stateVersion, metadata);
            }
            
            return (State<TNewState>)(object)_defaultTextStateAdapter.ToRawState(state!, stateVersion, metadata);
        }

        public TSource FromRaw<TSource, TState>(State<TState> state)
        {
            var adapter = NamedAdapter<TSource, TState>(state);
            if (adapter != null)
            {
                return adapter.FromRawState(state);
            }
            
            return (TSource) _defaultTextStateAdapter.FromRawState((TextState)(object)state);
        }
        
        private IStateAdapter<TSource, TState> Adapter<TSource, TState>()
        {
            var adapter = (IStateAdapter<TSource, TState>) _adapters[typeof(TSource)];
            return adapter;
        }
        
        private IStateAdapter<TSource, TState> NamedAdapter<TSource, TState>(State<TState> state)
        {
            var adapter = (IStateAdapter<TSource, TState>) _namedAdapters[state.Type];
            return adapter;
        }
    }
}