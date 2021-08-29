// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Streams;
using Vlingo.Xoom.Symbio.Store.Dispatch;
using Vlingo.Xoom.Symbio.Store.Dispatch.Control;
using Vlingo.Xoom.Symbio.Store.Dispatch.InMemory;
using IDispatcher = Vlingo.Xoom.Symbio.Store.Dispatch.IDispatcher;

namespace Vlingo.Xoom.Symbio.Store.State.InMemory
{
    public class InMemoryStateStoreActor<TRawState> : Actor, IStateStore where TRawState : class, IState
    {
        private readonly List<Dispatchable> _dispatchables;
        private readonly List<IDispatcher> _dispatchers;
        private readonly IDispatcherControl _dispatcherControl;
        // this is based on mock database design, it represents a database entries and it's shared (like database)
        // between this and InMemoryStateStoreEntryReaderActor
        private readonly List<IEntry> _entries;
        private readonly Dictionary<string, IStateStoreEntryReader> _entryReaders;
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly StateAdapterProvider _stateAdapterProvider;
        private readonly ReadAllResultCollector _readAllResultCollector;
        private readonly Dictionary<string, Dictionary<string, TRawState>> _store;

        public InMemoryStateStoreActor(IDispatcher dispatcher) : this(new []{dispatcher}, 1000L, 1000L)
        {
        }

        public InMemoryStateStoreActor(IDispatcher dispatcher, long checkConfirmationExpirationInterval, long confirmationExpiration)
        : this (new []{dispatcher}, checkConfirmationExpirationInterval, confirmationExpiration)
        {
        }

        public InMemoryStateStoreActor(IEnumerable<IDispatcher> dispatchers)
        : this (dispatchers, 1000L, 1000L)
        {
        }

        public InMemoryStateStoreActor(IEnumerable<IDispatcher> dispatchers, long checkConfirmationExpirationInterval, long confirmationExpiration)
        {
            if (dispatchers == null)
            {
                throw new ArgumentNullException(nameof(dispatchers), "Dispatcher must not be null.");
            }
            
            _dispatchers = dispatchers.ToList();
            _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
            _stateAdapterProvider = StateAdapterProvider.Instance(Stage.World);
            _entries = new List<IEntry>();
            _entryReaders = new Dictionary<string, IStateStoreEntryReader>();
            _store = new Dictionary<string, Dictionary<string, TRawState>>();
            _dispatchables = new List<Dispatchable>();
            _readAllResultCollector = new ReadAllResultCollector();

            var dispatcherControlDelegate = new InMemoryDispatcherControlDelegate(_dispatchables);

            _dispatcherControl = Stage.ActorFor<IDispatcherControl>(
                () => new DispatcherControlActor(
                    _dispatchers,
                    dispatcherControlDelegate,
                    checkConfirmationExpirationInterval,
                    confirmationExpiration));
        }

        public void Read<TState>(string id, IReadResultInterest interest) => Read<TState>(id, interest, null);

        public void Read<TState>(string id, IReadResultInterest interest, object? @object) => ReadFor<TState>(id, interest, @object);
        
        public void ReadAll<TState>(IEnumerable<TypedStateBundle> bundles, IReadResultInterest interest, object? @object)
        {
            _readAllResultCollector.Prepare();

            var typedStateBundles = bundles.ToList();
            foreach (var bundle in typedStateBundles)
            {
                ReadFor<TState>(bundle.Id!, _readAllResultCollector, null);
            }

            var outcome = _readAllResultCollector.ReadResultOutcome(typedStateBundles.Count);
            interest.ReadResultedIn<TState>(outcome!, _readAllResultCollector.ReadBundles, @object);
        }

        public ICompletes<IStream> StreamAllOf<TState>()
        {
            var storeName = StateTypeStateStoreMap.StoreNameFrom(typeof(TState));

            if (string.IsNullOrEmpty(storeName))
            {
                throw new ArgumentNullException(nameof(storeName), $"Cannot retrieve the store for name: {storeName}");
            }
            
            if (!_store.TryGetValue(storeName!, out var typeStore))
            {
                typeStore = new Dictionary<string, TRawState>();
            }
            
            return Completes().With((IStream) new StateStream<TRawState>(Stage, typeStore, _stateAdapterProvider));
        }

        public ICompletes<IStream> StreamSomeUsing(QueryExpression query)
        {
            // TODO Auto-generated method stub
            return null!;
        }

