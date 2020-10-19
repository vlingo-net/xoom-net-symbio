// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Object
{
    public class ObjectStore__Proxy : IObjectStore
    {
        private const string CloseRepresentation1 = "Close()";
        private const string IsNoIdRepresentation2 = "IsNoId(long)";
        private const string IsIdRepresentation3 = "IsId(long)";
        private const string EntryReaderRepresentation4 = "EntryReader<T>(string)";

        private const string QueryAllRepresentation5 =
            "QueryAll(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest)";

        private const string QueryAllRepresentation6 =
            "QueryAll(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest, object)";

        private const string QueryObjectRepresentation7 =
            "QueryObject(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest)";

        private const string QueryObjectRepresentation8 =
            "QueryObject(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest, object)";

        private const string PersistRepresentation9 =
            "Persist<TState>(TState, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation10 =
            "Persist<TState>(TState, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation11 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation12 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation13 =
            "Persist<TState>(TState, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation14 =
            "Persist<TState>(TState, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation15 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation16 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation17 =
            "Persist<TState>(TState, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation18 =
            "Persist<TState>(TState, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation19 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation20 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation21 =
            "Persist<TState>(TState, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation22 =
            "Persist<TState>(TState, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation23 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation24 =
            "Persist<TState, TSource>(TState, IEnumerable<TSource>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation25 =
            "PersistAll<TState>(IEnumerable<TState>, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation26 =
            "PersistAll<TState>(IEnumerable<TState>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation27 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation28 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation29 =
            "PersistAll<TState>(IEnumerable<TState>, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation30 =
            "PersistAll<TState>(IEnumerable<TState>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation31 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation32 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation33 =
            "PersistAll<TState>(IEnumerable<TState>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation34 =
            "PersistAll<TState>(IEnumerable<TState>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation35 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation36 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation37 =
            "PersistAll<TState>(IEnumerable<TState>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation38 =
            "PersistAll<TState>(IEnumerable<TState>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation39 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation40 =
            "PersistAll<TState, TSource>(IEnumerable<TState>, IEnumerable<TSource>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public ObjectStore__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public long NoId => 0;

        public void Close()
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ => __.Close();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, CloseRepresentation1);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            CloseRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, CloseRepresentation1));
            }
        }

        public bool IsNoId(long id)
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ => __.IsNoId(id);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, IsNoIdRepresentation2);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            IsNoIdRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, IsNoIdRepresentation2));
            }

            return false;
        }

        public bool IsId(long id)
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ => __.IsId(id);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, IsIdRepresentation3);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            IsIdRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, IsIdRepresentation3));
            }

            return false;
        }

        public ICompletes<IEntryReader<T>> EntryReader<T>(string name) where T : IEntry
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ => __.EntryReader<T>(name);
                var completes = new BasicCompletes<IEntryReader<T>>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, EntryReaderRepresentation4);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        completes, EntryReaderRepresentation4));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, EntryReaderRepresentation4));
            }

            return null!;
        }

        public void QueryAll(QueryExpression expression,
            IQueryResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ => __.QueryAll(expression, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, QueryAllRepresentation5);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            QueryAllRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, QueryAllRepresentation5));
            }
        }

        public void QueryAll(QueryExpression expression,
            IQueryResultInterest interest, object? @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.QueryAll(expression, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, QueryAllRepresentation6);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            QueryAllRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, QueryAllRepresentation6));
            }
        }

        public void QueryObject(QueryExpression expression,
            IQueryResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ => __.QueryObject(expression, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, QueryObjectRepresentation7);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        QueryObjectRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, QueryObjectRepresentation7));
            }
        }

        public void QueryObject(QueryExpression expression,
            IQueryResultInterest interest, object? @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.QueryObject(expression, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, QueryObjectRepresentation8);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        QueryObjectRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, QueryObjectRepresentation8));
            }
        }

        public void Persist<TState>(TState stateObject, IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation9);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation9));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation9));
            }
        }

        public void Persist<TState>(TState stateObject, Metadata metadata,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation10);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation10));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation10));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation11);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation11));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation11));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Metadata metadata, IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation12);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation12));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation12));
            }
        }

        public void Persist<TState>(TState stateObject, IPersistResultInterest interest,
            object? @object) where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation13);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation13));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation13));
            }
        }

        public void Persist<TState>(TState stateObject, Metadata metadata,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation14);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation14));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation14));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation15);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation15));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation15));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Metadata metadata, IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation16);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation16));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation16));
            }
        }

        public void Persist<TState>(TState stateObject, long updateId,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation17);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation17));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation17));
            }
        }

        public void Persist<TState>(TState stateObject, Metadata metadata, long updateId,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation18);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation18));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation18));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, long updateId,
            IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation19);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation19));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation19));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Metadata metadata, long updateId, IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation20);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation20));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation20));
            }
        }

        public void Persist<TState>(TState stateObject, long updateId,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation21);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation21));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation21));
            }
        }

        public void Persist<TState>(TState stateObject, Metadata metadata, long updateId,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation22);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation22));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation22));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, long updateId,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation23);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation23));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation23));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Metadata metadata, long updateId, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistRepresentation24);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IObjectStore>(_actor, cons128873,
                            PersistRepresentation24));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistRepresentation24));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation25);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation25));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation25));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation26);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation26));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation26));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation27);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation27));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation27));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Metadata metadata, IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation28);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation28));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation28));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation29);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation29));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation29));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation30);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation30));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation30));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation31);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation31));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation31));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Metadata metadata, IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation32);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation32));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation32));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, long updateId,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation33);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation33));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation33));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata, long updateId,
            IPersistResultInterest interest)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation34);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation34));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation34));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            long updateId, IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation35);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation35));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation35));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Metadata metadata, long updateId, IPersistResultInterest interest)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, updateId, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation36);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation36));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation36));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, long updateId,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation37);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation37));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation37));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata, long updateId,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation38);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation38));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation38));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            long updateId, IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation39);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation39));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation39));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Metadata metadata, long updateId, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, updateId, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, PersistAllRepresentation40);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IObjectStore>(_actor, cons128873,
                        PersistAllRepresentation40));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PersistAllRepresentation40));
            }
        }
    }
}