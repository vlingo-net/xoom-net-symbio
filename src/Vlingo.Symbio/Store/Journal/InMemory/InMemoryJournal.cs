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
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;
using Vlingo.Symbio.Store.Dispatch.Control;
using Vlingo.Symbio.Store.Dispatch.InMemory;

namespace Vlingo.Symbio.Store.Journal.InMemory
{
    public class InMemoryJournal<TEntry, TState> : Journal<TEntry>, IStoppable
    {
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly StateAdapterProvider _stateAdapterProvider;
        private readonly List<IEntry<TEntry>> _journal;
        private readonly Dictionary<string, IJournalReader<TEntry>> _journalReaders;
        private readonly Dictionary<string, IStreamReader<TEntry>> _streamReaders;
        private readonly Dictionary<string, Dictionary<int, int>> _streamIndexes;
        private readonly Dictionary<string, State<TState>> _snapshots;
        private readonly List<Dispatchable<TEntry, TState>> _dispatchables;
        private readonly IDispatcher<Dispatchable<TEntry, TState>> _dispatcher;
        private readonly IDispatcherControl _dispatcherControl;

        public InMemoryJournal(IDispatcher<Dispatchable<TEntry, TState>> dispatcher, World world, long checkConfirmationExpirationInterval = 1000L, long confirmationExpiration = 1000L)
        {
            _dispatcher = dispatcher;
            _entryAdapterProvider = EntryAdapterProvider.Instance(world);
            _stateAdapterProvider = StateAdapterProvider.Instance(world);
            _journal = new List<IEntry<TEntry>>();
            _journalReaders = new Dictionary<string, IJournalReader<TEntry>>(1);
            _streamReaders = new Dictionary<string, IStreamReader<TEntry>>(1);
            _streamIndexes = new Dictionary<string, Dictionary<int, int>>();
            _snapshots = new Dictionary<string, State<TState>>();
            _dispatchables = new List<Dispatchable<TEntry, TState>>();

            var dispatcherControlDelegate = new InMemoryDispatcherControlDelegate<TEntry, TState>(_dispatchables);
            
            _dispatcherControl = world.Stage.ActorFor<IDispatcherControl>(
                    Definition.Has<DispatcherControlActor<TEntry, TState>>(
                        Definition.Parameters(
                            dispatcher,
                            dispatcherControlDelegate,
                            checkConfirmationExpirationInterval,
                            confirmationExpiration)));
        }
        
        public override void Append<TSource, TSnapshotState>(string streamName, int streamVersion, Source<TSource> source, Metadata metadata, IAppendResultInterest interest, object @object)
        {
            var entry = _entryAdapterProvider.AsEntry<TSource, TEntry>(source, metadata);
            Insert(streamName, streamVersion, entry);
            Dispatch(streamName, streamVersion, new List<IEntry<TEntry>> { entry }, null);
            interest.AppendResultedIn(Success.Of<StorageException, Result>(Result.Success), streamName, streamVersion, source, Optional.Empty<TSnapshotState>(), @object);
        }

        public override void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, Source<TSource> source, Metadata metadata,
            TSnapshotState snapshot, IAppendResultInterest interest, object @object)
        {
            var entry = _entryAdapterProvider.AsEntry<TSource, TEntry>(source, metadata);
            Insert(streamName, streamVersion, entry);
            State<TState>? raw;
            Optional<TSnapshotState> snapshotResult;
            if (snapshot != null)
            {
                raw = _stateAdapterProvider.AsRaw<TSnapshotState, TState>(streamName, snapshot, streamVersion);
                _snapshots.Add(streamName, raw);
                snapshotResult = Optional.Of(snapshot);
            }
            else
            {
                raw = null;
                snapshotResult = Optional.Empty<TSnapshotState>();
            }

            Dispatch(streamName, streamVersion, new List<IEntry<TEntry>> { entry }, raw);
            interest.AppendResultedIn(Success.Of<StorageException, Result>(Result.Success), streamName, streamVersion, source, snapshotResult, @object);
        }

