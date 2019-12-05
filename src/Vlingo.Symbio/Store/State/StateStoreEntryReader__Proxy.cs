using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.State
{
    public class StateStoreEntryReader__Proxy<TEntry> : Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>
        where TEntry : Vlingo.Symbio.IEntry
    {
        private const string CloseRepresentation1 = "Close()";
        private const string ReadNextRepresentation2 = "ReadNext()";
        private const string ReadNextRepresentation3 = "ReadNext(string)";
        private const string ReadNextRepresentation4 = "ReadNext(int)";
        private const string ReadNextRepresentation5 = "ReadNext(string, int)";
        private const string RewindRepresentation6 = "Rewind()";
        private const string SeekToRepresentation7 = "SeekTo(string)";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public StateStoreEntryReader__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public string Beginning => null!;
        public string End => null!;
        public string Query => null!;
        public int DefaultGapPreventionRetries => 0;
        public long DefaultGapPreventionRetryInterval => 0;
        public Vlingo.Common.ICompletes<string> Name => null!;
        public Vlingo.Common.ICompletes<long> Size => null!;

        public void Close()
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ => __.Close();
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, CloseRepresentation1);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, CloseRepresentation1));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, CloseRepresentation1));
            }
        }

        public ICompletes<TEntry> ReadNext()
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ => __.ReadNext();
                var completes = new BasicCompletes<TEntry>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, ReadNextRepresentation2);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, completes, ReadNextRepresentation2));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadNextRepresentation2));
            }

            return null!;
        }

        public ICompletes<TEntry> ReadNext(string fromId)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ => __.ReadNext(fromId);
                var completes = new BasicCompletes<TEntry>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, ReadNextRepresentation3);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, completes, ReadNextRepresentation3));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadNextRepresentation3));
            }

            return null!;
        }

        public ICompletes<IEnumerable<TEntry>> ReadNext(int maximumEntries)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ =>
                    __.ReadNext(maximumEntries);
                var completes = new BasicCompletes<IEnumerable<TEntry>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, ReadNextRepresentation4);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, completes, ReadNextRepresentation4));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadNextRepresentation4));
            }

            return null!;
        }

        public ICompletes<IEnumerable<TEntry>> ReadNext(string fromId, int maximumEntries)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ =>
                    __.ReadNext(fromId, maximumEntries);
                var completes = new BasicCompletes<IEnumerable<TEntry>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, ReadNextRepresentation5);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, completes, ReadNextRepresentation5));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, ReadNextRepresentation5));
            }

            return null!;
        }

        public void Rewind()
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ => __.Rewind();
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, RewindRepresentation6);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, RewindRepresentation6));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, RewindRepresentation6));
            }
        }

        public Vlingo.Common.ICompletes<string> SeekTo(string id)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>> cons128873 = __ => __.SeekTo(id);
                var completes = new BasicCompletes<string>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, SeekToRepresentation7);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.State.IStateStoreEntryReader<TEntry>>(this.actor,
                            cons128873, completes, SeekToRepresentation7));
                }

                return completes;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, SeekToRepresentation7));
            }

            return null!;
        }
    }
}