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
using Environment = Vlingo.Actors.Environment;

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    ///     Provides a partitioning <see cref="IStateStore{TEntry}" />. All reads and writes are from/to the same storage and
    ///     tables.
    ///     The partitioning is based on id hashing, which is meant to read and write in parallel using more than one
    ///     connection. For reader operations not subject to id, such as queries and streaming, the partitioning is
    ///     based on smallest mailbox.
    /// </summary>
    /// <remarks>
    ///     WARNING: (1) When utilizing the smallest mailbox (least busy reader) operations, it is important to not request
    ///     additional operations of the same type if the earlier query must complete before the subsequent request.
    ///     (2) The underlying <see cref="Actor"/> must use a <see cref="IMailbox"/> that supports <code>int PendingMessages</code>. Otherwise,
    ///     the smallest mailbox (least busy reader) operations cannot be supported, and it does not make much sense to use
    ///     this a partitioning <see cref="IStateStore{TEntry}"/>.
    /// </remarks>
    public class PartitioningStateStore<TEntry> : IStateStore<TEntry> where TEntry : IEntry
    {
        public static readonly int MinimumReaders = 5;
        public static readonly int MaximumReaders = 256;

        public static readonly int MinimumWriters = 3;
        public static readonly int MaximumWriters = 256;

        private readonly Tuple<IStateStore<TEntry>, Actor>[] _readers;
        private readonly Tuple<IStateStore<TEntry>, Actor>[] _writers;

        public int ReadersCount => _readers.Length;
        public int WritersCount => _writers.Length;
        
        public static int PartitionOf(string identity, int totalPartitions) => identity.GetHashCode() % totalPartitions;

        private PartitioningStateStore(
            Stage stage,
            Type stateStoreActorType,
            int totalReaders,
            int totalWriters,
            object parameter)
        {
            _readers = CreateStateStores(stage, stateStoreActorType,
                ActualTotal(totalReaders, MinimumReaders, MaximumReaders), parameter);

            _writers = CreateStateStores(stage, stateStoreActorType,
                ActualTotal(totalWriters, MinimumWriters, MaximumWriters), parameter);
        }

        public ICompletes<IStateStoreEntryReader<TEntry>> EntryReader(string name) => ReaderOf(name).EntryReader(name);

        public Actor? Actor { get; } = null;

        public void Read<TState>(string id, IReadResultInterest interest) => Read<TState>(id, interest, null);

        public void Read<TState>(string id, IReadResultInterest interest, object? @object) => ReaderOf(id).Read<TState>(id, interest, @object);

        public void ReadAll<TState>(IEnumerable<TypedStateBundle> bundles, IReadResultInterest interest, object? @object)
            => LeastBusyReader()?.ReadAll<TState>(bundles, interest, @object);

        public void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest)
            => Write(id, state, stateVersion, Source<TState>.None(), Metadata.NullMetadata(), interest, null);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, IWriteResultInterest interest)
            => Write(id, state, stateVersion, sources, Metadata.NullMetadata(), interest, null);

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest)
            => Write(id, state, stateVersion, Source<TState>.None(), metadata, interest, null);


        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, Metadata metadata, IWriteResultInterest interest)
            => Write(id, state, stateVersion, sources, metadata, interest, null);

        public void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest, object @object)
            => Write(id, state, stateVersion, Source<TState>.None(), Metadata.NullMetadata(), interest, @object);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, IWriteResultInterest interest, object @object)
            => Write(id, state, stateVersion, sources, Metadata.NullMetadata(), interest, @object);

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest, object? @object)
            => Write(id, state, stateVersion, Source<TState>.None(), metadata, interest, @object);

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<Source<TSource>> sources, Metadata metadata, IWriteResultInterest interest, object? @object) =>
            WriterOf(id).Write(id, state, stateVersion, sources, metadata, interest, @object);

        /// <summary>
        ///     Gets a new <see cref="PartitioningStateStore{TEntry}" /> as a <see cref="IStateStore{TEntry}" /> with
        ///     <paramref name="totalReaders" /> and <paramref name="totalWriters" />.
        /// </summary>
        /// <param name="stage">the Stage within which the StateStore actors are created</param>
        /// <param name="stateStoreActorType">The type of the Actor that implements <see cref="IStateStore{TEntry}" /></param>
        /// <param name="totalReaders">
        ///     The int total number of readers, which may be between <code>MinimumReaders</code> and
        ///     <code>MaximumReaders</code>
        /// </param>
        /// <param name="totalWriters">
        ///     The int total number of writers, which may be between <code>MinimumReaders</code> and
        ///     <code>MaximumReaders</code>
        /// </param>
        /// <param name="parameter">Parameters for reader or writer actors</param>
        /// <returns>
        ///     <see cref="IStateStore{TEntry}" />
        /// </returns>
        public static IStateStore<TEntry> Using(
            Stage stage,
            Type stateStoreActorType,
            int totalReaders,
            int totalWriters,
            object parameter) =>
            new PartitioningStateStore<TEntry>(stage, stateStoreActorType, totalReaders, totalWriters, parameter);

        private int ActualTotal(int total, int minimum, int maximum)
        {
            if (total < minimum) return minimum;

            if (total > maximum) return maximum;

            return total;
        }

        private IStateStore<TEntry>? LeastBusyReader()
        {
            var totalMessages = int.MaxValue;
            IStateStore<TEntry>? reader = null;

            for (var idx = 0; idx < _readers.Length; ++idx)
            {
                var pending = Pending(_readers[idx].Item2);

                if (pending < totalMessages)
                {
                    totalMessages = pending;
                    reader = _readers[idx].Item1;
                }
            }

            return reader;
        }

        private IStateStore<TEntry> ReaderOf(string identity) => _readers[PartitionOf(identity, _readers.Length)].Item1;

        private IStateStore<TEntry> WriterOf(string identity) => _writers[PartitionOf(identity, _writers.Length)].Item1;

        private int Pending(Actor actor) => Environment.Of(actor).PendingMessages;

        private Tuple<IStateStore<TEntry>, Actor>[] CreateStateStores(
            Stage stage,
            Type stateStoreActorType,
            int total,
            object parameter)
        {
            var stateStores = new Tuple<IStateStore<TEntry>, Actor>[total];
            for (var idx = 0; idx < total; ++idx)
            {
                var stateStore = stage.ActorFor<IStateStore<TEntry>>(stateStoreActorType, parameter, total);
                Pending(stateStore.Actor!);
                stateStores[idx] = new Tuple<IStateStore<TEntry>, Actor>(stateStore, stateStore.Actor!);
            }

            return stateStores;
        }
    }
}