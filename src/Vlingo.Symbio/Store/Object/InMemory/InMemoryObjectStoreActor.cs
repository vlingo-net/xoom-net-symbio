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

namespace Vlingo.Symbio.Store.Object.InMemory
{
    /// <summary>
    /// In-memory implementation of <see cref="IObjectStore"/>. Note that <code>QueryAll()</code> variations
    /// do not support select constraints but always select all stored objects.
    /// </summary>
    public class InMemoryObjectStoreActor<TEntry, TState> : Actor, IObjectStore
    {
        private EntryAdapterProvider _entryAdapterProvider;

        private readonly IDispatcher<Dispatchable<TEntry, TState>> _dispatcher;
        private IDispatcherControl _dispatcherControl;
        private readonly Dictionary<string, IObjectStoreEntryReader<TEntry>> _entryReaders;
        private readonly IObjectStoreDelegate<TEntry, TState> _storeDelegate;

        /// <summary>
        /// Construct my default state.
        /// </summary>
        /// <param name="dispatcher">The dispatcher to be used</param>
        public InMemoryObjectStoreActor(IDispatcher<Dispatchable<TEntry, TState>> dispatcher) : this(dispatcher, 1000L, 1000L)
        {
        }

        public InMemoryObjectStoreActor(IDispatcher<Dispatchable<TEntry, TState>> dispatcher, long checkConfirmationExpirationInterval, long confirmationExpiration)
        {
            _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
            _dispatcher = dispatcher;

            _entryReaders = new Dictionary<string, IObjectStoreEntryReader<TEntry>>();

            _storeDelegate = new InMemoryObjectStoreDelegate<TEntry, TState>(StateAdapterProvider.Instance(Stage.World));

            _dispatcherControl = Stage.ActorFor<IDispatcherControl>(
                Definition.Has<Dispatch.Control.DispatcherControlActor<TEntry, TState>>(
                    Definition.Parameters(dispatcher, _storeDelegate, checkConfirmationExpirationInterval, confirmationExpiration)));
        }

        public void Close() => _storeDelegate.Close();

        public bool IsNoId(long id) => NoId == id;

        public bool IsId(long id) => id > NoId;

        public ICompletes<IEntryReader<T>> EntryReader<T>(string name)
        {
            if (!_entryReaders.TryGetValue(name, out var reader))
            {
                var definition = Definition.Has<InMemoryObjectStoreEntryReaderActor>(Definition.Parameters(ReadOnlyJournal(), name));
                reader = ChildActorFor<IObjectStoreEntryReader<TEntry>>(definition);
            }
            
            return Completes().With((IEntryReader<T>)reader);
        }

        public void QueryAll(QueryExpression expression, IQueryResultInterest interest) =>
            QueryAll(expression, interest, null);

        public void QueryAll(QueryExpression expression, IQueryResultInterest interest, object? @object)
        {
            var queryMultiResults = _storeDelegate.QueryAll(expression);
            interest.QueryAllResultedIn(Success.Of<StorageException, Result>(Result.Success), queryMultiResults, @object);
        }

        public void QueryObject(QueryExpression expression, IQueryResultInterest interest) =>
            QueryObject(expression, interest, null);

        public void QueryObject(QueryExpression expression, IQueryResultInterest interest, object? @object)
        {
            var result = _storeDelegate.QueryObject(expression);

            if (result.StateObject != null)
            {
                interest.QueryObjectResultedIn(Success.Of<StorageException, Result>(Result.Success), result, @object);
            }
            else
            {
                interest.QueryObjectResultedIn(Failure.Of<StorageException, Result>(new StorageException(Result.NotFound, "No object identified by expression: " + expression)), QuerySingleResult.Of(null), @object);
            }
        }

        public void Persist<TNewState>(TNewState stateObject, IPersistResultInterest interest)
            where TNewState : StateObject => Persist(stateObject, Source<TState>.None(), Metadata.NullMetadata(), -1, interest, null);

