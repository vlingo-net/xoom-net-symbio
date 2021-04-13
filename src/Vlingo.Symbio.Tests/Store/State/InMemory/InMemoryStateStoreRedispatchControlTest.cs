// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading;
using Vlingo.Actors;
using Vlingo.Symbio.Store.State;
using Vlingo.Symbio.Store.State.InMemory;
using Xunit;
using Xunit.Abstractions;
using IDispatcher = Vlingo.Symbio.Store.Dispatch.IDispatcher;

namespace Vlingo.Symbio.Tests.Store.State.InMemory
{
    public class InMemoryStateStoreRedispatchControlTest : IDisposable
    {
        private static string _storeName = typeof(Entity1).FullName;

        private readonly MockStateStoreDispatcher<TextState> _dispatcher;
        private readonly MockStateStoreResultInterest _interest;
        private readonly IStateStore _store;
        private readonly World _world;

        [Fact]
        public void TestRedispatch()
        {
            var accessDispatcher = _dispatcher.AfterCompleting(3);
            
            var entity = new Entity1("123", 5);

            accessDispatcher.WriteUsing("processDispatch", false);
            _store.Write(entity.Id, entity, 1, _interest);

            try
            {
                Thread.Sleep(3000);
            }
            catch
            {
                // ignore
            }

            accessDispatcher.WriteUsing("processDispatch", true);
            
            accessDispatcher.ReadFromExpecting("dispatchedStateCount", 1);

            var dispatchedStateCount = accessDispatcher.ReadFrom<int>("dispatchedStateCount");
            Assert.True(dispatchedStateCount == 1, "dispatchedStateCount");

            var dispatchAttemptCount = accessDispatcher.ReadFrom<int>("dispatchAttemptCount");
            Assert.True(dispatchAttemptCount > 1, "dispatchAttemptCount");
        }

        public InMemoryStateStoreRedispatchControlTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            _world = World.StartWithDefaults("test-store");

            _interest = new MockStateStoreResultInterest();
            _interest.AfterCompleting<string, Entity1>(0);
            _dispatcher = new MockStateStoreDispatcher<TextState>(_interest);

            var stateAdapterProvider = new StateAdapterProvider(_world);
            new EntryAdapterProvider(_world);

            stateAdapterProvider.RegisterAdapter(new Entity1StateAdapter());
            // NOTE: No adapter registered for Entity2.class because it will use the default

            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity1).FullName, typeof(Entity1));
            
            _store = _world.ActorFor<IStateStore>(typeof(InMemoryStateStoreActor<TextState, IEntry<string>>), new List<IDispatcher> {_dispatcher});
        }

        public void Dispose()
        {
            _world.Terminate();
        }
    }
}