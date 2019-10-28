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
    public class EntryAdapterProvider
    {
        internal static string InternalName = Guid.NewGuid().ToString();

        private readonly Dictionary<Type, object> _adapters;
        private readonly Dictionary<string, object> _namedAdapters;
        
        /// <summary>
        /// Answer the <see cref="EntryAdapterProvider"/> held by the <see cref="World"/>.
        /// If no such instance exists, create and answer a new instance of
        /// <see cref="EntryAdapterProvider"/> registered with <see cref="World"/>.
        /// </summary>
        /// <param name="world">The World where the EntryAdapterProvider is held</param>
        /// <returns><see cref="EntryAdapterProvider"/></returns>
        public static EntryAdapterProvider Instance(World world) =>
            world.ResolveDynamic<EntryAdapterProvider>(InternalName) ?? new EntryAdapterProvider(world);
        
        public EntryAdapterProvider(World world) : this() => world.RegisterDynamic(InternalName, this);
        
        public EntryAdapterProvider()
        {
            _adapters = new Dictionary<Type, object>();
            _namedAdapters = new Dictionary<string, object>();
        }
        
        public void RegisterAdapter<TS, TE>(IEntryAdapter<TS, TE> adapter)
        {
            _adapters.Add(typeof(TS), adapter);
            _namedAdapters.Add(nameof(TS), adapter);
        }
        
        public void RegisterAdapter<TS, TE>(TS sourceType, IEntryAdapter<TS, TE> adapter, Action<TS, IEntryAdapter<TS, TE>> consumer)
        {
            _adapters.Add(sourceType!.GetType(), adapter);
            _namedAdapters.Add(sourceType.GetType().Name, adapter);
            consumer(sourceType, adapter);
        }
        
        public IEnumerable<IEntry<TE>> AsEntries<TS, TE>(IEnumerable<Source<TS>> sources, Metadata metadata)
        {
            var defaultTextEntryAdapter = new DefaultTextEntryAdapter<TS>();
            return sources.Select(source => AsEntry<TS, TE>(source, metadata, defaultTextEntryAdapter)).ToList();
        }

        public IEntry<TE> AsEntry<TS, TE>(Source<TS> source, Metadata metadata, IEntryAdapter<TS, string> defaultTextEntryAdapter)
        {
            var  adapter = Adapter<TS, TE>();
            if (adapter != null)
            {
                return adapter.ToEntry(source, metadata);
            }
            
            return (IEntry<TE>) defaultTextEntryAdapter.ToEntry(source, metadata);
        }
        
        public IEnumerable<Source<TS>> AsSources<TS, TE>(IEnumerable<IEntry<TE>> entries)
        {
            var defaultTextEntryAdapter = new DefaultTextEntryAdapter<TS>();
            return entries.Select(entry => AsSource(entry, defaultTextEntryAdapter)).ToList();
        }

        public Source<TS> AsSource<TS, TE>(IEntry<TE> entry, IEntryAdapter<TS, string> defaultTextEntryAdapter)
        {
            var adapter = NamedAdapter<TS, TE>(entry);
            if (adapter != null)
            {
                return adapter.FromEntry(entry);
            }
            
            return defaultTextEntryAdapter.FromEntry((TextEntry)(object)entry);
        }
        
        private IEntryAdapter<TS, TE> Adapter<TS, TE>()
        {
            var adapter = (IEntryAdapter<TS, TE>) _adapters[typeof(TS)];
            return adapter;
        }
        
        private IEntryAdapter<TS, TE> NamedAdapter<TS, TE>(IEntry<TE> entry)
        {
            var adapter = (IEntryAdapter<TS, TE>) _namedAdapters[entry.TypeName];
            return adapter;
        }
    }
}