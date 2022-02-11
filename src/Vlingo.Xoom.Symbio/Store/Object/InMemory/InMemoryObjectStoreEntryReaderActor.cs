// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Streams;
using Vlingo.Xoom.Symbio.Store.Journal;

namespace Vlingo.Xoom.Symbio.Store.Object.InMemory;

public class InMemoryObjectStoreEntryReaderActor : Actor, IObjectStoreEntryReader
{
    private readonly EntryAdapterProvider _entryAdapterProvider;
    private int _currentIndex;
    private readonly List<IEntry> _entriesView;
    private readonly string _name;

    public InMemoryObjectStoreEntryReaderActor(List<IEntry> entriesView, string name)
    {
        _entriesView = entriesView;
        _name = name;
        _currentIndex = 0;
        _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
    }

    public void Close()
    {
    }
        
    public ICompletes<IEntry> ReadNext()
    {
        if (_currentIndex < _entriesView.Count)
        {
            return Completes().With(_entriesView[_currentIndex++]);
        }
            
        return Completes().With<IEntry>(null!);
    }

    public ICompletes<IEntry> ReadNext(string fromId)
    {
        SeekTo(fromId);
        return ReadNext();
    }

    public ICompletes<IEnumerable<IEntry>> ReadNext(int maximumEntries)
    {
        var entries = new List<IEntry>(maximumEntries);

        for (var count = 0; count < maximumEntries; ++count)
        {
            if (_currentIndex < _entriesView.Count)
            {
                entries.Add(_entriesView[_currentIndex++]);
            }
            else
            {
                break;
            }
        }
        return Completes().With<IEnumerable<IEntry>>(entries);
    }

    public ICompletes<IEnumerable<IEntry>> ReadNext(string fromId, int maximumEntries)
    {
        SeekTo(fromId);
        return ReadNext(maximumEntries);
    }

    public void Rewind() => _currentIndex = 0;

    public ICompletes<string> SeekTo(string id)
    {
        string currentId;

        switch (id)
        {
            case EntryReader.Beginning:
                Rewind();
                currentId = ReadCurrentId();
                break;
            case EntryReader.End:
                ToEnd();
                currentId = ReadCurrentId();
                break;
            case EntryReader.Query:
                currentId = ReadCurrentId();
                break;
            default:
                To(id);
                currentId = ReadCurrentId();
                break;
        }

        return Completes().With(currentId);
    }

    public string Beginning { get; } = EntryReader.Beginning;
        
    public string End { get; } = EntryReader.End;
        
    public string Query { get; } = EntryReader.Query;
        
    public int DefaultGapPreventionRetries { get; } = EntryReader.DefaultGapPreventionRetries;
        
    public long DefaultGapPreventionRetryInterval { get; } = EntryReader.DefaultGapPreventionRetryInterval;

    public ICompletes<string> Name => Completes().With(_name);

    public ICompletes<long> Size => Completes().With((long) _entriesView.Count);
        
    public ICompletes<IStream> StreamAll() => 
        Completes().With((IStream) new EntryReaderStream(Stage, SelfAs<IJournalReader>(), _entryAdapterProvider));

    private void ToEnd() => _currentIndex = _entriesView.Count - 1;

    private string ReadCurrentId()
    {
        if (_currentIndex < _entriesView.Count)
        {
            var currentId = _entriesView[_currentIndex].Id;
            return currentId;
        }
        return "-1";
    }
        
    private void To(string id)
    {
        Rewind();
        while (_currentIndex < _entriesView.Count)
        {
            var entry = _entriesView[_currentIndex];
            if (entry.Id.Equals(id))
            {
                return;
            }
            ++_currentIndex;
        }
    }
}