        public override void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<Source<TSource>> sources,
            Metadata metadata, IAppendResultInterest interest, object @object)
        {
            var sourcesForEntries = sources.ToList();
            var entries = _entryAdapterProvider.AsEntries<TSource, TEntry>(sourcesForEntries, metadata);
            var dispatchableEntries = entries.ToList();
            Insert(streamName, fromStreamVersion, dispatchableEntries);

            Dispatch(streamName, fromStreamVersion, dispatchableEntries, null);
            interest.AppendAllResultedIn(Success.Of<StorageException, Result>(Result.Success), streamName, fromStreamVersion, sourcesForEntries, Optional.Empty<TSnapshotState>(), @object);
        }

        public override void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<Source<TSource>> sources,
            Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object)
        {
            var sourcesForEntries = sources.ToList();
            var entries = _entryAdapterProvider.AsEntries<TSource, TEntry>(sourcesForEntries, metadata);
            var dispatchableEntries = entries.ToList();
            Insert(streamName, fromStreamVersion, dispatchableEntries);
            State<TState>? raw;
            Optional<TSnapshotState> snapshotResult;
            if (snapshot != null)
            {
                raw = _stateAdapterProvider.AsRaw<TSnapshotState, TState>(streamName, snapshot, fromStreamVersion);
                _snapshots.Add(streamName, raw);
                snapshotResult = Optional.Of(snapshot);
            }
            else
            {
                raw = null;
                snapshotResult = Optional.Empty<TSnapshotState>();
            }

            Dispatch(streamName, fromStreamVersion, dispatchableEntries, raw);
            interest.AppendAllResultedIn(Success.Of<StorageException, Result>(Result.Success), streamName, fromStreamVersion, sourcesForEntries, snapshotResult, @object);
        }

        public override ICompletes<IJournalReader<TEntry>?> JournalReader(string name)
        {
            IJournalReader<TEntry>? reader = null;
            if (!_journalReaders.ContainsKey(name))
            {
                reader = new InMemoryJournalReader<TEntry>(_journal, name);
                _journalReaders.Add(name, reader);
            }
            
            return Completes.WithSuccess(reader);
        }

        public override ICompletes<IStreamReader<TEntry>?> StreamReader(string name)
        {
            IStreamReader<TEntry>? reader = null;
            if (!_journalReaders.ContainsKey(name))
            {
                var castedDictionary = new Dictionary<string, State<TEntry>>();
                foreach (var snapshotPair in _snapshots)
                {
                    castedDictionary.Add(snapshotPair.Key, (State<TEntry>)(object)snapshotPair.Value);
                }
                reader = new InMemoryStreamReader<TEntry>(_journal.Cast<BaseEntry<TEntry>>().ToList(), _streamIndexes, castedDictionary, name);
                _streamReaders.Add(name, reader);
            }
            return Completes.WithSuccess(reader);
        }

        public void Conclude()
        {
        }

        public void Stop() => _dispatcherControl.Stop();

        public bool IsStopped { get; } = false;
        
        private void Insert(string streamName, int streamVersion, IEntry<TEntry> entry)
        {
            var entryIndex = _journal.Count;
            var id = $"{entryIndex + 1}";
            ((BaseEntry<TEntry>) entry).SetId(id); //questionable cast
            _journal.Add(entry);

            var versionIndexes = _streamIndexes.ComputeIfAbsent(streamName, k => new Dictionary<int, int>());
            versionIndexes.Add(streamVersion, entryIndex);
        }

        private void Insert(string streamName, int fromStreamVersion, IEnumerable<IEntry<TEntry>> entries)
        {
            int index = 0;
            foreach (var entry in entries)
            {
                Insert(streamName, fromStreamVersion + index, entry);
                ++index;
            }
        }

        private void Dispatch(string streamName, int streamVersion, IEnumerable<IEntry<TEntry>> entries, State<TState>? snapshot)
        {
            var dispatchableEntries = entries as IEntry<TEntry>[] ?? entries.ToArray();
            var id = GetDispatchId(streamName, streamVersion, dispatchableEntries);
            var dispatchable = new Dispatchable<TEntry, TState>(id,  DateTimeOffset.Now, snapshot, dispatchableEntries);
            _dispatchables.Add(dispatchable);
            _dispatcher.Dispatch(dispatchable);
        }

        private static string GetDispatchId(string streamName, int streamVersion, IEnumerable<IEntry<TEntry>> entries) => $"{streamName}:{streamVersion}:{string.Join(":", entries.Select(e => e.Id))}";
    }
}