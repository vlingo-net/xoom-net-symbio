// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Symbio.Store.Dispatch;
using Vlingo.Xoom.Symbio.Store.State;
using Vlingo.Xoom.Symbio.Store.State.InMemory;
using Vlingo.Xoom.Actors.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Symbio.Tests.Store.State.InMemory
{
    public class InMemoryStateStoreEntryReaderActorTest
    {
        private const string Id1 = "123-A";
        private const string Id2 = "123-B";
        private const string Id3 = "123-C";

        private readonly MockStateStoreDispatcher<TextState> _dispatcher;
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly MockStateStoreResultInterest _interest;
        private readonly IStateStoreEntryReader _reader;
        private readonly IStateStore _store;

        [Fact]
        public void TestThatEntryReaderReadsOne()
        {
            var access = _interest.AfterCompleting<Entity1, Event>(3);
            _dispatcher.AfterCompleting(0);

            _store.Write(Id1, new Entity1(Id1, 10), 1, new List<Event> {new Event1()}, _interest);
            _store.Write(Id2, new Entity2(Id2, "20"), 1, new List<Event> {new Event2()}, _interest);
            _store.Write(Id3, new Entity1(Id3, 30), 1, new List<Event> {new Event3()}, _interest);

            Assert.Equal(new Event1(), access.ReadFrom<object>("sources"));
            Assert.Equal(new Event2(), access.ReadFrom<object>("sources"));
            Assert.Equal(new Event3(), access.ReadFrom<object>("sources"));

            var entry1 = _reader.ReadNext().Await();
            Assert.True(_entryAdapterProvider.AsEntry<Event, IEntry<string>>(new Event1(), 1, Metadata.NullMetadata()).WithId("0").Equals(entry1));
            var entry2 = _reader.ReadNext().Await();
            Assert.True(_entryAdapterProvider.AsEntry<Event, IEntry<string>>(new Event2(), 1, Metadata.NullMetadata()).WithId("1").Equals(entry2));
            var entry3 = _reader.ReadNext().Await();
            Assert.True(_entryAdapterProvider.AsEntry<Event, IEntry<string>>(new Event3(), 1, Metadata.NullMetadata()).WithId("2").Equals(entry3));

            _reader.Rewind();
            Assert.Equal(new List<IEntry> { entry1, entry2, entry3}, _reader.ReadNext(3).Await());
        }

        public InMemoryStateStoreEntryReaderActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            var testWorld = TestWorld.StartWithDefaults("test-store");
            var world = testWorld.World;

            _interest = new MockStateStoreResultInterest();
            _dispatcher = new MockStateStoreDispatcher<TextState>(_interest);

            var stateAdapterProvider = new StateAdapterProvider(world);
            _entryAdapterProvider = new EntryAdapterProvider(world);

            stateAdapterProvider.RegisterAdapter(new Entity1StateAdapter());
            // NOTE: No adapter registered for Entity2.class because it will use the default

            _store = world.ActorFor<IStateStore>(typeof(InMemoryStateStoreActor<TextState>), new List<IDispatcher> {_dispatcher});
            
            var completes = _store.EntryReader<IEntry<string>>("test");
            _reader = completes.Await();

            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity1).FullName, typeof(Entity1));
            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity2).FullName, typeof(Entity2));
        }
    }
    
    public abstract class Event : Source<Event>
    {
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Event1 : Event
    {
    }
    
    public class Event2 : Event
    {
    }
    
    public class Event3 : Event
    {
    }
}