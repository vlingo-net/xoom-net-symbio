// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Symbio.Store;
using Vlingo.Xoom.Symbio.Store.State;
using Vlingo.Xoom.Symbio.Store.State.InMemory;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Actors.TestKit;
using Xunit;
using Xunit.Abstractions;
using IDispatcher = Vlingo.Xoom.Symbio.Store.Dispatch.IDispatcher;

namespace Vlingo.Xoom.Symbio.Tests.Store.State.InMemory
{
    public class InMemoryStateStoreTest : IDisposable
    {
        private readonly string _storeName1 = typeof(Entity1).FullName;
        private readonly string _storeName2 = typeof(Entity2).FullName;
        
        private readonly MockStateStoreDispatcher<TextState> _dispatcher;
        private readonly MockStateStoreResultInterest _interest;
        private readonly IStateStore _store;
        private readonly World _world;

        [Fact]
        public void TestThatStateStoreWritesText()
        {
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(1);
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
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(2);
            _dispatcher.AfterCompleting(2);

            var entity = new Entity1("123", 5);

            _store.Write(entity.Id, entity, 1, _interest);
            _store.Read<Entity1>(entity.Id, _interest);

            Assert.Equal(1, access1.ReadFrom<int>("readObjectResultedIn"));
            Assert.Equal(1, access1.ReadFrom<int>("writeObjectResultedIn"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectReadResult"));
            Assert.Equal(entity, access1.ReadFrom<Entity1>("objectState"));
            
            var readEntity = access1.ReadFrom<Entity1>("objectState");

            Assert.Equal("123", readEntity.Id);
            Assert.Equal(5, readEntity.Value);
        }
        
        [Fact]
        public void TestThatStateStoreWritesAndReadsMetadataValue()
        {
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(2);
            _dispatcher.AfterCompleting(2);

            var entity = new Entity1("123", 5);
            var sourceMetadata = Metadata.WithValue("value");

            _store.Write(entity.Id, entity, 1, sourceMetadata, _interest);
            _store.Read<Entity1>(entity.Id, _interest);

            Assert.Equal(1, access1.ReadFrom<int>("readObjectResultedIn"));
            Assert.Equal(1, access1.ReadFrom<int>("writeObjectResultedIn"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectReadResult"));
            Assert.Equal(entity, access1.ReadFrom<Entity1>("objectState"));
            Assert.NotNull(access1.ReadFrom<Metadata>("metadataHolder"));
            var metadata = access1.ReadFrom<Metadata>("metadataHolder");
            Assert.True(metadata.HasValue);
            Assert.Equal("value", metadata.Value);
            
            var readEntity = access1.ReadFrom<Entity1>("objectState");

            Assert.Equal("123", readEntity.Id);
            Assert.Equal(5, readEntity.Value);
        }
        
        [Fact]
        public void TestThatStateStoreWritesAndReadsMetadataOperation()
        {
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(2);
            _dispatcher.AfterCompleting(2);

            var entity = new Entity1("123", 5);
            var sourceMetadata = Metadata.With("value", "operation");

            _store.Write(entity.Id, entity, 1, sourceMetadata, _interest);
            _store.Read<Entity1>(entity.Id, _interest);

            Assert.Equal(1, access1.ReadFrom<int>("readObjectResultedIn"));
            Assert.Equal(1, access1.ReadFrom<int>("writeObjectResultedIn"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectReadResult"));
            Assert.Equal(entity, access1.ReadFrom<object>("objectState"));
            Assert.NotNull(access1.ReadFrom<Metadata>("metadataHolder"));
            var metadata = access1.ReadFrom<Metadata>("metadataHolder");
            Assert.True(metadata.HasOperation);
            Assert.Equal("operation", metadata.Operation);
            
            var readEntity = access1.ReadFrom<Entity1>("objectState");

            Assert.Equal("123", readEntity.Id);
            Assert.Equal(5, readEntity.Value);
        }
        
        [Fact]
        public void TestThatConcurrencyViolationsDetected()
        {
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(2);
            _dispatcher.AfterCompleting(2);

            var entity = new Entity1("123", 5);

            _store.Write(entity.Id, entity, 1, _interest);
            _store.Write(entity.Id, entity, 2, _interest);

            Assert.Equal(2, access1.ReadFrom<int>("objectWriteAccumulatedResultsCount"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectWriteAccumulatedResults"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectWriteAccumulatedResults"));
            Assert.Equal(0, access1.ReadFrom<int>("objectWriteAccumulatedResultsCount"));
            
            var access2 = _interest.AfterCompleting<Entity1, Entity1>(3);
            _dispatcher.AfterCompleting(3);

            _store.Write(entity.Id, entity, 1, _interest);
            _store.Write(entity.Id, entity, 2, _interest);
            _store.Write(entity.Id, entity, 3, _interest);

            Assert.Equal(3, access2.ReadFrom<int>("objectWriteAccumulatedResultsCount"));
            Assert.Equal(Result.ConcurrencyViolation, access2.ReadFrom<Result>("objectWriteAccumulatedResults"));
            Assert.Equal(Result.ConcurrencyViolation, access2.ReadFrom<Result>("objectWriteAccumulatedResults"));
            Assert.Equal(Result.Success, access2.ReadFrom<Result>("objectWriteAccumulatedResults"));
        }

        [Fact]
        public void TestThatStateStoreDispatches()
        {
            _interest.AfterCompleting<Entity1, Entity1>(3);
            var accessDispatcher = _dispatcher.AfterCompleting(3);

            var entity1 = new Entity1("123", 1);
            _store.Write(entity1.Id, entity1, 1, _interest);
            var entity2 = new Entity1("234", 2);
            _store.Write(entity2.Id, entity2, 1, _interest);
            var entity3 = new Entity1("345", 3);
            _store.Write(entity3.Id, entity3, 1, _interest);

            Assert.Equal(3, accessDispatcher.ReadFrom<int>("dispatchedStateCount"));
            var state123 = accessDispatcher.ReadFrom<string, State<string>>("dispatchedState", DispatchId("123"));
            Assert.Equal("123", state123.Id);
            var state234 = accessDispatcher.ReadFrom<string, State<string>>("dispatchedState", DispatchId("234"));
            Assert.Equal("234", state234.Id);
            var state345 = accessDispatcher.ReadFrom<string, State<string>>("dispatchedState", DispatchId("345"));
            Assert.Equal("345", state345.Id);

            _interest.AfterCompleting<Entity1, Entity1>(4);
            var accessDispatcher1 = _dispatcher.AfterCompleting(4);

            accessDispatcher1.WriteUsing("processDispatch", false);
            var entity4 = new Entity1("456", 4);
            _store.Write(entity4.Id, entity4, 1, _interest);
            var entity5 = new Entity1("567", 5);
            _store.Write(entity5.Id, entity5, 1, _interest);

            accessDispatcher1.WriteUsing("processDispatch", true);
            _dispatcher.DispatchUnconfirmed();
            accessDispatcher1.ReadFrom<int>("dispatchedStateCount");

            Assert.Equal(5, accessDispatcher1.ReadFrom<int>("dispatchedStateCount"));

            var state456 = accessDispatcher1.ReadFrom<string, State<string>>("dispatchedState", DispatchId("456"));
            Assert.Equal("456", state456.Id);
            var state567 = accessDispatcher1.ReadFrom<string, State<string>>("dispatchedState", DispatchId("567"));
            Assert.Equal("567", state567.Id);
        }

        [Fact]
        public void TestThatReadAllReadsAll()
        {
            var accessWrites = _interest.AfterCompleting<Entity1, Entity1>(3);

            var entity1 = new Entity1("123", 1);
            _store.Write(entity1.Id, entity1, 1, _interest);
            var entity2 = new Entity1("234", 2);
            _store.Write(entity2.Id, entity2, 1, _interest);
            var entity3 = new Entity1("345", 3);
            _store.Write(entity3.Id, entity3, 1, _interest);

            var totalWrites = accessWrites.ReadFrom<int>("objectWriteAccumulatedResultsCount");

            Assert.Equal(3, totalWrites);

            var accessReads = _interest.AfterCompleting<Entity1, Entity1>(3);

            var bundles = new List<TypedStateBundle>
            {
                new TypedStateBundle(entity1.Id, typeof(Entity1)),
                new TypedStateBundle(entity2.Id, typeof(Entity1)),
                new TypedStateBundle(entity3.Id, typeof(Entity1))
            };

            _store.ReadAll<Entity1>(bundles, _interest, null);

            var allStates = accessReads.ReadFrom<List<object>>("readAllStates").Cast<StoreData<Entity1>>().ToList();

            Assert.Equal(3, allStates.Count);
            var state123 = allStates[0].TypedState;
            Assert.Equal("123", state123.Id);
            Assert.Equal(1, state123.Value);
            var state234 = allStates[1].TypedState;
            Assert.Equal("234", state234.Id);
            Assert.Equal(2, state234.Value);
            var state345 = allStates[2].TypedState;
            Assert.Equal("345", state345.Id);
            Assert.Equal(3, state345.Value);
        }

        [Fact]
        public void TestThatReadErrorIsReported()
        {
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(2);
            _dispatcher.AfterCompleting(2);

            var entity = new Entity1("123", 1);
            _store.Write(entity.Id, entity, 1, _interest);
            _store.Read<Entity1>(null, _interest);

            Assert.Equal(1, access1.ReadFrom<int>("errorCausesCount"));
            var cause1 = access1.ReadFrom<Exception>("errorCauses");
            Assert.Equal("The id is null.", cause1.Message);
            var result1 = access1.ReadFrom<Result>("objectReadResult");
            Assert.True(result1 == Result.Error);
        }
        
        [Fact]
        public void TestThatWriteErrorIsReported()
        {
            var access1 = _interest.AfterCompleting<Entity1, Entity1>(1);
            _dispatcher.AfterCompleting(1);

            _store.Write<Entity1>(null, null, 0, _interest);

            Assert.Equal(1, access1.ReadFrom<int>("errorCausesCount"));
            var cause1 = access1.ReadFrom<Exception>("errorCauses");
            Assert.Equal("The state is null.", cause1.Message);
            var result1 = access1.ReadFrom<Result>("objectWriteAccumulatedResults");
            Assert.True(result1 == Result.Error);
            var objectState = access1.ReadFrom<object>("objectState");
            Assert.Null(objectState);
        }
        
        [Fact]
        public void TestThatStateStoreWritesTextWithDefaultAdapter()
        {
            var access1 = _interest.AfterCompleting<Entity2, Entity2>(1);
            _dispatcher.AfterCompleting(1);
            
            var entity = new Entity2("123", "5");

            _store.Write(entity.Id, entity, 1, _interest);

            Assert.Equal(0, access1.ReadFrom<int>("readObjectResultedIn"));
            Assert.Equal(1, access1.ReadFrom<int>("writeObjectResultedIn"));
            Assert.Equal(Result.Success, access1.ReadFrom<Result>("objectWriteResult"));
            Assert.Equal(entity, access1.ReadFrom<object>("objectState"));
        }

        public InMemoryStateStoreTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            var testWorld = TestWorld.StartWithDefaults("test-store");
            _world = testWorld.World;

            _interest = new MockStateStoreResultInterest();
            _dispatcher = new MockStateStoreDispatcher<TextState>(_interest);

            var stateAdapterProvider = new StateAdapterProvider(_world);
            new EntryAdapterProvider(_world);

            stateAdapterProvider.RegisterAdapter(new Entity1StateAdapter());
            // NOTE: No adapter registered for Entity2.class because it will use the default

            _store = _world.ActorFor<IStateStore>(typeof(InMemoryStateStoreActor<TextState, TextEntry>), new List<IDispatcher> {_dispatcher});

            StateTypeStateStoreMap.StateTypeToStoreName(_storeName1, typeof(Entity1));
            StateTypeStateStoreMap.StateTypeToStoreName(_storeName2, typeof(Entity2));
        }

        public void Dispose()
        {
            _world?.Terminate();
        }
        
        private string DispatchId(string entityId) => $"{_storeName1}:{entityId}";
    }
}