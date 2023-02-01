// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Common.Identity;
using Vlingo.Xoom.Symbio.Store.Dispatch;

namespace Vlingo.Xoom.Symbio.Store.Object.InMemory;

public class InMemoryObjectStoreDelegate : IObjectStoreDelegate
{
    private long _nextId;

    private readonly Dictionary<Type , Dictionary<long, IState>> _stores;
    private readonly List<IEntry> _entries;
    private readonly List<Dispatchable> _dispatchables;
    private readonly StateAdapterProvider _stateAdapterProvider;
    private readonly IIdentityGenerator _identityGenerator;

    public InMemoryObjectStoreDelegate(StateAdapterProvider stateAdapterProvider)
    {
        _stateAdapterProvider = stateAdapterProvider;
        _stores = new Dictionary<Type, Dictionary<long, IState>>();
        _entries = new List<IEntry>();
        _dispatchables = new List<Dispatchable>();
        _identityGenerator = IdentityGeneratorType.Random.Generator();

        _nextId = 1;
    }
        
    public void ConfirmDispatched(string dispatchId)
    {
        var toRemove = _dispatchables.FirstOrDefault(d => d.Id.Equals(dispatchId));
        _dispatchables.Remove(toRemove!);
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
    public IObjectStoreDelegate Copy() => new InMemoryObjectStoreDelegate(_stateAdapterProvider);

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
    public IEnumerable<IState> PersistAll(IEnumerable<StateObject> stateObjects, long updateId, Metadata metadata)
    {
        var states = stateObjects as StateObject[] ?? stateObjects.ToArray();
        var persistedStates = new List<IState>(states.Length);
        foreach (var stateObject in states)
        {
            var raw = Persist(stateObject, metadata);
            persistedStates.Add(raw);
        }

        return persistedStates;
    }

    /// <inheritdoc />
    public IState Persist(StateObject stateObject, long updateId, Metadata metadata) =>
        Persist(stateObject, metadata);

    /// <inheritdoc />
    public void PersistEntries(IEnumerable<IEntry> entries)
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
    public void PersistDispatchable(Dispatchable dispatchable) => _dispatchables.Add(dispatchable);

    /// <inheritdoc />
    public QueryMultiResults QueryAll(QueryExpression expression)
    {
        // NOTE: No query constraints accepted; selects all stored objects

        var all = new List<StateObject>();
        var store = _stores.ComputeIfAbsent(expression.Type, _ => new Dictionary<long, IState>());
        foreach (var entry in store.Values)
        {
            var stateObject = _stateAdapterProvider.FromRaw<StateObject, IState>(entry);
            all.Add(stateObject);
        }

        return new QueryMultiResults(all);
    }

    /// <inheritdoc />
    public QuerySingleResult QueryObject(QueryExpression expression)
    {
        string? id = null;
        if (expression.IsListQueryExpression)
        {
            id = IdParameterAsString(expression.AsListQueryExpression().Parameters.FirstOrDefault());
        }
        else if (expression.IsMapQueryExpression)
        {
            if (expression.AsMapQueryExpression().Parameters.TryGetValue("id", out var objectId))
            {
                id = IdParameterAsString(objectId);
            }
        }
        else
        {
            throw new StorageException(Result.Error, $"Unknown query type: {expression}");
        }

        var store = _stores.ComputeIfAbsent(expression.Type, _ => new Dictionary<long, IState>());
        var found = id == null || id.Equals("-1") ? null! : store[long.Parse(id)];

        var result = Optional
            .OfNullable(found)
            .Map(state => _stateAdapterProvider.FromRaw<StateObject, IState>(state))
            .OrElse(null!);
        return new QuerySingleResult(result);
    }

    /// <inheritdoc />
    public IEnumerable<Dispatchable> AllUnconfirmedDispatchableStates => _dispatchables;
        
    /// <inheritdoc />
    public IEnumerable<StateObjectMapper> RegisteredMappers { get; } = Enumerable.Empty<StateObjectMapper>();
        
    internal List<IEntry> ReadOnlyJournal() => _entries;

    private IState Persist(StateObject stateObject, Metadata metadata)
    {
        var raw = _stateAdapterProvider.AsRaw<StateObject, IState>(stateObject.PersistenceId.ToString(), stateObject, 1, metadata);
        var store = _stores.ComputeIfAbsent(stateObject.GetType(), _ => new Dictionary<long, IState>());
        var persistenceId = stateObject.PersistenceId == -1L ? _nextId++ : stateObject.PersistenceId;
        if (store.ContainsKey(persistenceId))
        {
            store[persistenceId] = raw;
        }
        else
        {
            store.Add(persistenceId, raw);
        }
        stateObject.SetPersistenceId(persistenceId);
        return raw;
    }
        
    private static string? IdParameterAsString(object? id) => id?.ToString();
}