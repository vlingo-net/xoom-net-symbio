// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Actors.TestKit;
using Vlingo.Symbio.Store.State;
using Vlingo.Symbio.Store.State.InMemory;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Symbio.Tests.Store.State.InMemory
{
    public class InMemoryStateStoreEntryReaderActorTest
    {
        private const string Id1 = "123-A";
        private const string Id2 = "123-B";
        private const string Id3 = "123-C";

        private readonly MockStateStoreDispatcher<TextEntry, string> _dispatcher;
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly MockStateStoreResultInterest _interest;
        private readonly IStateStoreEntryReader<TextEntry> _reader;
        private readonly IStateStore<TextEntry> _store;
        private readonly World _world;

        [Fact(Skip = "Fails")]
        public void TestThatEntryReaderReadsOne()
        {
            var access = _interest.AfterCompleting<Entity1, TextEntry>(3);
            _dispatcher.AfterCompleting(0);

            _store.Write(Id1, new Entity1(Id1, 10), 1, new List<Source<Event>> {new Event1()}, _interest);
            _store.Write(Id2, new Entity2(Id2, "20"), 1, new List<Source<Event>> {new Event2()}, _interest);
            _store.Write(Id3, new Entity1(Id3, 30), 1, new List<Source<Event>> {new Event3()}, _interest);

            Assert.Equal(new Event1(), access.ReadFrom<Event1>("sources"));
            Assert.Equal(new Event2(), access.ReadFrom<Event2>("sources"));
            Assert.Equal(new Event3(), access.ReadFrom<Event3>("sources"));

            var entry1 = _reader.ReadNext().Await();
            Assert.Equal(_entryAdapterProvider.AsEntry<Event, TextEntry>(new Event1(), Metadata.NullMetadata(), new DefaultTextEntryAdapter<Event>()).WithId("0"), entry1);
            var entry2 = _reader.ReadNext().Await();
            Assert.Equal(_entryAdapterProvider.AsEntry<Event, TextEntry>(new Event2(), Metadata.NullMetadata(), new DefaultTextEntryAdapter<Event>()).WithId("1"), entry2);
            var entry3 = _reader.ReadNext().Await();
            Assert.Equal(_entryAdapterProvider.AsEntry<Event, TextEntry>(new Event3(), Metadata.NullMetadata(), new DefaultTextEntryAdapter<Event>()).WithId("2"), entry3);

            _reader.Rewind();
            Assert.Equal(new List<IEntry<TextEntry>> { entry1, entry2, entry3}, _reader.ReadNext(3).Await());
        }

        public InMemoryStateStoreEntryReaderActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            var testWorld = TestWorld.StartWithDefaults("test-store");
            _world = testWorld.World;

            _interest = new MockStateStoreResultInterest();
            _dispatcher = new MockStateStoreDispatcher<TextEntry, string>(_interest);

            var stateAdapterProvider = new StateAdapterProvider(_world);
            _entryAdapterProvider = new EntryAdapterProvider(_world);

            stateAdapterProvider.RegisterAdapter(new Entity1StateAdapter());
            // NOTE: No adapter registered for Entity2.class because it will use the default

            _store = _world.ActorFor<IStateStore<TextEntry>>(typeof(InMemoryStateStoreActor<string, TextEntry>), _dispatcher);
            
            var completes = _store.EntryReader("test");
            _reader = completes.Await<IStateStoreEntryReader<TextEntry>>();

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