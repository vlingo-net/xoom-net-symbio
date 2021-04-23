// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Symbio.Store;
using Vlingo.Symbio.Store.Dispatch;
using Vlingo.Symbio.Store.Object;
using Vlingo.Symbio.Tests.Store.Dispatch;
using Vlingo.Xoom.Actors;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Symbio.Tests.Store.Object.InMemory
{
    public class InMemoryObjectStoreActorTest
    {
        private readonly MockPersistResultInterest _persistInterest;
        private readonly MockQueryResultInterest _queryResultInterest;
        private readonly IObjectStore _objectStore;
        private readonly MockDispatcher _dispatcher;

        [Fact]
        public void TestThatObjectPersistsQueries()
        {
            _dispatcher.AfterCompleting(1);
            var persistAccess = _persistInterest.AfterCompleting(1);
            var person = new Person("Tom Jones", 85);
            var source = new Test1Source();
            _objectStore.Persist(StateSources<Person, Test1Source>.Of(person, source), _persistInterest);
            var persistSize = persistAccess.ReadFrom<int>("size");
            Assert.Equal(1, persistSize);
            Assert.Equal(person, persistAccess.ReadFrom<int ,object>("object", 0));
            
            var query = MapQueryExpression.Using<Person>("find", MapQueryExpression.Map("id", person.PersistenceId));

            var queryAccess = _queryResultInterest.AfterCompleting(1);
            _objectStore.QueryObject(query, _queryResultInterest, null);
            var querySize = queryAccess.ReadFrom<int>("size");
            Assert.Equal(1, querySize);
            Assert.Equal(person, queryAccess.ReadFrom<int ,object>("object", 0));

            Assert.Equal(1, _dispatcher.DispatchedCount());
            var dispatched = _dispatcher.GetDispatched()[0];
            ValidateDispatchedState(person, dispatched);

            var dispatchedEntries = dispatched.Entries;
            Assert.Single(dispatchedEntries);
            var entry = dispatchedEntries[0];
            Assert.NotNull(entry.Id);
            Assert.Equal(source.GetType().AssemblyQualifiedName, entry.TypeName);
            Assert.Equal(Metadata.NullMetadata(), entry.Metadata);
        }

        [Fact]
        public void TestThatMultiPersistQueryResolves()
        {
            _dispatcher.AfterCompleting(3);
            var persistAllAccess = _persistInterest.AfterCompleting(1);

            var person1 = new Person("Tom Jones", 78);
            var person2 = new Person("Dean Martin", 78);
            var person3 = new Person("Sally Struthers", 71);
            _objectStore.PersistAll(new List<StateSources<Person, Test1Source>>
            {
                StateSources<Person, Test1Source>.Of(person1),
                StateSources<Person, Test1Source>.Of(person2),
                StateSources<Person, Test1Source>.Of(person3)
            }, _persistInterest);
            var persistSize = persistAllAccess.ReadFrom<int>("size");
            Assert.Equal(3, persistSize);

            var queryAllAccess = _queryResultInterest.AfterCompleting(1);
            _objectStore.QueryAll(QueryExpression.Using<Person>("findAll"), _queryResultInterest, null);
            var querySize = queryAllAccess.ReadFrom<int>("size");
            Assert.Equal(3, querySize);
            Assert.Equal(person1, queryAllAccess.ReadFrom<int, object>("object", 0));
            Assert.Equal(person2, queryAllAccess.ReadFrom<int, object>("object", 1));
            Assert.Equal(person3, queryAllAccess.ReadFrom<int, object>("object", 2));

            Assert.Equal(3, _dispatcher.DispatchedCount());

            var dispatched = _dispatcher.GetDispatched()[0];
            ValidateDispatchedState(person1, dispatched);
            var dispatchedEntries = dispatched.Entries;
            Assert.Empty(dispatchedEntries);

            dispatched = _dispatcher.GetDispatched()[1];
            ValidateDispatchedState(person2, dispatched);
            dispatchedEntries = dispatched.Entries;
            Assert.Empty(dispatchedEntries);

            dispatched = _dispatcher.GetDispatched()[2];
            ValidateDispatchedState(person3, dispatched);
            dispatchedEntries = dispatched.Entries;
            Assert.Empty(dispatchedEntries);
        }
        
        public InMemoryObjectStoreActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            _persistInterest = new MockPersistResultInterest();
            _queryResultInterest = new MockQueryResultInterest();
            var world = World.StartWithDefaults("test-object-store");
            var entryAdapterProvider = new EntryAdapterProvider(world);
            entryAdapterProvider.RegisterAdapter(new Test1SourceAdapter());
    
            _dispatcher = new MockDispatcher(new MockConfirmDispatchedResultInterest());
            _objectStore = world.ActorFor<IObjectStore>(typeof(Vlingo.Symbio.Store.Object.InMemory.InMemoryObjectStoreActor<Test1Source>), _dispatcher);
        }
        
        private void ValidateDispatchedState(Person persistedObject, Dispatchable dispatched)
        {
            Assert.NotNull(dispatched);
            Assert.NotNull(dispatched.Id);

            Assert.NotNull(dispatched.State.Get());
            var state = dispatched.TypedState<State<string>>();
            Assert.Equal(persistedObject.PersistenceId.ToString(), state.Id);
            Assert.Equal(persistedObject.GetType().AssemblyQualifiedName, state.Type);
            Assert.Equal(Metadata.NullMetadata(), state.Metadata);
        }
    }
    
    public class Test1Source : Source<string>
    {
        private int _one = 1;

        public int One()
        {
            return _one;
        }
    }
    
    public class Test1SourceAdapter : EntryAdapter<Test1Source, IEntry<Test1Source>>
    {
        public override Test1Source FromEntry(IEntry<Test1Source> entry) => entry.EntryData;

        public override IEntry<Test1Source> ToEntry(Test1Source source, Metadata metadata) =>
            new ObjectEntry<Test1Source>(typeof(Test1Source), 1, source, metadata);

        public override IEntry<Test1Source> ToEntry(Test1Source source, int version, Metadata metadata)
            => new ObjectEntry<Test1Source>(typeof(Test1Source), 1, source, version, metadata);

        public override IEntry<Test1Source> ToEntry(Test1Source source, int version, string id, Metadata metadata)
            => new ObjectEntry<Test1Source>(id, typeof(Test1Source), 1, source, version, metadata);

        public override IEntry<Test1Source> ToEntry(Test1Source source, string id, Metadata metadata)=>
            new ObjectEntry<Test1Source>(id, typeof(Test1Source), 1, source, metadata);
    }
}