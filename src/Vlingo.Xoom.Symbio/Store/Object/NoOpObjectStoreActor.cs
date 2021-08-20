// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Object
{
    public sealed class NoOpObjectStoreActor<T> : Actor, IObjectStore
    {
        private const string WarningMessage =
            "\n===============================================================================================\n" +
            "                                                                                             \n" +
            " All journal operations are stopped. Please check your DB settings and user credentials.     \n" +
            "                                                                                             \n" +
            "===============================================================================================\n";

        public NoOpObjectStoreActor()
        {
            Logger.Warn(WarningMessage);
            Stop();
        }
        
        
        public long NoId { get; }
        
        public bool IsNoId(long id)
        {
            Logger.Error(WarningMessage);
            return false;
        }

        public bool IsId(long id)
        {
            Logger.Error(WarningMessage);
            return false;
        }

        public ICompletes<IEntryReader> EntryReader<TNewEntry>(string name) where TNewEntry : IEntry
        {
            Logger.Error(WarningMessage);
            return Common.Completes.WithFailure<IEntryReader>();
        }

        public void QueryAll(QueryExpression expression, IQueryResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void QueryAll(QueryExpression expression, IQueryResultInterest interest, object? @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void QueryObject(QueryExpression expression, IQueryResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void QueryObject(QueryExpression expression, IQueryResultInterest interest, object? @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, long updateId,
            IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, long updateId, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, long updateId,
            IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, long updateId,
            IPersistResultInterest interest) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, long updateId, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, long updateId,
            IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }

        public void Close()
        {
            Logger.Error(WarningMessage);
            ((ICompletes<T>)Completes()).Failed();
        }
    }
}