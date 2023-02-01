// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Concurrent;
using System.Collections.Generic;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Symbio.Store.State;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Actors.TestKit;
using Vlingo.Xoom.Streams;
using Vlingo.Xoom.Symbio.Store;

namespace Vlingo.Xoom.Symbio.Tests.Store.State;

public class MessageCountingStateStoreActor : Actor, IStateStore
{
    private readonly MessageCountingResults _results;
    private readonly int _totalPartitions;

    public MessageCountingStateStoreActor(MessageCountingResults results, int totalPartitions)
    {
        _results = results;
        _totalPartitions = totalPartitions;
    }

    public ICompletes<IStateStoreEntryReader> EntryReader<TEntry>(string name) where TEntry : IEntry
    {
        _results.PutIncrementEntryReader();

        return Completes().With<IStateStoreEntryReader>(null);
    }

    public Actor Actor { get; } = null;

    public void Read<TState1>(string id, IReadResultInterest interest) => _results.PutIncrementRead(id, _totalPartitions);

    public void Read<TState1>(string id, IReadResultInterest interest, object @object)
        => _results.PutIncrementRead(id, _totalPartitions);

    public void ReadAll<TState1>(IEnumerable<TypedStateBundle> bundles, IReadResultInterest interest,
        object @object)
        => _results.PutIncrementReadAll();

    public ICompletes<IStream> StreamAllOf<TState>()
    {
        _results.PutIncrementStreamAllOf();

        return Completes().With((IStream) null);
    }

    public ICompletes<IStream> StreamSomeUsing(QueryExpression query)
    {
        _results.PutIncrementStreamSomeUsing();

        return Completes().With((IStream) null);
    }

    public void Write<TState1>(string id, TState1 state, int stateVersion, IWriteResultInterest interest)
        => _results.PutIncrementWrite(id, _totalPartitions);

    public void Write<TState1, TSource>(string id, TState1 state, int stateVersion,
        IEnumerable<TSource> sources,
        IWriteResultInterest interest) => _results.PutIncrementWrite(id, _totalPartitions);

    public void Write<TState1>(string id, TState1 state, int stateVersion, Metadata metadata,
        IWriteResultInterest interest)
        => _results.PutIncrementWrite(id, _totalPartitions);

    public void Write<TState1, TSource>(string id, TState1 state, int stateVersion,
        IEnumerable<TSource> sources, Metadata metadata,
        IWriteResultInterest interest) => _results.PutIncrementWrite(id, _totalPartitions);

    public void Write<TState1>(string id, TState1 state, int stateVersion, IWriteResultInterest interest,
        object @object)
        => _results.PutIncrementWrite(id, _totalPartitions);

    public void Write<TState1, TSource>(string id, TState1 state, int stateVersion,
        IEnumerable<TSource> sources,
        IWriteResultInterest interest, object @object) => _results.PutIncrementWrite(id, _totalPartitions);

    public void Write<TState1>(string id, TState1 state, int stateVersion, Metadata metadata,
        IWriteResultInterest interest,
        object @object)
        => _results.PutIncrementWrite(id, _totalPartitions);
        
    public void Write<TState1, TSource>(string id, TState1 state, int stateVersion,
        IEnumerable<TSource> sources, Metadata metadata,
        IWriteResultInterest interest, object @object) => _results.PutIncrementWrite(id, _totalPartitions);
}

public class MessageCountingResults
{
    private readonly AccessSafely _access;

    private readonly AtomicInteger _ctor = new AtomicInteger(0);
    private readonly AtomicInteger _entryReader = new AtomicInteger(0);
    private readonly AtomicInteger _read = new AtomicInteger(0);
    private readonly AtomicInteger _readAll = new AtomicInteger(0);
    private readonly AtomicInteger _readerCtor = new AtomicInteger(0);
    private readonly ConcurrentDictionary<int, int> _readPartitions = new ConcurrentDictionary<int, int>();
    private readonly AtomicInteger _streamAllOf = new AtomicInteger(0);
    private readonly AtomicInteger _streamSomeUsing = new AtomicInteger(0);
    private readonly AtomicInteger _write = new AtomicInteger(0);
    private readonly ConcurrentDictionary<int, int> _writePartitions = new ConcurrentDictionary<int, int>();
    private readonly AtomicInteger _writerCtor = new AtomicInteger(0);

