// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;

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
        public static EntryAdapterProvider Instance(World world)
        {
            var entryAdapterProvider = world.ResolveDynamic<EntryAdapterProvider>(InternalName);
            if (entryAdapterProvider == null)
            {
                return new EntryAdapterProvider(world);
            }
            
            return entryAdapterProvider;
        }
        
        public EntryAdapterProvider(World world) : this() => world.RegisterDynamic(InternalName, this);
        
        public EntryAdapterProvider()
        {
            _adapters = new Dictionary<Type, object>();
            _namedAdapters = new Dictionary<string, object>();
        }
        
        public void RegisterAdapter<TSource, TEntry>(IEntryAdapter<TSource, TEntry> adapter) where TEntry : IEntry where TSource : ISource
        {
            if (!_adapters.ContainsKey(typeof(TSource)))
            {
                _adapters.Add(typeof(TSource), adapter);
                _namedAdapters.Add(typeof(TSource).FullName!, adapter);   
            }
        }

        public void RegisterAdapter<TSource, TEntry>(IEntryAdapter<TSource, TEntry> adapter, Action<IEntryAdapter<TSource, TEntry>> consumer) where TEntry : IEntry where TSource : ISource
        {
            var sourceType = typeof(TSource);
            _adapters.Add(sourceType, adapter);
            _namedAdapters.Add(sourceType.Name, adapter);
            consumer(adapter);   
        }
        
        public IEnumerable<TEntry> AsEntries<TSource, TEntry>(IEnumerable<ISource> sources, int version, Metadata? metadata) where TEntry : IEntry => 
            sources.Select(source => AsEntry<TSource, TEntry>(source, version, metadata)).ToList();

        public IEnumerable<TEntry> AsEntries<TSource, TEntry>(IEnumerable<ISource> sources, Metadata? metadata) where TEntry : IEntry where TSource : ISource => 
            AsEntries<TSource, TEntry>(sources, Entry<TEntry>.DefaultVersion, metadata);

        public TEntry AsEntry<TSource, TEntry>(ISource source, int startingVersion, Metadata? metadata) where TEntry : IEntry
        {
            var adapter = Adapter<TSource, TEntry>(source.GetType());

            if (adapter != null)
            {
                return metadata == null ? adapter.ToEntry((TSource)source) : adapter.ToEntry((TSource) source, startingVersion, metadata);
            }
            // TODO: if called by AsSources we will create each new instance in the loop
            return (TEntry)(object) new DefaultTextEntryAdapter<ISource>().ToEntry(source, startingVersion, metadata!);
        }
        
        public TEntry AsEntry<TSource, TEntry>(ISource source, Metadata? metadata) where TEntry : IEntry where TSource : ISource => 
            AsEntry<TSource, TEntry>(source, Entry<TEntry>.DefaultVersion, metadata);

        public IEnumerable<TSource> AsSources<TSource, TEntry>(IEnumerable<TEntry> entries) where TEntry : IEntry where TSource : ISource
            => entries.Select(AsSource<TSource, TEntry>).ToList();

        public TSource AsSource<TSource, TEntry>(TEntry entry) where TEntry : IEntry where TSource : ISource
        {
            var adapter = NamedAdapter<TSource, TEntry>(entry);
            if (adapter != null)
            {
                return adapter.FromEntry(entry);
            }
            // TODO: if called by AsSources we will create each new instance in the loop
            return new DefaultTextEntryAdapter<TSource>().FromEntry((TextEntry)(object)entry);
        }

        private IEntryAdapter<TSource, TEntry>? Adapter<TSource, TEntry>(Type source) where TEntry : IEntry
        {
            if (!_adapters.ContainsKey(source))
            {
                return null;
            }
            
            var adapter = _adapters[source] as IEntryAdapter<TSource, TEntry>;
            return adapter;
        }

        private IEntryAdapter<TSource, TEntry>? NamedAdapter<TSource, TEntry>(TEntry entry) where TEntry : IEntry where TSource : ISource
        {
            if (!_namedAdapters.ContainsKey(entry.TypeName))
            {
                return null;
            }
            var adapter = _namedAdapters[entry.TypeName] as IEntryAdapter<TSource, TEntry>;
            return adapter;
        }
    }
}