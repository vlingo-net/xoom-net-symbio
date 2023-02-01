// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Streams;
using Vlingo.Xoom.Symbio.Store.Journal;
using Vlingo.Xoom.Symbio.Store.Object;
using Vlingo.Xoom.Symbio.Store.State;

namespace Vlingo.Xoom.Symbio.Store;

/// <summary>
/// Reads <see cref="IEntry"/> instances from a <see cref="IJournal{T}"/>, <see cref="IObjectStore"/>,
/// or <see cref="IStateStore"/>, by means of a given <see cref="IEntryReader"/>. You must
/// provide a <code>maximumEntries</code> to the constructor, or use one of the two
/// methods that requires a <code>maximumElements</code>.
/// </summary>
public sealed class EntryReaderSource<T> : Actor, ISource<T>, IScheduled<object>
{
    private readonly Queue<IEntry> _cache;
    private readonly ICancellable _cancellable;
    private readonly IEntryReader _entryReader;
    private readonly long _flowElementsRate;
    private bool _reading;
    private readonly EntryAdapterProvider _entryAdapterProvider;

    /// <summary>
    /// Constructs my default state.
    /// </summary>
    /// <param name="entryReader">The <see cref="_entryReader"/> from which to read entry elements</param>
    /// <param name="entryAdapterProvider">The <see cref="EntryAdapterProvider"/> used to turn <see cref="IEntry"/> instances into <see cref="Source{T}"/> instances</param>
    /// <param name="flowElementsRate">The long maximum elements to read at once</param>
    public EntryReaderSource(IEntryReader entryReader, EntryAdapterProvider entryAdapterProvider, long flowElementsRate)
    {
        _entryReader = entryReader;
        _entryAdapterProvider = entryAdapterProvider;
        _flowElementsRate = flowElementsRate;
        _cache = new Queue<IEntry>();

        _cancellable = Scheduler.Schedule(SelfAs<IScheduled<object?>>(), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(Stream.FastProbeInterval));
    }
        
    public ISource<T> Empty() => Streams.Source<T>.Empty();

    public ISource<T> Only(IEnumerable<T> elements) => Streams.Source<T>.Only(elements);

    public ISource<long> RangeOf(long startInclusive, long endExclusive) => Streams.Source<T>.RangeOf(startInclusive, endExclusive);

    public long OrElseMaximum(long elements) => Streams.Source<T>.OrElseMaximum(elements);

    public long OrElseMinimum(long elements) => Streams.Source<T>.OrElseMinimum(elements);

    public ISource<T> With(IEnumerable<T> iterable) => Streams.Source<T>.With(iterable);

    public ISource<T> With(IEnumerable<T> iterable, bool slowIterable) => Streams.Source<T>.With(iterable, slowIterable);

    public ISource<T> With(Func<T> supplier) => Streams.Source<T>.With(supplier);

    public ISource<T> With(Func<T> supplier, bool slowSupplier) => Streams.Source<T>.With(supplier, slowSupplier);

    public ICompletes<Elements<T>> Next()
    {
        if (_cache.Any())
        {
            var next = new List<EntryBundle>();

            for (var index = 0; index < _flowElementsRate && _cache.Any(); ++index)
            {
                var entry = _cache.Dequeue();
                // This little trick gets a PersistentEntry to a BaseEntry: entry.WithId(entry.Id)
                var normalized = entry is BaseEntry ? entry : entry.WithId(entry.Id);
                var source = _entryAdapterProvider.AsSource<ISource, IEntry>(normalized);
                next.Add(new EntryBundle(entry, source));
            }
                
            // TODO: should have been -> var elements = Elements<T>.Of(next.Cast<T>().ToArray());
            var partialElements = Elements<T>.Of(next.Select(e => e.Source).Cast<T>().ToArray());
            return Completes().With(partialElements);
        }
        return Completes().With(Elements<T>.Empty());
    }

    public ICompletes<Elements<T>> Next(int maximumElements) => Next();

    public ICompletes<Elements<T>> Next(long index) => Next();

    public ICompletes<Elements<T>> Next(long index, int maximumElements) => Next();

    public ICompletes<bool> IsSlow() => Completes().With(false);

    //====================================
    // Internal implementation
    //====================================
        
    public void IntervalSignal(IScheduled<object> scheduled, object data)
    {
        if (!_cache.Any() && !_reading)
        {
            _reading = true;
            var max = _flowElementsRate > int.MaxValue ? int.MaxValue : (int) _flowElementsRate;
            _entryReader.ReadNext(max).AndThenConsume(entries =>
            {
                foreach (var entry in entries)
                {
                    _cache.Enqueue(entry);
                }
                _reading = false;
            });
        }
    }
        
    //====================================
    // Internal implementation
    //====================================

    public override void Stop()
    {
        _cancellable.Cancel();
        base.Stop();
    }
}