    public MessageCountingResults(int times)
    {
        _access = AccessSafely.AfterCompleting(times);

        _access.WritingWith("ctor", (InstantiationType type) =>
        {
            _ctor.IncrementAndGet();
            if (type == InstantiationType.Reader)
                _readerCtor.IncrementAndGet();
            else if (type == InstantiationType.Writer) _writerCtor.IncrementAndGet();
        });
        _access.ReadingWith("ctor", () => _ctor.Get());
        _access.ReadingWith("readerCtor", () => _readerCtor.Get());
        _access.ReadingWith("writerCtor", () => _writerCtor.Get());

        _access.WritingWith("read", (string id, int totalPartitions) =>
        {
            _read.IncrementAndGet();
            var partition = PartitioningStateStore.PartitionOf(id, totalPartitions);
            _readPartitions.TryGetValue(partition, out var count);
            _readPartitions.AddOrUpdate(partition, i =>  1, (x, y) => count + 1);
        });
        _access.ReadingWith("read", () => _read.Get());

        _access.WritingWith<int>("readAll", one => _readAll.IncrementAndGet());
        _access.ReadingWith("readAll", () => _readAll.Get());

        _access.WritingWith<int>("streamAllOf", one => _streamAllOf.IncrementAndGet());
        _access.ReadingWith("streamAllOf", () => _streamAllOf.Get());

        _access.WritingWith<int>("streamSomeUsing", one => _streamSomeUsing.IncrementAndGet());
        _access.ReadingWith("streamSomeUsing", () => _streamSomeUsing.Get());

        _access.WritingWith("write", (string id, int totalPartitions) =>
        {
            _write.IncrementAndGet();
            var partition = PartitioningStateStore.PartitionOf(id, totalPartitions);
            _writePartitions.TryGetValue(partition, out var count);
            _writePartitions.AddOrUpdate(partition, i =>  1, (x, y) => count + 1);
        });
        _access.ReadingWith("write", () => _write.Get());

        _access.WritingWith<int>("entryReader", one => _entryReader.IncrementAndGet());
        _access.ReadingWith("entryReader", () => _entryReader.Get());
    }

    public int GetCtor() => _access.ReadFrom<int>("ctor");

    public int GetReaderCtor() => _access.ReadFrom<int>("readerCtor");

    public int GetWriterCtor() => _access.ReadFrom<int>("writerCtor");

    public int GetRead() => _access.ReadFrom<int>("read");

    public int GetReadPartitionCount(int partition)
    {
        _access.ReadFrom<int>("read");
        return _readPartitions[partition];
    }

    public int GetReadAll() => _access.ReadFrom<int>("readAll");

    public int GetStreamAllOf() => _access.ReadFrom<int>("streamAllOf");

    public int GetStreamSomeUsing() => _access.ReadFrom<int>("streamSomeUsing");

    public int GetWrite() => _access.ReadFrom<int>("write");

    public int GetWritePartitionCount(int partition)
    {
        _access.ReadFrom<int>("read");
        return _writePartitions[partition];
    }

    public int GetEntryReader() => _access.ReadFrom<int>("entryReader");
        
    public void IncrementCtor(InstantiationType type) => _access.WriteUsing("ctor", type);

    public void PutIncrementRead(string id, int totalPartitions) => _access.WriteUsing("read", id, totalPartitions);

    public void PutIncrementReadAll() => _access.WriteUsing("readAll", 1);
        
    public void PutIncrementStreamAllOf() => _access.WriteUsing("streamAllOf", 1);

    public void PutIncrementStreamSomeUsing() => _access.WriteUsing("streamSomeUsing", 1);

    public void PutIncrementWrite(string id, int totalPartitions) => _access.WriteUsing("write", id, totalPartitions);

    public void PutIncrementEntryReader() => _access.WriteUsing("entryReader", 1);
}
    
public enum InstantiationType
{
    Reader,
    Writer
}