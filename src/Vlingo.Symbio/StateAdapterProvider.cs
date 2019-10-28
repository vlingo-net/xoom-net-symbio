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
        
        public void RegisterAdapter<TS, TStateType>(IStateAdapter<TS, TStateType> adapter)
        {
            _adapters.Add(typeof(TS), adapter);
            _namedAdapters.Add(nameof(TS), adapter);
        }
        
        public void RegisterAdapter<TS, TStateType>(TS stateType, IStateAdapter<TS, TStateType> adapter, Action<TS, IStateAdapter<TS, TStateType>> consumer)
        {
            _adapters.Add(stateType!.GetType(), adapter);
            _namedAdapters.Add(stateType.GetType().Name, adapter);
            consumer(stateType, adapter);
        }

        public State<TStateType> AsRaw<TS, TStateType>(string id, TS state, int stateVersion) =>
            AsRaw<TS, TStateType>(id, state, stateVersion, Metadata.NullMetadata());

        public State<TStateType> AsRaw<TS, TStateType>(string id, TS state, int stateVersion, Metadata metadata)
        {
            var  adapter = Adapter<TS, TStateType>();
            if (adapter != null)
            {
                return adapter.ToRawState(state, stateVersion, metadata);
            }
            
            return (State<TStateType>)(object)_defaultTextStateAdapter.ToRawState(state!, stateVersion, metadata);
        }

        public TS FromRaw<TS, TStateType>(State<TStateType> state)
        {
            var adapter = NamedAdapter<TS, TStateType>(state);
            if (adapter != null)
            {
                return adapter.FromRawState(state);
            }
            
            return (TS) _defaultTextStateAdapter.FromRawState((TextState)(object)state);
        }
        
        private IStateAdapter<TS, TStateType> Adapter<TS, TStateType>()
        {
            var adapter = (IStateAdapter<TS, TStateType>) _adapters[typeof(TS)];
            return adapter;
        }
        
        private IStateAdapter<TS, TStateType> NamedAdapter<TS, TStateType>(State<TStateType> state)
        {
            var adapter = (IStateAdapter<TS, TStateType>) _namedAdapters[state.Type];
            return adapter;
        }
    }
}