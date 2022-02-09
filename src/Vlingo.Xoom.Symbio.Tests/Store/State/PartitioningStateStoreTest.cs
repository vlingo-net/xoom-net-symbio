// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Symbio.Store.State;
using Vlingo.Xoom.Actors;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Symbio.Tests.Store.State
{
    public class PartitioningStateStoreTest
    {
        private readonly World _world;

        public PartitioningStateStoreTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);

            _world = World.StartWithDefaults("test-partitioning-statestore");
        }

        [Fact]
        public void TestThatMinimumStoresCreated()
        {
            var readers = 3;
            var writers = 2;
            var times = PartitioningStateStore.MinimumReaders + PartitioningStateStore.MinimumWriters;

            var results = new MessageCountingResults(times);

            // 3, 2 must be minimum MinimumReaders, MinimumWriters and must default to that if below minimum is given
            var store = (PartitioningStateStore) PartitioningStateStore.Using(_world.Stage, typeof(MessageCountingStateStoreActor), readers, writers, results);
            for (var i = 0; i < store.ReadersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Reader);
            }
            for (var i = 0; i < store.WritersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Writer);
            }
            
            Assert.NotNull(store);
            Assert.Equal(times, results.GetCtor());
            Assert.NotEqual(readers, results.GetReaderCtor());
            Assert.Equal(PartitioningStateStore.MinimumReaders, results.GetReaderCtor());
            Assert.NotEqual(writers, results.GetWriterCtor());
            Assert.Equal(PartitioningStateStore.MinimumWriters, results.GetWriterCtor());
        }

        [Fact]
        public void TestThatMaximumStoresCreated()
        {
            var readers = int.MaxValue / 2;
            var writers = int.MaxValue / 10;
            var times = PartitioningStateStore.MaximumReaders + PartitioningStateStore.MaximumWriters;

            var results = new MessageCountingResults(times);

            // 3, 2 must be minimum 5, 3 and must default to that below minimum given
            var store = (PartitioningStateStore) PartitioningStateStore.Using(_world.Stage, typeof(MessageCountingStateStoreActor), readers, writers, results);
            for (var i = 0; i < store.ReadersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Reader);
            }
            for (var i = 0; i < store.WritersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Writer);
            }
            Assert.NotNull(store);
            Assert.Equal(times, results.GetCtor());
            Assert.NotEqual(readers, results.GetReaderCtor());
            Assert.Equal(PartitioningStateStore.MaximumReaders, results.GetReaderCtor());
            Assert.NotEqual(writers, results.GetWriterCtor());
            Assert.Equal(PartitioningStateStore.MaximumWriters, results.GetWriterCtor());
        }

        [Fact]
        public void TestThatExactStoresCreated()
        {
            var readers = 10;
            var writers = 7;
            var times = readers + writers;

            var results = new MessageCountingResults(times);

            // must be exact
            var store = (PartitioningStateStore) PartitioningStateStore.Using(_world.Stage, typeof(MessageCountingStateStoreActor), readers, writers, results);
            for (var i = 0; i < store.ReadersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Reader);
            }
            for (var i = 0; i < store.WritersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Writer);
            }
            
            Assert.NotNull(store);
            Assert.Equal(times, results.GetCtor());
            Assert.Equal(readers, results.GetReaderCtor());
            Assert.Equal(writers, results.GetWriterCtor());
        }

        [Fact]
        public void TestThatReadersReceive()
        {
            var ctors = PartitioningStateStore.MinimumReaders + PartitioningStateStore.MinimumWriters;
            var interations = 5;
            var partitionReads = 2;
            var reads = interations * partitionReads;
            var readAlls = 1;

            var times = ctors + reads + readAlls;

            var results = new MessageCountingResults(times);

            var store = (PartitioningStateStore) PartitioningStateStore.Using(_world.Stage, typeof(MessageCountingStateStoreActor), 0, 0, results);
            for (var i = 0; i < store.ReadersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Reader);
            }
            for (var i = 0; i < store.WritersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Writer);
            }
            
            Assert.NotNull(store);

            for (var read = 0; read < interations; ++read)
            {
                store.Read<TextEntry>(IdFor(read, PartitioningStateStore.MinimumReaders), null!, null);
                store.Read<TextEntry>(IdFor(read, PartitioningStateStore.MinimumReaders), null!, null);
            }

            store.ReadAll<TextEntry>(new []{new TypedStateBundle("3", typeof(TextEntry)), new TypedStateBundle("4", typeof(TextEntry))}, null!, null);

            Assert.Equal(ctors, results.GetCtor());
            Assert.Equal(PartitioningStateStore.MinimumReaders, results.GetReaderCtor());
            Assert.Equal(PartitioningStateStore.MinimumWriters, results.GetWriterCtor());

            Assert.Equal(reads, results.GetRead());
            Assert.Equal(readAlls, results.GetReadAll());

            Assert.Equal(partitionReads, results.GetReadPartitionCount(0));
            Assert.Equal(partitionReads, results.GetReadPartitionCount(1));
            Assert.Equal(partitionReads, results.GetReadPartitionCount(2));
            Assert.Equal(partitionReads, results.GetReadPartitionCount(3));
            Assert.Equal(partitionReads, results.GetReadPartitionCount(4));
        }

        [Fact]
        public void TestThatWritersReceive()
        {
            var iterations = 3;

            var ctors = PartitioningStateStore.MinimumReaders + PartitioningStateStore.MinimumWriters;
            var writes = 3;

            var times = ctors + writes * iterations;

            var results = new MessageCountingResults(times);

            var store = (PartitioningStateStore) PartitioningStateStore.Using(_world.Stage, typeof(MessageCountingStateStoreActor), 0, 0, results);
            for (var i = 0; i < store.ReadersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Reader);
            }
            for (var i = 0; i < store.WritersCount; i++)
            {
                results.IncrementCtor(InstantiationType.Writer);
            }
            
            Assert.NotNull(store);

            for (var outer = 0; outer < iterations; ++outer)
            for (var inner = 0; inner < writes; ++inner)
                store.Write(IdFor(inner, PartitioningStateStore.MinimumWriters), this, 1, null!);

            Assert.Equal(ctors, results.GetCtor());
            Assert.Equal(PartitioningStateStore.MinimumReaders, results.GetReaderCtor());
            Assert.Equal(PartitioningStateStore.MinimumWriters, results.GetWriterCtor());

            Assert.Equal(iterations * writes, results.GetWrite());

            Assert.Equal(writes, results.GetWritePartitionCount(0));
            Assert.Equal(writes, results.GetWritePartitionCount(1));
            Assert.Equal(writes, results.GetWritePartitionCount(2));
        }

        private string IdFor(int targetPartition, int max)
        {
            while (true)
            {
                var id = Guid.NewGuid().ToString();
                var hashCode = id.GetHashCode();
                var partition = hashCode % max;

                if (partition == targetPartition) return id;
            }
        }
    }
}