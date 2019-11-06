// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Actors;
using Vlingo.Actors.TestKit;
using Vlingo.Symbio.Store;
using Vlingo.Symbio.Store.State;
using Vlingo.Symbio.Store.State.InMemory;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Symbio.Tests.Store.State.InMemory
{
    public class InMemoryStateStoreTest : IDisposable
    {
        private readonly string _storeName1 = typeof(Entity1).FullName;
        private readonly string _storeName2 = typeof(Entity2).FullName;
        
        private readonly MockStateStoreDispatcher<Entity1, string> _dispatcher;
        private readonly MockStateStoreResultInterest<Entity1> _interest;
        private readonly IStateStore<Entity1, Entity1> _store;
        private readonly World _world;

        [Fact]
        public void TestThatStateStoreWritesText()
        {
            var access1 = _interest.AfterCompleting(1);
            _dispatcher.AfterCompleting(1);

            var entity = new Entity1("123", 5);

            _store.Write(entity.Id, entity, 1, _interest);

            Assert.Equal(0, access1.ReadFrom<int>("readObjectResultedIn"));
            Assert.Equal(1, access1.ReadFrom<int>("writeObjectResultedIn"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectWriteResult"));
            Assert.Equal(entity, access1.ReadFrom<Entity1>("objectState"));
        }
        
        [Fact]
        public void TestThatStateStoreWritesAndReadsObject()
        {
            var access1 = _interest.AfterCompleting(2);
            _dispatcher.AfterCompleting(2);

            var entity = new Entity1("123", 5);

            _store.Write(entity.Id, entity, 1, _interest);
            _store.Read(entity.Id, _interest);

            Assert.Equal(1, access1.ReadFrom<int>("readObjectResultedIn"));
            Assert.Equal(1, access1.ReadFrom<int>("writeObjectResultedIn"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectReadResult"));
            Assert.Equal(entity, access1.ReadFrom<Entity1>("objectState"));
            
            var readEntity = access1.ReadFrom<Entity1>("objectState");

            Assert.Equal("123", readEntity.Id);
            Assert.Equal(5, readEntity.Value);
        }

        public InMemoryStateStoreTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            var testWorld = TestWorld.StartWithDefaults("test-store");
            _world = testWorld.World;

            _interest = new MockStateStoreResultInterest<Entity1>();
            _dispatcher = new MockStateStoreDispatcher<Entity1, string>(_interest);

            var stateAdapterProvider = new StateAdapterProvider(_world);
            new EntryAdapterProvider(_world);

            stateAdapterProvider.RegisterAdapter(new Entity1StateAdapter());
            // NOTE: No adapter registered for Entity2.class because it will use the default

            _store = _world.ActorFor<IStateStore<Entity1, Entity1>>(typeof(InMemoryStateStoreActor<Entity1, string, Entity1>), _dispatcher);

            StateTypeStateStoreMap.StateTypeToStoreName(_storeName1, typeof(Entity1));
            StateTypeStateStoreMap.StateTypeToStoreName(_storeName2, typeof(Entity2));
        }

        public void Dispose()
        {
            _world?.Terminate();
        }
    }
}