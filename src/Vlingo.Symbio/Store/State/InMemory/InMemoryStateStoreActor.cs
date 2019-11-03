// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;
using Vlingo.Symbio.Store.Dispatch.Control;
using Vlingo.Symbio.Store.Dispatch.InMemory;

namespace Vlingo.Symbio.Store.State.InMemory
{
    public class InMemoryStateStoreActor<TState, TEntry> : Actor, IStateStore<TState, TEntry>
    {
        private readonly List<Dispatchable<TEntry, TState>> _dispatchables;
        private readonly IDispatcher<Dispatchable<TEntry, TState>> _dispatcher;
        private readonly IDispatcherControl _dispatcherControl;
        private readonly List<IEntry<TEntry>> _entries;
        private readonly Dictionary<string, IStateStoreEntryReader<IEntry<TEntry>>> _entryReaders;
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly StateAdapterProvider _stateAdapterProvider;
        private readonly Dictionary<string, Dictionary<string, State<TState>>> _store;

        public InMemoryStateStoreActor(IDispatcher<Dispatchable<TEntry, TState>> dispatcher, long checkConfirmationExpirationInterval = 1000L, long confirmationExpiration = 1000L)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher), "Dispatcher must not be null.");
            }
            
            _dispatcher = dispatcher;
            _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
            _stateAdapterProvider = StateAdapterProvider.Instance(Stage.World);
            _entries = new List<IEntry<TEntry>>();
            _entryReaders = new Dictionary<string, IStateStoreEntryReader<IEntry<TEntry>>>();
            _store = new Dictionary<string, Dictionary<string, State<TState>>>();
            _dispatchables = new List<Dispatchable<TEntry, TState>>();

            var dispatcherControlDelegate = new InMemoryDispatcherControlDelegate<TEntry, TState>(_dispatchables);

            _dispatcherControl = Stage.ActorFor<IDispatcherControl>(
                Definition.Has<DispatcherControlActor<TEntry, TState>>(
                    Definition.Parameters(
                        dispatcher,
                        dispatcherControlDelegate,
                        checkConfirmationExpirationInterval,
                        confirmationExpiration)));
        }

        public void Read(string id, IReadResultInterest interest) => Read(id, interest, null);

        public void Read(string id, IReadResultInterest interest, object? @object) => ReadFor(id, interest, @object);

        public void Write(string id, TState state, int stateVersion, IWriteResultInterest interest)
        {
            throw new System.NotImplementedException();
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, IWriteResultInterest interest)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest)
        {
            throw new System.NotImplementedException();
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, Metadata metadata,
            IWriteResultInterest interest)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string id, TState state, int stateVersion, IWriteResultInterest interest, object @object)
        {
            throw new System.NotImplementedException();
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, IWriteResultInterest interest, object @object)
        {
            throw new System.NotImplementedException();
        }

        public void Write(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest, object @object)
        {
            throw new System.NotImplementedException();
        }

        public void Write<TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, Metadata metadata, IWriteResultInterest interest, object @object)
        {
            throw new System.NotImplementedException();
        }

        public ICompletes<IStateStoreEntryReader<IEntry<TEntry>>> EntryReader(string name)
        {
            var reader = _entryReaders[name];
            if (reader == null)
            {
                reader = ChildActorFor<IStateStoreEntryReader<IEntry<TEntry>>>(Definition.Has<InMemoryStateStoreEntryReaderActor<TEntry>>(Definition.Parameters(_entries, name)));
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
        
        private void ReadFor(string id, IReadResultInterest interest, object? @object)
        {
            if (interest != null)
            {
                if (id == null)
                {
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.Error, "The id is null.")), null, default!, -1, null, @object);
                    return;
                }

                var storeName = StateTypeStateStoreMap.StoreNameFrom(typeof(TState).FullName);

                if (storeName == null)
                {
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.NoTypeStore,
                        $"No type store for: {typeof(TState).FullName}")), id, default!, -1, null, @object);
                    return;
                }

                var typeStore = _store[storeName];

                if (typeStore == null)
                {
                    interest.ReadResultedIn<TState>(Failure.Of<StorageException, Result>(new StorageException(Result.NotFound,
                        $"Store not found: {storeName}")), id, default!, -1, null, @object);
                    return;
                }

                var raw = typeStore[id];

                if (raw != null)
                {
                    var state = _stateAdapterProvider.FromRaw<string, TState>(raw);
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
                Logger.Warn($"{GetType().FullName} readText() missing ReadResultInterest for: {id ?? "unknown id"}");
            }
        }

        private void WriteWith<TSource>(string id, TState state, int stateVersion, List<Source<TSource>> sources,
            Metadata metadata, IWriteResultInterest interest, object @object)
        {
            if (interest != null)
            {
                if (state == null)
                {
                    interest.WriteResultedIn(
                        Failure.Of<StorageException, Result>(new StorageException(Result.Error, "The state is null.")),
                        id, state, stateVersion, sources, @object);
                }
                else
                {
                    try
                    {
                        var storeName = StateTypeStateStoreMap.StoreNameFrom(typeof(TState));

                        if (storeName == null)
                        {
                            interest.WriteResultedIn(Failure.Of<StorageException, Result>(new StorageException(
                                Result.NoTypeStore,
                                $"No type store for: {state.GetType()}")), id, state, stateVersion, sources, @object);
                            return;
                        }

                        var typeStore = _store[storeName];

                        if (typeStore == null)
                        {
                            typeStore = new Dictionary<string, State<TState>>();
                            if (_store.TryGetValue(storeName, out var existingTypeStore))
                            {
                                typeStore = existingTypeStore;
                            }
                            else
                            {
                                _store.Add(storeName, typeStore);
                            }
                        }

                        var raw = metadata == null
                            ? _stateAdapterProvider.AsRaw<TState, TSource>(id, state, stateVersion)
                            : _stateAdapterProvider.AsRaw<TState, TSource>(id, state, stateVersion, metadata);

                        if (!typeStore.TryGetValue(raw.Id, out var persistedState))
                        {
                            //typeStore.Add(raw.Id, raw);
                        }
                        
                        if (persistedState != null)
                        {
                            if (persistedState.DataVersion >= raw.DataVersion)
                            {
                                interest.WriteResultedIn(Failure.Of<StorageException, Result>(new StorageException(Result.ConcurrencyViolation, "Version conflict.")), id, state, stateVersion, sources, @object);
                                return;
                            }
                        }   
//                        typeStore.put(id, raw);
//                        final List<Entry<?>> entries = appendEntries(sources, metadata);
//                        dispatch(id, storeName, raw, entries);
//
//                        interest.writeResultedIn(Success.of(Result.Success), id, state, stateVersion, sources, object);
                    }
                    catch (Exception e)
                    {
//                      logger().error(getClass().getSimpleName() + " writeText() error because: " + e.getMessage(), e);
//                      interest.writeResultedIn(Failure.of(new StorageException(Result.Error, e.getMessage(), e)), id, state, stateVersion, sources, object);
                    }
                    

//                } else {
//                  logger().warn(
//                          getClass().getSimpleName() +
//                          " writeText() missing WriteResultInterest for: " +
//                          (state == null ? "unknown id" : id));
                }
            }
        }
    }
}