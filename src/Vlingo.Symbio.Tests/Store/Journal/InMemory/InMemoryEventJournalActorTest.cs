// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Actors.TestKit;
using Vlingo.Common.Serialization;
using Vlingo.Symbio.Store.Journal;
using Vlingo.Symbio.Store.Journal.InMemory;
using Vlingo.Symbio.Tests.Store.Dispatch;
using Vlingo.Symbio.Tests.Store.State;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Symbio.Tests.Store.Journal.InMemory
{
    public class InMemoryEventJournalActorTest : IDisposable
    {
        private object _object = new object();
        private MockAppendResultInterest<string, string> _interest = new MockAppendResultInterest<string, string>();
        private IJournal<string> _journal;
        private readonly World _world;
        private readonly MockDispatcher<string, string> _dispatcher;

        [Fact]
        public void TestThatJournalAppendsOneEvent()
        {
            
        }

        public InMemoryEventJournalActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            _world = World.StartWithDefaults("test-journal");
            _dispatcher = new MockDispatcher<string, string>(new MockConfirmDispatchedResultInterest());
            
            _journal = Journal<string>.Using<InMemoryJournalActor<string, string>, string>(_world.Stage, _dispatcher);
            EntryAdapterProvider.Instance(_world).RegisterAdapter(new Test1SourceAdapter());
            EntryAdapterProvider.Instance(_world).RegisterAdapter(new Test2SourceAdapter());
            StateAdapterProvider.Instance(_world).RegisterAdapter(new SnapshotStateAdapter());
        }
        
        public void Dispose() => _world.Terminate();
    }
    
    public class Test1Source : Source<string>
    {
        private int _one = 1;

        public int One => _one;
    }

    public class Test2Source : Source<string>
    {
        private int _two = 2;

        public int Two => _two;
    }
    
    internal class Test1SourceAdapter : EntryAdapter<string, string>
    {
        public override Source<string> FromEntry(IEntry<string> entry) => JsonSerialization.Deserialized<Test1Source>(entry.EntryData);

        public override IEntry<string> ToEntry(Source<string> source, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(typeof(Test1Source), 1, serialization, metadata);
        }

        public override IEntry<string> ToEntry(Source<string> source, string id, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(id, typeof(Test1Source), 1, serialization, metadata);
        }
    }
    
    internal class Test2SourceAdapter : EntryAdapter<string, string>
    {
        public override Source<string> FromEntry(IEntry<string> entry) => JsonSerialization.Deserialized<Test2Source>(entry.EntryData);

        public override IEntry<string> ToEntry(Source<string> source, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(typeof(Test2Source), 1, serialization, metadata);
        }

        public override IEntry<string> ToEntry(Source<string> source, string id, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(id, typeof(Test2Source), 1, serialization, metadata);
        }
    }

    internal class TestResults
    {
        private AccessSafely _access;
        internal List<BaseEntry<string>> Entries { get; } = new List<BaseEntry<string>>();
        
        internal AccessSafely AfterCompleting(int times)
        {
            _access = AccessSafely.AfterCompleting(times)
                .WritingWith<List<BaseEntry<string>>>("addAll", values => Entries.AddRange(values))
                .ReadingWith<int, BaseEntry<string>>("entry", index => Entries[index])
                .ReadingWith<int, string>("entryId", index => Entries[index].Id)
                .ReadingWith("size", () => Entries.Count);

            return _access;
        }
    }
}