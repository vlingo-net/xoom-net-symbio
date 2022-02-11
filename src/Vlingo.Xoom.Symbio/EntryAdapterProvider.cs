// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;

namespace Vlingo.Xoom.Symbio;

public class EntryAdapterProvider
{
    internal static readonly string InternalName = Guid.NewGuid().ToString();

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
        
    public void RegisterAdapter(IEntryAdapter adapter)
    {
        if (!_adapters.ContainsKey(adapter.SourceType))
        {
            _adapters.Add(adapter.SourceType, adapter);
            _namedAdapters.Add(adapter.SourceType.AssemblyQualifiedName!, adapter);   
        }
    }

    public void RegisterAdapter(IEntryAdapter adapter, Action<IEntryAdapter> consumer)
    {
        var sourceType = adapter.SourceType;
        _adapters.Add(sourceType, adapter);
        _namedAdapters.Add(sourceType.Name, adapter);
        consumer(adapter);   
    }
        
    public IEnumerable<IEntry> AsEntries(IEnumerable<ISource> sources, int version, Metadata? metadata) => 
        sources.Select(source => AsEntry(source, version, metadata)).ToList();

    public IEnumerable<IEntry> AsEntries(IEnumerable<ISource> sources, Metadata? metadata) => 
        AsEntries(sources, Entry<object>.DefaultVersion, metadata);

    public IEntry AsEntry(ISource source, int startingVersion, Metadata? metadata)
    {
        var adapter = Adapter(source.GetType());

        if (adapter != null)
        {
            return metadata == null ? adapter.ToEntry(source) : adapter.ToEntry(source, startingVersion, metadata);
        }
        // TODO: if called by AsSources we will create each new instance in the loop
        return new DefaultTextEntryAdapter<ISource>().ToEntry(source, startingVersion, metadata!);
    }
        
    public IEntry AsEntry(ISource source, Metadata? metadata) => 
        AsEntry(source, Entry<object>.DefaultVersion, metadata);

    public IEnumerable<ISource> AsSources<TSource, TEntry>(IEnumerable<TEntry> entries) where TEntry : IEntry where TSource : ISource
        => entries.Select(AsSource<TSource, TEntry>).ToList();

    public ISource AsSource<TSource, TEntry>(TEntry entry) where TEntry : IEntry where TSource : ISource
    {
        var adapter = NamedAdapter(entry);
        if (adapter != null)
        {
            return adapter.FromEntry(entry);
        }
        // TODO: if called by AsSources we will create each new instance in the loop
        return new DefaultTextEntryAdapter<TSource>().FromEntry((TextEntry)(object)entry);
    }

    private IEntryAdapter? Adapter(Type source)
    {
        if (!_adapters.ContainsKey(source))
        {
            return null;
        }
            
        var adapter = _adapters[source] as IEntryAdapter;
        return adapter;
    }

    private IEntryAdapter? NamedAdapter(IEntry entry)
    {
        if (!_namedAdapters.ContainsKey(entry.TypeName))
        {
            return null;
        }
        var adapter = _namedAdapters[entry.TypeName] as IEntryAdapter;
        return adapter;
    }
}