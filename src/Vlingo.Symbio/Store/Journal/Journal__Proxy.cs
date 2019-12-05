using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Store.Journal
{
    public class Journal__Proxy<TEntry> : Vlingo.Symbio.Store.Journal.IJournal<TEntry>
        where TEntry : Vlingo.Symbio.IEntry
    {
        private const string UsingRepresentation1 =
            "Using<TActor, TState>(Vlingo.Actors.Stage, IDispatcher<Dispatchable<TEntry, TState>>, System.Object[])";

        private const string AppendRepresentation2 =
            "Append<TSource, TSnapshotState>(string, int, Source<TSource>, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendRepresentation3 =
            "Append<TSource, TSnapshotState>(string, int, Source<TSource>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendWithRepresentation4 =
            "AppendWith<TSource, TSnapshotState>(string, int, Source<TSource>, TSnapshotState, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendWithRepresentation5 =
            "AppendWith<TSource, TSnapshotState>(string, int, Source<TSource>, Vlingo.Symbio.Metadata, TSnapshotState, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendAllRepresentation6 =
            "AppendAll<TSource, TSnapshotState>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendAllRepresentation7 =
            "AppendAll<TSource, TSnapshotState>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Metadata, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendAllWithRepresentation8 =
            "AppendAllWith<TSource, TSnapshotState>(string, int, IEnumerable<Source<TSource>>, TSnapshotState, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string AppendAllWithRepresentation9 =
            "AppendAllWith<TSource, TSnapshotState>(string, int, IEnumerable<Source<TSource>>, Vlingo.Symbio.Metadata, TSnapshotState, Vlingo.Symbio.Store.Journal.IAppendResultInterest, object)";

        private const string JournalReaderRepresentation10 = "JournalReader<TNewEntry>(string)";
        private const string StreamReaderRepresentation11 = "StreamReader(string)";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public Journal__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public Vlingo.Symbio.Store.Journal.IJournal<TEntry> Using<TActor, TState>(Vlingo.Actors.Stage stage,
            IDispatcher<Dispatchable<TEntry, TState>> dispatcher, System.Object[] additional)
            where TActor : Vlingo.Actors.Actor where TState : class, IState
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.Using<TActor, TState>(stage, dispatcher, additional);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, UsingRepresentation1);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            UsingRepresentation1));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, UsingRepresentation1));
            }

            return null!;
        }

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.Append<TSource, TSnapshotState>(streamName, streamVersion, source, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendRepresentation2);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            AppendRepresentation2));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendRepresentation2));
            }
        }

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            Vlingo.Symbio.Metadata metadata, Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.Append<TSource, TSnapshotState>(streamName, streamVersion, source, metadata, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendRepresentation3);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            AppendRepresentation3));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendRepresentation3));
            }
        }

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            TSnapshotState snapshot, Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.AppendWith<TSource, TSnapshotState>(streamName, streamVersion, source, snapshot, interest,
                        @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendWithRepresentation4);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            AppendWithRepresentation4));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendWithRepresentation4));
            }
        }

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            Vlingo.Symbio.Metadata metadata, TSnapshotState snapshot,
            Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.AppendWith<TSource, TSnapshotState>(streamName, streamVersion, source, metadata, snapshot,
                        interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendWithRepresentation5);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            AppendWithRepresentation5));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendWithRepresentation5));
            }
        }

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, Vlingo.Symbio.Store.Journal.IAppendResultInterest interest,
            object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendAllRepresentation6);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            AppendAllRepresentation6));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendAllRepresentation6));
            }
        }

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, Vlingo.Symbio.Metadata metadata,
            Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata, interest,
                        @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendAllRepresentation7);
                }
                else
                {
                    this.mailbox.Send(
                        new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor, cons128873,
                            AppendAllRepresentation7));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendAllRepresentation7));
            }
        }

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, TSnapshotState snapshot,
            Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, snapshot,
                        interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendAllWithRepresentation8);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor,
                        cons128873, AppendAllWithRepresentation8));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendAllWithRepresentation8));
            }
        }

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, Vlingo.Symbio.Metadata metadata, TSnapshotState snapshot,
            Vlingo.Symbio.Store.Journal.IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata,
                        snapshot, interest, @object);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, null, AppendAllWithRepresentation9);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor,
                        cons128873, AppendAllWithRepresentation9));
                }
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, AppendAllWithRepresentation9));
            }
        }

        public ICompletes<IJournalReader<TNewEntry>?> JournalReader<TNewEntry>(string name)
            where TNewEntry : Vlingo.Symbio.IEntry
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ =>
                    __.JournalReader<TNewEntry>(name);
                var completes = new BasicCompletes<IJournalReader<TNewEntry>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, JournalReaderRepresentation10);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor,
                        cons128873, completes, JournalReaderRepresentation10));
                }

                return completes!;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, JournalReaderRepresentation10));
            }

            return null!;
        }

        public ICompletes<IStreamReader<TEntry>?> StreamReader(string name)
        {
            if (!this.actor.IsStopped)
            {
                Action<Vlingo.Symbio.Store.Journal.IJournal<TEntry>> cons128873 = __ => __.StreamReader(name);
                var completes = new BasicCompletes<IStreamReader<TEntry>>(this.actor.Scheduler);
                if (this.mailbox.IsPreallocated)
                {
                    this.mailbox.Send(this.actor, cons128873, completes, StreamReaderRepresentation11);
                }
                else
                {
                    this.mailbox.Send(new LocalMessage<Vlingo.Symbio.Store.Journal.IJournal<TEntry>>(this.actor,
                        cons128873, completes, StreamReaderRepresentation11));
                }

                return completes!;
            }
            else
            {
                this.actor.DeadLetters.FailedDelivery(new DeadLetter(this.actor, StreamReaderRepresentation11));
            }

            return null!;
        }
    }
}