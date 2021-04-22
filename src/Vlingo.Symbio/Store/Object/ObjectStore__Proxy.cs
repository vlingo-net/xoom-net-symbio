// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Symbio.Store.Object
{
    public class ObjectStore__Proxy : IObjectStore
    {
        private const string CloseRepresentation1 = "Close()";
        private const string IsNoIdRepresentation2 = "IsNoId(long)";
        private const string IsIdRepresentation3 = "IsId(long)";
        private const string EntryReaderRepresentation4 = "EntryReader<TNewEntry>(string)";

        private const string QueryAllRepresentation5 =
            "QueryAll(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest)";

        private const string QueryAllRepresentation6 =
            "QueryAll(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest, object)";

        private const string QueryObjectRepresentation7 =
            "QueryObject(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest)";

        private const string QueryObjectRepresentation8 =
            "QueryObject(Vlingo.Symbio.Store.Object.QueryExpression, Vlingo.Symbio.Store.Object.IQueryResultInterest, object)";

        private const string PersistRepresentation9 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation10 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation11 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation12 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation13 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation14 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistRepresentation15 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistRepresentation16 =
            "Persist<TState, TSource>(StateSources<TState, TSource>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation17 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation18 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation19 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation20 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation21 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation22 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest)";

        private const string PersistAllRepresentation23 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private const string PersistAllRepresentation24 =
            "PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>>, Vlingo.Symbio.Metadata, long, Vlingo.Symbio.Store.Object.IPersistResultInterest, object)";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public ObjectStore__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public long NoId => 0;

        public void Close()
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons743684197 = __ => __.Close();
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons743684197, null, CloseRepresentation1);
                else
                    mailbox.Send(
                        new LocalMessage<IObjectStore>(actor, cons743684197,
                            CloseRepresentation1));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, CloseRepresentation1));
            }
        }

        public bool IsNoId(long id)
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1686390220 = __ => __.IsNoId(id);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1686390220, null, IsNoIdRepresentation2);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1686390220, IsNoIdRepresentation2));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, IsNoIdRepresentation2));
            }

            return false;
        }

        public bool IsId(long id)
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1390614019 = __ => __.IsId(id);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1390614019, null, IsIdRepresentation3);
                else
                    mailbox.Send(
                        new LocalMessage<IObjectStore>(actor, cons1390614019,
                            IsIdRepresentation3));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, IsIdRepresentation3));
            }

            return false;
        }

        public ICompletes<IEntryReader<TNewEntry>> EntryReader<TNewEntry>(string name)
            where TNewEntry : IEntry
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons371671809 = __ => __.EntryReader<TNewEntry>(name);
                var completes = new BasicCompletes<IEntryReader<TNewEntry>>(actor.Scheduler);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons371671809, completes, EntryReaderRepresentation4);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons371671809, completes, EntryReaderRepresentation4));

                return completes;
            }

            actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, EntryReaderRepresentation4));

            return null!;
        }

        public void QueryAll(QueryExpression expression,
            IQueryResultInterest interest)
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons956609003 = __ => __.QueryAll(expression, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons956609003, null, QueryAllRepresentation5);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons956609003, QueryAllRepresentation5));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, QueryAllRepresentation5));
            }
        }

        public void QueryAll(QueryExpression expression,
            IQueryResultInterest interest, object? @object)
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons272555097 = __ =>
                    __.QueryAll(expression, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons272555097, null, QueryAllRepresentation6);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons272555097, QueryAllRepresentation6));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, QueryAllRepresentation6));
            }
        }

        public void QueryObject(QueryExpression expression,
            IQueryResultInterest interest)
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1823632399 = __ =>
                    __.QueryObject(expression, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1823632399, null, QueryObjectRepresentation7);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1823632399, QueryObjectRepresentation7));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, QueryObjectRepresentation7));
            }
        }

        public void QueryObject(QueryExpression expression,
            IQueryResultInterest interest, object? @object)
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1357924112 = __ =>
                    __.QueryObject(expression, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1357924112, null, QueryObjectRepresentation8);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1357924112, QueryObjectRepresentation8));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, QueryObjectRepresentation8));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources,
            IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons738467078 = __ =>
                    __.Persist(stateSources, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons738467078, null, PersistRepresentation9);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons738467078, PersistRepresentation9));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation9));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources,
            Metadata metadata, IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons297809971 = __ =>
                    __.Persist(stateSources, metadata, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons297809971, null, PersistRepresentation10);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons297809971, PersistRepresentation10));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation10));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1425279528 = __ =>
                    __.Persist(stateSources, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1425279528, null, PersistRepresentation11);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1425279528, PersistRepresentation11));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation11));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources,
            Metadata metadata, IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1263382963 = __ =>
                    __.Persist(stateSources, metadata, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1263382963, null, PersistRepresentation12);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1263382963, PersistRepresentation12));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation12));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, long updateId,
            IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons440866163 = __ =>
                    __.Persist(stateSources, updateId, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons440866163, null, PersistRepresentation13);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons440866163, PersistRepresentation13));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation13));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources,
            Metadata metadata, long updateId, IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1787697675 = __ =>
                    __.Persist(stateSources, metadata, updateId, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1787697675, null, PersistRepresentation14);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1787697675, PersistRepresentation14));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation14));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, long updateId,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1658623364 = __ =>
                    __.Persist(stateSources, updateId, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1658623364, null, PersistRepresentation15);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1658623364, PersistRepresentation15));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation15));
            }
        }

        public void Persist<TState, TSource>(StateSources<TState, TSource> stateSources,
            Metadata metadata, long updateId, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1567165775 = __ =>
                    __.Persist(stateSources, metadata, updateId, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1567165775, null, PersistRepresentation16);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1567165775, PersistRepresentation16));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistRepresentation16));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons542297984 = __ =>
                    __.PersistAll(allStateSources, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons542297984, null, PersistAllRepresentation17);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons542297984, PersistAllRepresentation17));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation17));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            Metadata metadata, IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons964439767 = __ =>
                    __.PersistAll(allStateSources, metadata, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons964439767, null, PersistAllRepresentation18);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons964439767, PersistAllRepresentation18));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation18));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1292430346 = __ =>
                    __.PersistAll(allStateSources, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1292430346, null, PersistAllRepresentation19);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1292430346, PersistAllRepresentation19));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation19));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            Metadata metadata, IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons106955477 = __ =>
                    __.PersistAll(allStateSources, metadata, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons106955477, null, PersistAllRepresentation20);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons106955477, PersistAllRepresentation20));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation20));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            long updateId, IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1805700743 = __ =>
                    __.PersistAll(allStateSources, updateId, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1805700743, null, PersistAllRepresentation21);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1805700743, PersistAllRepresentation21));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation21));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            Metadata metadata, long updateId, IPersistResultInterest interest)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons56923825 = __ =>
                    __.PersistAll(allStateSources, metadata, updateId, interest);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons56923825, null, PersistAllRepresentation22);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons56923825, PersistAllRepresentation22));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation22));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            long updateId, IPersistResultInterest interest, object? @object)
            where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons1420492840 = __ =>
                    __.PersistAll(allStateSources, updateId, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons1420492840, null, PersistAllRepresentation23);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons1420492840, PersistAllRepresentation23));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation23));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources,
            Metadata metadata, long updateId, IPersistResultInterest interest,
            object? @object) where TState : StateObject where TSource : ISource
        {
            if (!actor.IsStopped)
            {
                Action<IObjectStore> cons129016173 = __ =>
                    __.PersistAll(allStateSources, metadata, updateId, interest, @object);
                if (mailbox.IsPreallocated)
                    mailbox.Send(actor, cons129016173, null, PersistAllRepresentation24);
                else
                    mailbox.Send(new LocalMessage<IObjectStore>(actor,
                        cons129016173, PersistAllRepresentation24));
            }
            else
            {
                actor.DeadLetters?.FailedDelivery(new DeadLetter(actor, PersistAllRepresentation24));
            }
        }
    }
}