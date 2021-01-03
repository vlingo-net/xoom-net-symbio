// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Store.Journal
{
    public class Journal__Proxy<T> : IJournal<T>
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

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public Journal__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public IJournal<T> Using<TActor, TEntry, TState>(Stage stage,
            IDispatcher<Dispatchable<TEntry, TState>> dispatcher, System.Object[] additional)
            where TActor : Actor where TEntry : IEntry<T> where TState : class, IState
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.Using<TActor, TEntry, TState>(stage, dispatcher, additional);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, UsingRepresentation1);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            UsingRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, UsingRepresentation1));
            }

            return null!;
        }

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.Append<TSource, TSnapshotState>(streamName, streamVersion, source, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendRepresentation2);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            AppendRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendRepresentation2));
            }
        }

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            Metadata metadata, IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.Append<TSource, TSnapshotState>(streamName, streamVersion, source, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendRepresentation3);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            AppendRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendRepresentation3));
            }
        }

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.AppendWith<TSource, TSnapshotState>(streamName, streamVersion, source, snapshot, interest,
                        @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendWithRepresentation4);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            AppendWithRepresentation4));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendWithRepresentation4));
            }
        }

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source,
            Metadata metadata, TSnapshotState snapshot,
            IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.AppendWith<TSource, TSnapshotState>(streamName, streamVersion, source, metadata, snapshot,
                        interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendWithRepresentation5);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            AppendWithRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendWithRepresentation5));
            }
        }

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, IAppendResultInterest interest,
            object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendAllRepresentation6);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            AppendAllRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendAllRepresentation6));
            }
        }

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, Metadata metadata,
            IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata, interest,
                        @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendAllRepresentation7);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IJournal<T>>(_actor, cons128873,
                            AppendAllRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendAllRepresentation7));
            }
        }

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, TSnapshotState snapshot,
            IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, snapshot,
                        interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendAllWithRepresentation8);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IJournal<T>>(_actor,
                        cons128873, AppendAllWithRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendAllWithRepresentation8));
            }
        }

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion,
            IEnumerable<TSource> sources, Metadata metadata, TSnapshotState snapshot,
            IAppendResultInterest interest, object @object) where TSource : Source
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata,
                        snapshot, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, AppendAllWithRepresentation9);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IJournal<T>>(_actor,
                        cons128873, AppendAllWithRepresentation9));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AppendAllWithRepresentation9));
            }
        }

        public ICompletes<IJournalReader<TNewEntry>?> JournalReader<TNewEntry>(string name)
            where TNewEntry : IEntry
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ =>
                    __.JournalReader<TNewEntry>(name);
                var completes = new BasicCompletes<IJournalReader<TNewEntry>>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, JournalReaderRepresentation10);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IJournal<T>>(_actor,
                        cons128873, completes, JournalReaderRepresentation10));
                }

                return completes!;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, JournalReaderRepresentation10));
            }

            return null!;
        }

        public ICompletes<IStreamReader<T>?> StreamReader(string name)
        {
            if (!_actor.IsStopped)
            {
                Action<IJournal<T>> cons128873 = __ => __.StreamReader(name);
                var completes = new BasicCompletes<IStreamReader<T>>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, StreamReaderRepresentation11);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IJournal<T>>(_actor,
                        cons128873, completes, StreamReaderRepresentation11));
                }

                return completes!;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StreamReaderRepresentation11));
            }

            return null!;
        }
    }
}