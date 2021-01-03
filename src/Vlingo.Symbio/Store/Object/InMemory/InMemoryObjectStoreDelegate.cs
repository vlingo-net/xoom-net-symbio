// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Common;
using Vlingo.Common.Identity;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Store.Object.InMemory
{
    public class InMemoryObjectStoreDelegate<TEntry, TState> : IObjectStoreDelegate<TEntry, TState> where TEntry : IEntry where TState : class, IState
    {
        private long _nextId;

        private Dictionary<Type , Dictionary<long, TState>> _stores;
        private List<TEntry> _entries;
        private List<Dispatchable<TEntry, TState>> _dispatchables;
        private StateAdapterProvider _stateAdapterProvider;
        private IIdentityGenerator _identityGenerator;

        public InMemoryObjectStoreDelegate(StateAdapterProvider stateAdapterProvider)
        {
            _stateAdapterProvider = stateAdapterProvider;
            _stores = new Dictionary<Type, Dictionary<long, TState>>();
            _entries = new List<TEntry>();
            _dispatchables = new List<Dispatchable<TEntry, TState>>();
            _identityGenerator = IdentityGeneratorType.Random.Generator();

            _nextId = 1;
        }
        
        public void ConfirmDispatched(string dispatchId)
        {
            var toRemove = _dispatchables.FirstOrDefault(d => d.Id.Equals(dispatchId));
            _dispatchables.Remove(toRemove);
        }

        /// <inheritdoc />
        public void Stop() => Close();

        /// <inheritdoc />
        public void RegisterMapper(StateObjectMapper mapper)
        {
            //InMemory store does not require mappers
        }

        /// <inheritdoc />
        public void Close()
        {
            _stores.Clear();
            _entries.Clear();
            _dispatchables.Clear();
        }

        /// <inheritdoc />
        public IObjectStoreDelegate<TEntry, TState> Copy() => new InMemoryObjectStoreDelegate<TEntry, TState>(_stateAdapterProvider);

        /// <inheritdoc />
        public void BeginTransaction()
        {
            //InMemory store does not require transactions
        }

        /// <inheritdoc />
        public void CompleteTransaction()
        {
            //InMemory store does not require transactions
        }

        /// <inheritdoc />
        public void FailTransaction()
        {
            //InMemory store does not require transactions
        }

        /// <inheritdoc />
        public IEnumerable<TState> PersistAll(IEnumerable<StateObject> stateObjects, long updateId, Metadata metadata)
        {
            var states = stateObjects as StateObject[] ?? stateObjects.ToArray();
            var persistedStates = new List<TState>(states.Length);
            foreach (var stateObject in states)
            {
                var raw = Persist(stateObject, metadata);
                persistedStates.Add(raw);
            }

            return persistedStates;
        }

        /// <inheritdoc />
        public TState Persist(StateObject stateObject, long updateId, Metadata metadata) =>
            Persist(stateObject, metadata);

        /// <inheritdoc />
        public void PersistEntries(IEnumerable<TEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (entry is BaseEntry baseEntry)
                {
                    baseEntry.SetId(_identityGenerator.Generate().ToString());
                    _entries.Add(entry);   
                }
            }
        }

        /// <inheritdoc />
        public void PersistDispatchable(Dispatchable<TEntry, TState> dispatchable) => _dispatchables.Add(dispatchable);

        /// <inheritdoc />
        public QueryMultiResults QueryAll(QueryExpression expression)
        {
            // NOTE: No query constraints accepted; selects all stored objects

            var all = new List<StateObject>();
            var store = _stores.ComputeIfAbsent(expression.Type, type => new Dictionary<long, TState>());
            foreach (var entry in store.Values)
            {
                var stateObject = _stateAdapterProvider.FromRaw<StateObject, TState>(entry);
                all.Add(stateObject);
            }

            return new QueryMultiResults(all);
        }

        /// <inheritdoc />
        public QuerySingleResult QueryObject(QueryExpression expression)
        {
            string? id;
            if (expression.IsListQueryExpression)
            {
                id = IdParameterAsString(expression.AsListQueryExpression().Parameters.First());
            }
            else if (expression.IsMapQueryExpression)
            {
                id = IdParameterAsString(expression.AsMapQueryExpression().Parameters["id"]);
            }
            else
            {
                throw new StorageException(Result.Error, $"Unknown query type: {expression}");
            }

            var store = _stores.ComputeIfAbsent(expression.Type, type => new Dictionary<long, TState>());
            var found = id == null || id.Equals("-1") ? null! : store[long.Parse(id)];

            var result = Optional
                .OfNullable(found)
                .Map(state => _stateAdapterProvider.FromRaw<StateObject, TState>(state))
                .OrElse(null!);
            return new QuerySingleResult(result);
        }

        /// <inheritdoc />
        public IEnumerable<Dispatchable<TEntry, TState>> AllUnconfirmedDispatchableStates => _dispatchables;
        
        /// <inheritdoc />
        public IEnumerable<StateObjectMapper> RegisteredMappers { get; } = Enumerable.Empty<StateObjectMapper>();
        
        internal List<TEntry> ReadOnlyJournal()
        {
            return _entries;
        }
        
        private TState Persist(StateObject stateObject, Metadata metadata)
        {
            var raw = _stateAdapterProvider.AsRaw<StateObject, TState>(stateObject.PersistenceId.ToString(), stateObject, 1, metadata);
            var store = _stores.ComputeIfAbsent(stateObject.GetType(), type => new Dictionary<long, TState>());
            var persistenceId = stateObject.PersistenceId == -1L ? _nextId++ : stateObject.PersistenceId;
            store.Add(persistenceId, raw);
            stateObject.SetPersistenceId(persistenceId);
            return raw;
        }
        
        private static string? IdParameterAsString(object id)
        {
            return id?.ToString();
        }
    }
}