        public void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest) =>
            Write(id, state, stateVersion, Source<TState>.None(), Metadata.NullMetadata(), interest, null);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, IWriteResultInterest interest) =>
            Write(id, state, stateVersion, sources, Metadata.NullMetadata(), interest, null);

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest) => 
            Write(id, state, stateVersion, Source<TState>.None(), metadata, interest, null);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata, IWriteResultInterest interest) =>
            Write(id, state, stateVersion, sources, metadata, interest, null);

        public void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest, object? @object) =>
            Write(id, state, stateVersion, Source<TState>.None(), Metadata.NullMetadata(), interest, @object);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, IWriteResultInterest interest, object? @object) =>
            Write(id, state, stateVersion, sources, Metadata.NullMetadata(), interest, @object);

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest, object? @object) =>
            Write(id, state, stateVersion, Source<TState>.None(), metadata, interest, @object);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata, IWriteResultInterest interest, object? @object) =>
            WriteWith(id, state, stateVersion, sources, metadata, interest, @object);

        public ICompletes<IStateStoreEntryReader> EntryReader<TEntryReader>(string name) where TEntryReader : IEntry
        {
            if (!_entryReaders.TryGetValue(name, out var reader))
            {
                reader = ChildActorFor<IStateStoreEntryReader>(
                    () => new InMemoryStateStoreEntryReaderActor(_entries, name));
                _entryReaders.Add(name, reader);
            }
            
            return Completes().With(reader);
        }

        public override void Stop()
        {
            if (_dispatcherControl != null)
            {
                _dispatcherControl.Stop();
            }
            
            base.Stop();
        }
        
        private void ReadFor<TState>(string id, IReadResultInterest interest, object? @object)
        {
            if (interest != null)
            {
                if (id == null)
                {
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.Error, "The id is null.")), null, default!, -1, null, @object);
                    return;
                }

                var storeName = StateTypeStateStoreMap.StoreNameFrom(typeof(TState).FullName!);

                if (storeName == null)
                {
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.NoTypeStore,
                        $"No type store for: {typeof(TState).FullName}")), id, default!, -1, null, @object);
                    return;
                }

                if (!_store.TryGetValue(storeName, out var typeStore))
                {
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.NotFound,
                        $"Store not found: {storeName}")), id, default!, -1, null, @object);
                    return;
                }

                var raw = typeStore[id];

                if (raw != null)
                {
                    var state = _stateAdapterProvider.FromRaw<TState, TRawState>(raw);
                    interest.ReadResultedIn(Success.Of<StorageException, Result>(Result.Success), id, state, raw.DataVersion, raw.Metadata, @object);
                }
                else
                {
                    foreach (var storeId in typeStore.Keys)
                    {
                        Logger.Debug("UNFOUND STATES\n=====================");
                        Logger.Debug($"STORE ID: '{storeId}' STATE: {typeStore[storeId]}");
                    }
                    
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.NotFound, "Not found.")), id, default!, -1, null, @object);
                }
            }
            else
            {
                Logger.Warn($"{GetType().FullName} ReadFor() missing ReadResultInterest for: {id ?? "unknown id"}");
            }
        }

        private void WriteWith<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata, IWriteResultInterest interest, object? @object)
        {
            if (interest != null)
            {
                if (state == null)
                {
                    interest.WriteResultedIn(Failure.Of<StorageException, Result>(new StorageException(Result.Error, "The state is null.")), id, state, stateVersion, sources, @object);
                }
                else
                {
                    try
                    {
                        var storeName = StateTypeStateStoreMap.StoreNameFrom(typeof(TState));

                        if (storeName == null)
                        {
                            interest.WriteResultedIn(Failure.Of<StorageException, Result>(new StorageException(Result.NoTypeStore, $"No type store for: {state.GetType()}")), id, state, stateVersion, sources, @object);
                            return;
                        }

                        if (!_store.TryGetValue(storeName, out var typeStore))
                        {
                            typeStore = new Dictionary<string, TRawState>();
                            var existingTypeStore = _store.AddIfAbsent(storeName, typeStore);
                            if (existingTypeStore != null)
                            {
                                typeStore = existingTypeStore;
                            }
                        }

                        var raw = metadata == null
                            ? _stateAdapterProvider.AsRaw<TState, TRawState>(id, state, stateVersion)
                            : _stateAdapterProvider.AsRaw<TState, TRawState>(id, state, stateVersion, metadata);

                        var persistedState = typeStore.AddIfAbsent(raw.Id, raw);
                        if (persistedState != null)
                        {

                            if (persistedState.DataVersion >= raw.DataVersion)
                            {
                                interest.WriteResultedIn(Failure.Of<StorageException, Result>(new StorageException(Result.ConcurrencyViolation, "Version conflict.")), id, state, stateVersion, sources, @object);
                                return;
                            }
                        }

                        typeStore[id] = raw; // maybe useless as it was added by the line above var persistedState = typeStore.AddIfAbsent(raw.Id, raw);
                        var entries = AppendEntries(sources.Cast<ISource>(), stateVersion, metadata);
                        Dispatch(id, storeName, raw, entries);

                        interest.WriteResultedIn(Success.Of<StorageException, Result>(Result.Success), id, state, stateVersion, sources, @object);
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"{GetType().FullName} WriteWith() error because: {e.Message}", e);
                        interest.WriteResultedIn(Failure.Of<StorageException, Result>(new StorageException(Result.Error, e.Message, e)), id, state, stateVersion, sources, @object);
                    }
                }
            }
            else
            {
              Logger.Warn($"{GetType().FullName} WriteWith() missing WriteResultInterest for: {(state == null ? "unknown id" : id)}");
            }
        }

        private IEnumerable<IEntry> AppendEntries(IEnumerable<ISource> sources, int stateVersion, Metadata? metadata)
        {
            var adapted = _entryAdapterProvider.AsEntries(sources, stateVersion, metadata);
            var appendEntries = adapted.ToList();
            foreach (var entry in appendEntries)
            {
                if (entry is BaseEntry baseEntry)
                {
                    baseEntry.SetId(_entries.Count.ToString());   
                }
                _entries.Add(entry);
            }

            return appendEntries;
        }
        
        private void Dispatch(string id, string storeName, TRawState raw, IEnumerable<IEntry> entries)
        {
            var dispatchId = $"{storeName}:{id}";
            var dispatchable = new Dispatchable(dispatchId, DateTimeOffset.Now, raw, entries);
            _dispatchables.Add(dispatchable);
            _dispatchers.ForEach(d => d.Dispatch(dispatchable));
        }
    }
}