        public void Persist<TNewState>(TNewState stateObject, Metadata metadata, IPersistResultInterest interest)
            where TNewState : StateObject => Persist(stateObject, Source<TState>.None(), metadata, -1, interest, null);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, IPersistResultInterest interest) where TNewState : StateObject
            => Persist(stateObject, sources, Metadata.NullMetadata(), -1, interest, null);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, Metadata metadata, IPersistResultInterest interest) where TNewState : StateObject
            => Persist(stateObject, sources, metadata, -1, interest, null);

        public void Persist<TNewState>(TNewState stateObject, IPersistResultInterest interest, object? @object)
            where TNewState : StateObject => Persist(stateObject, Source<TState>.None(), Metadata.NullMetadata(), -1, interest, @object);

        public void Persist<TNewState>(TNewState stateObject, Metadata metadata, IPersistResultInterest interest, object? @object)
            where TNewState : StateObject => Persist(stateObject, Source<TState>.None(), metadata, -1, interest, @object);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => Persist(stateObject, sources, Metadata.NullMetadata(), -1, interest, @object);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, Metadata metadata, IPersistResultInterest interest, object? @object)
            where TNewState : StateObject => Persist(stateObject, sources, metadata, -1, interest, @object);

        public void Persist<TNewState>(TNewState stateObject, long updateId, IPersistResultInterest interest)
            where TNewState : StateObject => Persist(stateObject, Source<TState>.None(), Metadata.NullMetadata(), updateId, interest, null);

        public void Persist<TNewState>(TNewState stateObject, Metadata metadata, long updateId, IPersistResultInterest interest)
            where TNewState : StateObject => Persist(stateObject,Source<TState>.None(), metadata, updateId, interest, null);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, long updateId, IPersistResultInterest interest)
            where TNewState : StateObject => Persist(stateObject, sources, Metadata.NullMetadata(), updateId, interest, null);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, Metadata metadata, long updateId, IPersistResultInterest interest)
            where TNewState : StateObject => Persist(stateObject, sources, metadata, updateId, interest, null);

        public void Persist<TNewState>(TNewState stateObject, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => Persist(stateObject, Source<TState>.None(), Metadata.NullMetadata(), updateId, interest, @object);

        public void Persist<TNewState>(TNewState stateObject, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => Persist(stateObject, Source<TState>.None(), metadata, updateId, interest, @object);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => Persist(stateObject, sources, Metadata.NullMetadata(), updateId, interest, @object);

        public void Persist<TNewState, TSource>(TNewState stateObject, IEnumerable<Source<TSource>> sources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
        {
            try
            {
                var raw = _storeDelegate.Persist(stateObject, updateId, metadata);

                var entries = _entryAdapterProvider.AsEntries<TSource, TEntry>(sources, metadata).ToList();
                var dispatchable = BuildDispatchable(raw, entries);

                _storeDelegate.PersistEntries(entries);
                _storeDelegate.PersistDispatchable(dispatchable);

                Dispatch(dispatchable);
                interest.PersistResultedIn(Success.Of<StorageException, Result>(Result.Success), stateObject, 1, 1, @object);
            } 
            catch (StorageException e)
            {
                Logger.Error("Failed to persist all objects", e);
                interest.PersistResultedIn(Failure.Of<StorageException, Result>(e), null, 0, 0, @object);
            }
        }

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, IPersistResultInterest interest) where TNewState : StateObject 
            => PersistAll(stateObjects, Source<TState>.None(), Metadata.NullMetadata(), -1, interest, null);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, Metadata metadata, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), metadata, -1, interest, null);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, sources, Metadata.NullMetadata(), -1, interest, null);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, Metadata metadata, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, sources, metadata, -1, interest, null);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), Metadata.NullMetadata(), -1, interest, @object);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, Metadata metadata, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), metadata, -1, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, sources, Metadata.NullMetadata(), -1, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, Metadata metadata, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, sources, metadata, -1, interest, @object);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, long updateId, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), Metadata.NullMetadata(), updateId, interest, null);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, Metadata metadata, long updateId, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), metadata, updateId, interest, null);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, long updateId, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, sources, Metadata.NullMetadata(), updateId, interest, null);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, Metadata metadata, long updateId, IPersistResultInterest interest) where TNewState : StateObject
            => PersistAll(stateObjects, sources, metadata, updateId, interest, null);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), Metadata.NullMetadata(), updateId, interest, @object);

        public void PersistAll<TNewState>(IEnumerable<TNewState> stateObjects, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, Source<TState>.None(), metadata, updateId, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
            => PersistAll(stateObjects, sources, Metadata.NullMetadata(), updateId, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<TNewState> stateObjects, IEnumerable<Source<TSource>> sources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject
        {
            try
            {
                var stateObjectsToPersist = stateObjects as TNewState[] ?? stateObjects.ToArray();
                var states = _storeDelegate.PersistAll(stateObjectsToPersist, updateId, metadata);

                var entries = _entryAdapterProvider.AsEntries<TSource, TEntry>(sources, metadata).ToList();

                _storeDelegate.PersistEntries(entries);

                foreach (var state in states)
                {
                    var dispatchable = BuildDispatchable(state, entries);
                    _storeDelegate.PersistDispatchable(dispatchable);
                    //dispatch each persistent object
                    Dispatch(BuildDispatchable(state, entries));
                }

                interest.PersistResultedIn(Success.Of<StorageException, Result>(Result.Success), stateObjects, stateObjectsToPersist.Length, stateObjectsToPersist.Length, @object);
            }
            catch (StorageException e)
            {
                Logger.Error("Failed to persist all objects", e);
                interest.PersistResultedIn(Failure.Of<StorageException, Result>(e), null, 0, 0, @object);
            }
        }

        public long NoId { get; } = -1L;
        
        private void Dispatch(Dispatchable<TEntry, TState> dispatchable) => _dispatcher.Dispatch(dispatchable);

        private static Dispatchable<TEntry, TState> BuildDispatchable(State<TState> state, List<IEntry<TEntry>> entries)
        {
            var id = GetDispatchId(state, entries);
            return new Dispatchable<TEntry, TState>(id, DateTimeOffset.Now, state, entries);
        }

        private static string GetDispatchId(State<TState> raw, IEnumerable<IEntry<TEntry>> entries) =>
            $"{raw.Id}:{string.Join(":", entries.Select(entry => entry.Id))}";
        
        private List<IEntry<TEntry>> ReadOnlyJournal()
        {
            return ((InMemoryObjectStoreDelegate<TEntry, TState>) _storeDelegate).ReadOnlyJournal();
        }
    }
}