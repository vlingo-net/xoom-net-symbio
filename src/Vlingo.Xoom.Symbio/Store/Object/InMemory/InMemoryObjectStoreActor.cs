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
using Vlingo.Xoom.Symbio.Store.Dispatch;
using Vlingo.Xoom.Symbio.Store.Dispatch.Control;
using IDispatcher = Vlingo.Xoom.Symbio.Store.Dispatch.IDispatcher;

namespace Vlingo.Xoom.Symbio.Store.Object.InMemory
{
    /// <summary>
    /// In-memory implementation of <see cref="IObjectStore"/>. Note that <code>QueryAll()</code> variations
    /// do not support select constraints but always select all stored objects.
    /// </summary>
    public class InMemoryObjectStoreActor<T> : Actor, IObjectStore
    {
        private readonly EntryAdapterProvider _entryAdapterProvider;

        private readonly IDispatcher _dispatcher;
        private readonly IDispatcherControl _dispatcherControl;
        private readonly IReadOnlyDictionary<string, IObjectStoreEntryReader> _entryReaders;
        private readonly IObjectStoreDelegate _storeDelegate;

        /// <summary>
        /// Construct my default state.
        /// </summary>
        /// <param name="dispatcher">The dispatcher to be used</param>
        public InMemoryObjectStoreActor(IDispatcher dispatcher) : this(dispatcher, 1000L, 1000L)
        {
        }

        public InMemoryObjectStoreActor(IDispatcher dispatcher, long checkConfirmationExpirationInterval, long confirmationExpiration)
        {
            _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
            _dispatcher = dispatcher;

            _entryReaders = new Dictionary<string, IObjectStoreEntryReader>();

            _storeDelegate = new InMemoryObjectStoreDelegate(StateAdapterProvider.Instance(Stage.World));

            _dispatcherControl = Stage.ActorFor<IDispatcherControl>(
                () => new DispatcherControlActor(dispatcher, _storeDelegate,
                    checkConfirmationExpirationInterval, confirmationExpiration));
        }

        public void Close() => _storeDelegate.Close();

        public bool IsNoId(long id) => NoId == id;

        public bool IsId(long id) => id > NoId;

        public ICompletes<IEntryReader> EntryReader<TNewEntry>(string name) where TNewEntry : IEntry
        {
            if (!_entryReaders.TryGetValue(name, out var reader))
            {
                var definition = Definition.Has<InMemoryObjectStoreEntryReaderActor>(Definition.Parameters(ReadOnlyJournal(), name));
                reader = (IObjectStoreEntryReader)ChildActorFor<IObjectStoreEntryReader>(definition);
            }
            
            return Completes().With((IEntryReader)reader);
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
        
        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, IPersistResultInterest interest) where TNewState : StateObject where TSource : ISource
            => Persist(stateSources, Metadata.NullMetadata(), -1, interest, null);

        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, Metadata metadata, IPersistResultInterest interest) where TNewState : StateObject where TSource : ISource
            => Persist(stateSources, metadata, -1, interest, null);
       
        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
            => Persist(stateSources, Metadata.NullMetadata(), -1, interest, @object);

        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, Metadata metadata, IPersistResultInterest interest, object? @object)
            where TNewState : StateObject where TSource : ISource => Persist(stateSources, metadata, -1, interest, @object);
        
        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, long updateId, IPersistResultInterest interest)
            where TNewState : StateObject where TSource : ISource => Persist(stateSources, Metadata.NullMetadata(), updateId, interest, null);

        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, Metadata metadata, long updateId, IPersistResultInterest interest)
            where TNewState : StateObject where TSource : ISource => Persist(stateSources, metadata, updateId, interest, null);
        
        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
            => Persist(stateSources, Metadata.NullMetadata(), updateId, interest, @object);

        public void Persist<TNewState, TSource>(StateSources<TNewState, TSource> stateSources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
        {
            try
            {
                var stateObject = stateSources.StateObject;
                var sources = stateSources.Sources;
                var raw = _storeDelegate.Persist(stateObject, updateId, metadata);

                var entryVersion = (int) stateSources.StateObject.Version;
                var entries = _entryAdapterProvider.AsEntries(sources, entryVersion, metadata).ToList();
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
        
        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, IPersistResultInterest interest) where TNewState : StateObject where TSource : ISource =>
            PersistAll(allStateSources, Metadata.NullMetadata(), -1, interest, null);

        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, Metadata metadata, IPersistResultInterest interest) where TNewState : StateObject where TSource : ISource
            => PersistAll(allStateSources, metadata, -1, interest, null);
        
        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
            => PersistAll(allStateSources, Metadata.NullMetadata(), -1, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, Metadata metadata, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
            => PersistAll(allStateSources, metadata, -1, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, long updateId, IPersistResultInterest interest) where TNewState : StateObject where TSource : ISource
            => PersistAll(allStateSources, Metadata.NullMetadata(), updateId, interest, null);

        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, Metadata metadata, long updateId, IPersistResultInterest interest) where TNewState : StateObject where TSource : ISource
            => PersistAll(allStateSources, metadata, updateId, interest, null);
        
        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
            => PersistAll(allStateSources, Metadata.NullMetadata(), updateId, interest, @object);

        public void PersistAll<TNewState, TSource>(IEnumerable<StateSources<TNewState, TSource>> allStateSources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TNewState : StateObject where TSource : ISource
        {
            var allPersistentObjects = new List<TNewState>();
            try
            {
                foreach (var stateSources in allStateSources)
                {
                    var stateObject = stateSources.StateObject;
                    var state = _storeDelegate.Persist(stateObject, updateId, metadata);
                    allPersistentObjects.Add(stateObject);

                    var entryVersion = (int) stateSources.StateObject.Version;
                    var entries = _entryAdapterProvider.AsEntries(stateSources.Sources, entryVersion, metadata).ToList();
                    _storeDelegate.PersistEntries(entries);

                    var dispatchable = BuildDispatchable(state, entries);
                    _storeDelegate.PersistDispatchable(dispatchable);

                    Dispatch(BuildDispatchable(state, entries));
                }
                
                interest.PersistResultedIn(Success.Of<StorageException, Result>(Result.Success), allPersistentObjects, allPersistentObjects.Count, allPersistentObjects.Count, @object);
            }
            catch (StorageException e)
            {
                Logger.Error("Failed to persist all objects", e);
                interest.PersistResultedIn(Failure.Of<StorageException, Result>(e), null, 0, 0, @object);
            }
        }

        public override void Stop()
        {
            _dispatcherControl.Stop();
            base.Stop();
        }

        public long NoId { get; } = -1L;
        
        private void Dispatch(Dispatchable dispatchable) => _dispatcher.Dispatch(dispatchable);

        private static Dispatchable BuildDispatchable(IState state, IList<IEntry> entries)
        {
            var id = GetDispatchId(state, entries);
            return new Dispatchable(id, DateTimeOffset.Now, state, entries);
        }

        private static string GetDispatchId(IState raw, IEnumerable<IEntry> entries) =>
            $"{raw.Id}:{string.Join(":", entries.Select(entry => entry.Id))}";
        
        private List<IEntry> ReadOnlyJournal() => ((InMemoryObjectStoreDelegate) _storeDelegate).ReadOnlyJournal();
    }
}