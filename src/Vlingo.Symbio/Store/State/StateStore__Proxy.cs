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

namespace Vlingo.Symbio.Store.State
{
    public class StateStore__Proxy<TEntry> : IStateStore<TEntry>
        where TEntry : IEntry
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

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public StateStore__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public ICompletes<IStateStoreEntryReader<TEntry>> EntryReader(string name)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ => __.EntryReader(name);
                var completes = new BasicCompletes<IStateStoreEntryReader<TEntry>>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, EntryReaderRepresentation1);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IStateStore<TEntry>>(_actor,
                        cons128873, completes, EntryReaderRepresentation1));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, EntryReaderRepresentation1));
            }

            return null!;
        }

        public void Read<TState>(string id, IReadResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ => __.Read<TState>(id, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, ReadRepresentation2);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            ReadRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadRepresentation2));
            }
        }

        public void Read<TState>(string id, IReadResultInterest interest, object? @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Read<TState>(id, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, ReadRepresentation3);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            ReadRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadRepresentation3));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion,
            IWriteResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation4);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation4));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation4));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, IWriteResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation5);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation5));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata,
            IWriteResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, metadata, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation6);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation6));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, Metadata metadata,
            IWriteResultInterest interest)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, metadata, interest);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation7);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation7));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion,
            IWriteResultInterest interest, object @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation8);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation8));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, IWriteResultInterest interest,
            object @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation9);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation9));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation9));
            }
        }

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata,
            IWriteResultInterest interest, object @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState>(id, state, stateVersion, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation10);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation10));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation10));
            }
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion,
            IEnumerable<Source<TSource>> sources, Metadata metadata,
            IWriteResultInterest interest, object @object)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStore<TEntry>> cons128873 = __ =>
                    __.Write<TState, TSource>(id, state, stateVersion, sources, metadata, interest, @object);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, WriteRepresentation11);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStore<TEntry>>(_actor, cons128873,
                            WriteRepresentation11));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation11));
            }
        }
    }
}