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
    ///     The partitioning based on id hashing, which is meant to read and write in parallel using more than one
    ///     connection. For reader operations, such as queries and streaming, the partitioning is based on smallest mailbox.
    /// </summary>
    /// <remarks>
    ///     WARNING: When utilizing the smallest mailbox (least busy reader) operations, it is important not request
    ///     additional operations of the same type
    /// </remarks>
    public class PartitioningStateStore<TEntry> : IStateStore<TEntry> where TEntry : IEntry
    {
        private static readonly int MinimumReaders = 5;
        private static readonly int MaximumReaders = 128;

        private static readonly int MinimumWriters = 3;
        private static readonly int MaxWriters = 128;

        private readonly Tuple<IStateStore<TEntry>, Actor>[] _readers;
        private readonly Tuple<IStateStore<TEntry>, Actor>[] _writers;

        private PartitioningStateStore(
            Stage stage,
            Type stateStoreActorType,
            int totalReaders,
            int totalWriters)
        {
            _readers = CreateStateStores(stage, stateStoreActorType,
                ActualTotal(totalReaders, MinimumReaders, MaximumReaders));

            _writers = CreateStateStores(stage, stateStoreActorType,
                ActualTotal(totalWriters, MinimumWriters, MaxWriters));
        }

        public ICompletes<IStateStoreEntryReader<TEntry>> EntryReader(string name) => ReaderOf(name).EntryReader(name);

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
        /// <returns>
        ///     <see cref="IStateStore{TEntry}" />
        /// </returns>
        public static IStateStore<TEntry> StateStoreUsing(
            Stage stage,
            Type stateStoreActorType,
            int totalReaders,
            int totalWriters) =>
            new PartitioningStateStore<TEntry>(stage, stateStoreActorType, totalReaders, totalWriters);

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
                var pending = Environment.Of(_readers[idx].Item2).PendingMessages;

                if (pending < totalMessages)
                {
                    totalMessages = pending;
                    reader = _readers[idx].Item1;
                }
            }

            return reader;
        }

        private IStateStore<TEntry> ReaderOf(string identity) => _readers[identity.GetHashCode() / _readers.Length].Item1;

        private IStateStore<TEntry> WriterOf(string identity) => _writers[identity.GetHashCode() / _readers.Length].Item1;

        private Tuple<IStateStore<TEntry>, Actor>[] CreateStateStores(
            Stage stage,
            Type stateStoreActorType,
            int total)
        {
            var stateStores = new Tuple<IStateStore<TEntry>, Actor>[total];
            for (var idx = 0; idx < total; ++idx)
            {
                var stateStore = stage.ActorFor<IStateStore<TEntry>>(stateStoreActorType);
                var actor = (Actor) stateStore;
                stateStores[idx] = new Tuple<IStateStore<TEntry>, Actor>(stateStore, actor);
            }

            return stateStores;
        }
    }
}