// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
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
        private static string _id1 = "123-A";
        private static string _id2 = "123-B";
        private static string _id3 = "123-C";
        
        private readonly MockStateStoreDispatcher<TextEntry, string> _dispatcher;
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly MockStateStoreResultInterest _interest;
        private readonly IStateStoreEntryReader<TextEntry> _reader;
        private readonly IStateStore<TextEntry> _store;
        private readonly World _world;

        [Fact]
        public void TestThatEntryReaderReadsOne()
        {
            
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
            
            // TODO: here we have the wrong returned type
            var completes = _store.EntryReader("test");
            _reader = completes.Await<IStateStoreEntryReader<TextEntry>>();

            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity1).FullName, typeof(Entity1));
            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity2).FullName, typeof(Entity2));
        }
    }
    
    public class Event : Source<Event>
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
}