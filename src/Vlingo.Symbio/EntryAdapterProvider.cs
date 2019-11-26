// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
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
        
        public void RegisterAdapter<TSource, TEntry>(IEntryAdapter<TSource, TEntry> adapter)
        {
            _adapters.Add(typeof(TSource), adapter);
            _namedAdapters.Add(nameof(TSource), adapter);
        }
        
        public void RegisterAdapter<TSource, TEntry>(TSource sourceType, IEntryAdapter<TSource, TEntry> adapter, Action<TSource, IEntryAdapter<TSource, TEntry>> consumer)
        {
            _adapters.Add(sourceType!.GetType(), adapter);
            _namedAdapters.Add(sourceType.GetType().Name, adapter);
            consumer(sourceType, adapter);
        }
        
        public IEnumerable<IEntry<TEntry>> AsEntries<TSource, TEntry>(IEnumerable<Source<TSource>> sources, Metadata? metadata)
        {
            return sources.Select(source => AsEntry<TSource, TEntry>(source, metadata)).ToList();
        }

        public IEntry<TEntry> AsEntry<TSource, TEntry>(Source<TSource> source, Metadata? metadata)
        {
            var  adapter = Adapter<TSource, TEntry>();
            if (adapter != null)
            {
                return metadata == null ? adapter.ToEntry(source) : adapter.ToEntry(source, metadata);
            }
            // TODO: if called by AsSources we will create each new instance in the loop
            return (IEntry<TEntry>) new DefaultTextEntryAdapter<TSource>().ToEntry(source, metadata!);
        }
        
        public IEnumerable<Source<TSource>> AsSources<TSource, TEntry>(IEnumerable<IEntry<TEntry>> entries)
        {
            return entries.Select(AsSource<TSource, TEntry>).ToList();
        }

        public Source<TSource> AsSource<TSource, TEntry>(IEntry<TEntry> entry)
        {
            var adapter = NamedAdapter<TSource, TEntry>(entry);
            if (adapter != null)
            {
                return adapter.FromEntry(entry);
            }
            // TODO: if called by AsSources we will create each new instance in the loop
            return new DefaultTextEntryAdapter<TSource>().FromEntry((IEntry<string>)entry);
        }
        
        private IEntryAdapter<TSource, TEntry>? Adapter<TSource, TEntry>()
        {
            if (!_adapters.ContainsKey(typeof(TSource)))
            {
                return null;
            }
            var adapter = (IEntryAdapter<TSource, TEntry>) _adapters[typeof(TSource)];
            return adapter;
        }
        
        private IEntryAdapter<TSource, TEntry>? NamedAdapter<TSource, TEntry>(IEntry<TEntry> entry)
        {
            if (!_namedAdapters.ContainsKey(entry.TypeName))
            {
                return null;
            }
            var adapter = (IEntryAdapter<TSource, TEntry>) _namedAdapters[entry.TypeName];
            return adapter;
        }
    }
}