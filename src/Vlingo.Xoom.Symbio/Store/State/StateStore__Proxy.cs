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

namespace Vlingo.Xoom.Symbio.Store.State;

public class StateStore__Proxy : IStateStore
{
    private const string EntryReaderRepresentation1 = "EntryReader(string)";

    private const string ReadRepresentation2 =
        "Read<TState>(string, Vlingo.Xoom.Symbio.Store.State.IReadResultInterest)";

    private const string ReadRepresentation3 =
        "Read<TState>(string, Vlingo.Xoom.Symbio.Store.State.IReadResultInterest, object)";
        
    private const string ReadRepresentation3A =
        "Read<TState>(IEnumerable<TypedStateBundle>, Vlingo.Xoom.Symbio.Store.State.IReadResultInterest, object)";

    private const string WriteRepresentation4 =
        "Write<TState>(string, TState, int, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest)";

    private const string WriteRepresentation5 =
        "Write<TState, TSource>(string, TState, int, IEnumerable<TSource>, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest)";

    private const string WriteRepresentation6 =
        "Write<TState>(string, TState, int, Vlingo.Xoom.Symbio.Metadata, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest)";

    private const string WriteRepresentation7 =
        "Write<TState, TSource>(string, TState, int, IEnumerable<TSource>, Vlingo.Xoom.Symbio.Metadata, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest)";

    private const string WriteRepresentation8 =
        "Write<TState>(string, TState, int, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest, object)";

    private const string WriteRepresentation9 =
        "Write<TState, TSource>(string, TState, int, IEnumerable<TSource>, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest, object)";

    private const string WriteRepresentation10 =
        "Write<TState>(string, TState, int, Vlingo.Xoom.Symbio.Metadata, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest, object)";

    private const string WriteRepresentation11 =
        "Write<TState, TSource>(string, TState, int, IEnumerable<TSource>, Vlingo.Xoom.Symbio.Metadata, Vlingo.Xoom.Symbio.Store.State.IWriteResultInterest, object)";

    private const string StreamAllOfRepresentation12 = "StreamAllOf()";
        
    private const string StreamSomeUsingRepresentation13 = "StreamSomeUsing(QueryExpression query)";
        
    private readonly Actor _actor;
    private readonly IMailbox _mailbox;

    public StateStore__Proxy(Actor actor, IMailbox mailbox)
    {
        _actor = actor;
        _mailbox = mailbox;
    }

    public ICompletes<IStateStoreEntryReader> EntryReader<TEntry>(string name) where TEntry : IEntry
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ => __.EntryReader<TEntry>(name);
            var completes = new BasicCompletes<IStateStoreEntryReader>(_actor.Scheduler);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, completes, EntryReaderRepresentation1);
            }
            else
            {
                _mailbox.Send(new LocalMessage<IStateStore>(_actor,
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

    internal Actor Actor => _actor;

    public void Read<TState>(string id, IReadResultInterest interest)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ => __.Read<TState>(id, interest);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, ReadRepresentation2);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
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
            Action<IStateStore> cons128873 = __ =>
                __.Read<TState>(id, interest, @object);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, ReadRepresentation3);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        ReadRepresentation3));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadRepresentation3));
        }
    }

    public void ReadAll<TState>(IEnumerable<TypedStateBundle> bundles, IReadResultInterest interest, object? @object)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ => __.ReadAll<TState>(bundles, interest, @object);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, ReadRepresentation3A);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873, ReadRepresentation3A));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReadRepresentation3A));
        }
    }

    public ICompletes<IStream> StreamAllOf<TState>()
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128880 = __ => __.StreamAllOf<TState>();
            var completes = new BasicCompletes<IStream>(_actor.Scheduler);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128880, completes, StreamAllOfRepresentation12);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128880, completes, StreamAllOfRepresentation12));
            }

            return completes;
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StreamAllOfRepresentation12));
        }

        return null!;
    }

    public ICompletes<IStream> StreamSomeUsing(QueryExpression query)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128881 = __ => __.StreamSomeUsing(query);
            var completes = new BasicCompletes<IStream>(_actor.Scheduler);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128881, completes, StreamSomeUsingRepresentation13);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128881, completes, StreamSomeUsingRepresentation13));
            }

            return completes;
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StreamSomeUsingRepresentation13));
        }

        return null!;
    }

    public void Write<TState>(string id, TState state, int stateVersion,
        IWriteResultInterest interest)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState>(id, state, stateVersion, interest);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation4);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        WriteRepresentation4));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation4));
        }
    }

    public void Write<TState, TSource>(string id, TState state, int stateVersion,
        IEnumerable<TSource> sources, IWriteResultInterest interest)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState, TSource>(id, state, stateVersion, sources, interest);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation5);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
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
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState>(id, state, stateVersion, metadata, interest);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation6);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        WriteRepresentation6));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation6));
        }
    }

    public void Write<TState, TSource>(string id, TState state, int stateVersion,
        IEnumerable<TSource> sources, Metadata metadata,
        IWriteResultInterest interest)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState, TSource>(id, state, stateVersion, sources, metadata, interest);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation7);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
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
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState>(id, state, stateVersion, interest, @object);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation8);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        WriteRepresentation8));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation8));
        }
    }

    public void Write<TState, TSource>(string id, TState state, int stateVersion,
        IEnumerable<TSource> sources, IWriteResultInterest interest,
        object @object)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState, TSource>(id, state, stateVersion, sources, interest, @object);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation9);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        WriteRepresentation9));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation9));
        }
    }

    public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest, object? @object)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState>(id, state, stateVersion, metadata, interest, @object);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation10);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        WriteRepresentation10));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation10));
        }
    }

    public void Write<TState, TSource>(string id, TState state, int stateVersion,
        IEnumerable<TSource> sources, Metadata metadata,
        IWriteResultInterest interest, object? @object)
    {
        if (!_actor.IsStopped)
        {
            Action<IStateStore> cons128873 = __ =>
                __.Write<TState, TSource>(id, state, stateVersion, sources, metadata, interest, @object);
            if (_mailbox.IsPreallocated)
            {
                _mailbox.Send(_actor, cons128873, null, WriteRepresentation11);
            }
            else
            {
                _mailbox.Send(
                    new LocalMessage<IStateStore>(_actor, cons128873,
                        WriteRepresentation11));
            }
        }
        else
        {
            _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, WriteRepresentation11));
        }
    }
}