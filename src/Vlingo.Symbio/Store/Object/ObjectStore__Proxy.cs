using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Object
{
    public class ObjectStore__Proxy : Vlingo.Symbio.Store.Object.IObjectStore
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
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ => __.Close();
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, CloseRepresentation1);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            CloseRepresentation1));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, CloseRepresentation1));
            }
        }

        public bool IsNoId(long id)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ => __.IsNoId(id);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, IsNoIdRepresentation2);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            IsNoIdRepresentation2));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, IsNoIdRepresentation2));
            }

            return false;
        }

        public bool IsId(long id)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ => __.IsId(id);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, IsIdRepresentation3);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            IsIdRepresentation3));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, IsIdRepresentation3));
            }

            return false;
        }

        public ICompletes<IEntryReader<T>> EntryReader<T>(string name) where T : Vlingo.Symbio.IEntry
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ => __.EntryReader<T>(name);
                var completes = new BasicCompletes<IEntryReader<T>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, EntryReaderRepresentation4);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        completes, EntryReaderRepresentation4));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, EntryReaderRepresentation4));
            }

            return null;
        }

        public void QueryAll(Vlingo.Symbio.Store.Object.QueryExpression expression,
            Vlingo.Symbio.Store.Object.IQueryResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ => __.QueryAll(expression, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, QueryAllRepresentation5);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            QueryAllRepresentation5));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, QueryAllRepresentation5));
            }
        }

        public void QueryAll(Vlingo.Symbio.Store.Object.QueryExpression expression,
            Vlingo.Symbio.Store.Object.IQueryResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.QueryAll(expression, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, QueryAllRepresentation6);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            QueryAllRepresentation6));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, QueryAllRepresentation6));
            }
        }

        public void QueryObject(Vlingo.Symbio.Store.Object.QueryExpression expression,
            Vlingo.Symbio.Store.Object.IQueryResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ => __.QueryObject(expression, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, QueryObjectRepresentation7);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        QueryObjectRepresentation7));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, QueryObjectRepresentation7));
            }
        }

        public void QueryObject(Vlingo.Symbio.Store.Object.QueryExpression expression,
            Vlingo.Symbio.Store.Object.IQueryResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.QueryObject(expression, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, QueryObjectRepresentation8);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        QueryObjectRepresentation8));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, QueryObjectRepresentation8));
            }
        }

        public void Persist<TState>(TState stateObject, Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation9);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation9));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation9));
            }
        }

        public void Persist<TState>(TState stateObject, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation10);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation10));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation10));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation11);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation11));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation11));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation12);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation12));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation12));
            }
        }

        public void Persist<TState>(TState stateObject, Vlingo.Symbio.Store.Object.IPersistResultInterest interest,
            object @object) where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation13);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation13));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation13));
            }
        }

        public void Persist<TState>(TState stateObject, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation14);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation14));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation14));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation15);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation15));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation15));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation16);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation16));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation16));
            }
        }

        public void Persist<TState>(TState stateObject, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation17);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation17));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation17));
            }
        }

        public void Persist<TState>(TState stateObject, Vlingo.Symbio.Metadata metadata, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation18);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation18));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation18));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation19);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation19));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation19));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, long updateId, Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation20);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation20));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation20));
            }
        }

        public void Persist<TState>(TState stateObject, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation21);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation21));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation21));
            }
        }

        public void Persist<TState>(TState stateObject, Vlingo.Symbio.Metadata metadata, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState>(stateObject, metadata, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation22);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation22));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation22));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation23);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation23));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation23));
            }
        }

        public void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, long updateId, Vlingo.Symbio.Store.Object.IPersistResultInterest interest,
            object @object) where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.Persist<TState, TSource>(stateObject, sources, metadata, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistRepresentation24);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                            PersistRepresentation24));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistRepresentation24));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation25);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation25));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation25));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation26);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation26));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation26));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation27);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation27));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation27));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation28);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation28));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation28));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation29);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation29));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation29));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation30);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation30));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation30));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation31);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation31));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation31));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation32);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation32));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation32));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation33);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation33));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation33));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Vlingo.Symbio.Metadata metadata, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation34);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation34));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation34));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            long updateId, Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation35);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation35));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation35));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, long updateId, Vlingo.Symbio.Store.Object.IPersistResultInterest interest)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, updateId, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation36);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation36));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation36));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation37);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation37));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation37));
            }
        }

        public void PersistAll<TState>(IEnumerable<TState> stateObjects, Vlingo.Symbio.Metadata metadata, long updateId,
            Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState>(stateObjects, metadata, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation38);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation38));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation38));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            long updateId, Vlingo.Symbio.Store.Object.IPersistResultInterest interest, object @object)
            where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation39);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation39));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation39));
            }
        }

        public void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources,
            Vlingo.Symbio.Metadata metadata, long updateId, Vlingo.Symbio.Store.Object.IPersistResultInterest interest,
            object @object) where TState : Vlingo.Symbio.Store.Object.StateObject where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Object.IObjectStore> cons128873 = __ =>
                    __.PersistAll<TState, TSource>(stateObjects, sources, metadata, updateId, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, PersistAllRepresentation40);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Object.IObjectStore>(this.actor, cons128873,
                        PersistAllRepresentation40));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, PersistAllRepresentation40));
            }
        }
    }
}