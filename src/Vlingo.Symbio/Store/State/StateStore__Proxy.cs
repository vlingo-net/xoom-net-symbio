using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.State
{
    public class StateStore__Proxy<TEntry> : Vlingo.Symbio.Store.State.IStateStore<TEntry>
        where TEntry : Vlingo.Symbio.IEntry
    {
        private const string EntryReaderRepresentation1 = "EntryReader(string)";

        private const string ReadRepresentation2 =
            "Read<TState>(string, Vlingo.Symbio.Store.State.IReadResultInterest)";

        private const string ReadRepresentation3 =
            "Read<TState>(string, Vlingo.Symbio.Store.State.IReadResultInterest, object)";

        private const string WriteRepresentation4 =
            "Write<TState>(string, TState, int, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation5 =
            "Write<TState, TSource>(string, TState, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation6 =
            "Write<TState>(string, TState, int, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation7 =
            "Write<TState, TSource>(string, TState, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest)";

        private const string WriteRepresentation8 =
            "Write<TState>(string, TState, int, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private const string WriteRepresentation9 =
            "Write<TState, TSource>(string, TState, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private const string WriteRepresentation10 =
            "Write<TState>(string, TState, int, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private const string WriteRepresentation11 =
            "Write<TState, TSource>(string, TState, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.State.IWriteResultInterest, object)";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public StateStore__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public ICompletes<IStateStoreEntryReader<TEntry>> EntryReader(string name)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ => __.EntryReader(name);
                var completes = new BasicCompletes<IStateStoreEntryReader<TEntry>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, EntryReaderRepresentation1);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor,
                        cons128873, completes, EntryReaderRepresentation1));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, EntryReaderRepresentation1));
            }

            return null!;
        }

        public void Read<TState>(string id, Vlingo.Symbio.Store.State.IReadResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ => __.Read<TState>(id, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, ReadRepresentation2);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            ReadRepresentation2));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadRepresentation2));
            }
        }

        public void Read<TState>(string id, Vlingo.Symbio.Store.State.IReadResultInterest interest, object? @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Read<TState>(id, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, ReadRepresentation3);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            ReadRepresentation3));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadRepresentation3));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation4);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation4));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation4));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation5);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation5));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation5));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation6);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation6));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation6));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, metadata, interest);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation7);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation7));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation7));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation8);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation8));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation8));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, Vlingo.Symbio.Store.State.IWriteResultInterest interest,
            object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation9);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation9));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation9));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation10);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation10));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation10));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.State.IWriteResultInterest interest, object @object)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, WriteRepresentation11);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStore<TEntry>>(this.actor, cons128873,
                            WriteRepresentation11));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, WriteRepresentation11));
            }
        }
    }
}