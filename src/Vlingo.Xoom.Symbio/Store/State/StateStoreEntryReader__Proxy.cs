// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Streams;

namespace Vlingo.Xoom.Symbio.Store.State
{
    public class StateStoreEntryReader__Proxy : IStateStoreEntryReader
    {
        private const string CloseRepresentation1 = "Close()";
        private const string ReadNextRepresentation2 = "ReadNext()";
        private const string ReadNextRepresentation3 = "ReadNext(string)";
        private const string ReadNextRepresentation4 = "ReadNext(int)";
        private const string ReadNextRepresentation5 = "ReadNext(string, int)";
        private const string RewindRepresentation6 = "Rewind()";
        private const string SeekToRepresentation7 = "SeekTo(string)";
        private const string StreamAllRepresentation8 = "StreamAll()";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public StateStoreEntryReader__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public string Beginning => null!;
        public string End => null!;
        public string Query => null!;
        public int DefaultGapPreventionRetries => 0;
        public long DefaultGapPreventionRetryInterval => 0;
        public ICompletes<string> Name => null!;
        public ICompletes<long> Size => null!;

        public void Close()
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ => __.Close();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, CloseRepresentation1);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, CloseRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, CloseRepresentation1));
            }
        }

        public ICompletes<IEntry> ReadNext()
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ => __.ReadNext();
                var completes = new BasicCompletes<IEntry>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, ReadNextRepresentation2);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, completes, ReadNextRepresentation2));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadNextRepresentation2));
            }

            return null!;
        }

        public ICompletes<IEntry> ReadNext(string fromId)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ => __.ReadNext(fromId);
                var completes = new BasicCompletes<IEntry>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, ReadNextRepresentation3);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, completes, ReadNextRepresentation3));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadNextRepresentation3));
            }

            return null!;
        }

        public ICompletes<IEnumerable<IEntry>> ReadNext(int maximumEntries)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ =>
                    __.ReadNext(maximumEntries);
                var completes = new BasicCompletes<IEnumerable<IEntry>>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, ReadNextRepresentation4);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, completes, ReadNextRepresentation4));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadNextRepresentation4));
            }

            return null!;
        }

        public ICompletes<IEnumerable<IEntry>> ReadNext(string fromId, int maximumEntries)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ =>
                    __.ReadNext(fromId, maximumEntries);
                var completes = new BasicCompletes<IEnumerable<IEntry>>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, ReadNextRepresentation5);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, completes, ReadNextRepresentation5));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadNextRepresentation5));
            }

            return null!;
        }

        public void Rewind()
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ => __.Rewind();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, null, RewindRepresentation6);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, RewindRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, RewindRepresentation6));
            }
        }

        public ICompletes<string> SeekTo(string id)
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128873 = __ => __.SeekTo(id);
                var completes = new BasicCompletes<string>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128873, completes, SeekToRepresentation7);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128873, completes, SeekToRepresentation7));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, SeekToRepresentation7));
            }

            return null!;
        }
        
        public ICompletes<IStream> StreamAll()
        {
            if (!_actor.IsStopped)
            {
                Action<IStateStoreEntryReader> cons128870 = __ => __.StreamAll();
                var completes = new BasicCompletes<IStream>(_actor.Scheduler);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, cons128870, completes, StreamAllRepresentation8);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IStateStoreEntryReader>(_actor,
                            cons128870, completes, StreamAllRepresentation8));
                }

                return completes;
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StreamAllRepresentation8));
            }

            return null!;
        }
    }
}