// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common.Serialization;
using Vlingo.Xoom.Symbio.Store;
using Vlingo.Xoom.Symbio.Store.Journal;
using Vlingo.Xoom.Symbio.Store.Journal.InMemory;
using Vlingo.Xoom.Symbio.Tests.Store.Dispatch;
using Vlingo.Xoom.Symbio.Tests.Store.State;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Actors.TestKit;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Symbio.Tests.Store.Journal.InMemory
{
    public class InMemoryEventJournalActorTest : IDisposable
    {
        private readonly object _object = new object();
        private readonly IJournal<string> _journal;
        private readonly World _world;
        private readonly MockDispatcher _dispatcher;

        [Fact]
        public void TestThatJournalAppendsOneEvent()
        {
            var interest = new MockAppendResultInterest<Test1Source, SnapshotState>();
            _dispatcher.AfterCompleting(1);
            interest.AfterCompleting(1);

            var source = new Test1Source();
            var streamName = "123";
            var streamVersion = 1;
            _journal.Append(streamName, streamVersion, source, interest, _object);

            Assert.Equal(1, interest.ReceivedAppendsSize);

            var entries = interest.Entries;
            var journalData = entries.First();
            Assert.NotNull(journalData);
            Assert.Equal(streamName, journalData.StreamName);
            Assert.Equal(streamVersion, journalData.StreamVersion);
            Assert.Equal(Result.Success, journalData.Result);
            Assert.False(journalData.Snapshot.IsPresent);

            var sourceList = journalData.Sources;
            Assert.Single(sourceList);
            Assert.Equal(source, sourceList.First());

            Assert.Equal(1, _dispatcher.DispatchedCount());
            var dispatched = _dispatcher.GetDispatched()[0];

            Assert.NotEqual(new DateTimeOffset(),  dispatched.CreatedOn);
            Assert.False(dispatched.State.IsPresent);
            Assert.NotNull(dispatched.Id);
            var dispatchedEntries = dispatched.Entries;
            Assert.Single(dispatchedEntries);
        }
        
        [Fact]
        public void TestThatJournalAppendsOneEventWithSnapshot()
        {
            var interest = new MockAppendResultInterest<Test1Source, SnapshotState>();
            _dispatcher.AfterCompleting(1);
            interest.AfterCompleting(1);

            var source = new Test1Source();
            var streamName = "123";
            var streamVersion = 1;
            
            _journal.AppendWith(streamName, streamVersion, new Test1Source(), new SnapshotState(),  interest, _object);

            var entries = interest.Entries;
            var journalData = entries.First();
            Assert.NotNull(journalData);
            Assert.Equal(streamName, journalData.StreamName);
            Assert.Equal(streamVersion, journalData.StreamVersion);
            Assert.Equal(Result.Success, journalData.Result);
            Assert.True(journalData.Snapshot.IsPresent);

            var sourceList = journalData.Sources;
            Assert.Single(sourceList);
            Assert.Equal(source, sourceList.First());

            Assert.Equal(1, _dispatcher.DispatchedCount());
            var dispatched = _dispatcher.GetDispatched()[0];

            Assert.NotEqual(new DateTimeOffset(),  dispatched.CreatedOn);
            Assert.True(dispatched.State.IsPresent);
            Assert.NotNull(dispatched.Id);
            var dispatchedEntries = dispatched.Entries;
            Assert.Single(dispatchedEntries);
        }

        [Fact]
        public void TestThatJournalReaderReadsOneEvent()
        {
            var interest = new MockAppendResultInterest<Test1Source, SnapshotState>();
            _dispatcher.AfterCompleting(1);
            interest.AfterCompleting(1);

            var source = new Test1Source();
            var streamName = "123";
            var streamVersion = 1;
            
            _journal.Append(streamName, streamVersion, source,  interest, _object);

            var accessResults = new TestResults().AfterCompleting(1);
            _journal.JournalReader("test")
                .AndThenTo(reader => reader.ReadNext()
                    .AndThenConsume(@event => {
                        accessResults.WriteUsing("addAll", new List<IEntry> {@event});
            }));

            Assert.NotNull(accessResults.ReadFrom<int, IEntry>("entry", 0));
            Assert.Equal("1", accessResults.ReadFrom<int, string>("entryId", 0));
        }
        
        [Fact]
        public void TestThatJournalReaderReadsThreeEvents()
        {
            var interest = new MockAppendResultInterest<Test1Source, SnapshotState>();
            _dispatcher.AfterCompleting(1);
            interest.AfterCompleting(1);

            var three = new List<Source<string>> { new Test1Source(), new Test2Source(), new Test1Source() };
            _journal.AppendAll<Source<string>>("123", 1, three, interest, _object);

            var accessResults = new TestResults().AfterCompleting(1);
            _journal.JournalReader("test")
                .AndThenTo(reader => reader.ReadNext(5)
                    .AndThenConsume(entries => {
                        accessResults.WriteUsing("addAll", entries.Select(entry => (IEntry)entry).ToList());
            }));

            Assert.Equal(3, accessResults.ReadFrom<int>("size"));
            Assert.Equal("1", accessResults.ReadFrom<int, string>("entryId", 0));
            Assert.Equal("2", accessResults.ReadFrom<int, string>("entryId", 1));
            Assert.Equal("3", accessResults.ReadFrom<int, string>("entryId", 2));
        }
        
        [Fact]
        public void TestThatStreamReaderReadsFiveEventsWithSnapshot()
        {
            var interest = new MockAppendResultInterest<Test1Source, SnapshotState>();
            _dispatcher.AfterCompleting(1);
            interest.AfterCompleting(1);

            _journal.Append("123", 1, new Test1Source(), interest, _object);
            _journal.Append("123", 2, new Test1Source(), interest, _object);
            _journal.AppendWith("123", 3, new Test1Source(), new SnapshotState(), interest, _object);
            _journal.Append("123", 4, new Test1Source(), interest, _object);
            _journal.Append("123", 5, new Test1Source(), interest, _object);

            var accessResults = new TestResults().AfterCompleting(1);
            _journal.StreamReader("test")
                .AndThenTo(reader => reader.StreamFor("123")
                    .AndThenConsume(eventStream => {
                        accessResults.WriteUsing("addAll", eventStream.Entries.Select(entry => (IEntry)entry).ToList());
                    }));

            Assert.Equal(3, accessResults.ReadFrom<int>("size"));
            Assert.Equal("3", accessResults.ReadFrom<int, string>("entryId", 0));
            Assert.Equal("4", accessResults.ReadFrom<int, string>("entryId", 1));
            Assert.Equal("5", accessResults.ReadFrom<int, string>("entryId", 2));
        }
        
        [Fact]
        public void TestThatStreamReaderReadsFromBeyondSnapshot()
        {
            var interest = new MockAppendResultInterest<Test1Source, SnapshotState>();
            _dispatcher.AfterCompleting(5);
            interest.AfterCompleting(5);

            _journal.Append("123", 1, new Test1Source(), interest, _object);
            _journal.Append("123", 2, new Test1Source(), interest, _object);
            _journal.AppendWith("123", 3, new Test1Source(), new SnapshotState(), interest, _object);
            _journal.Append("123", 4, new Test1Source(), interest, _object);
            _journal.Append("123", 5, new Test1Source(), interest, _object);

            var accessResults = new TestResults().AfterCompleting(1);
            _journal.StreamReader("test")
                .AndThenTo(reader => reader.StreamFor("123", 4)
                    .AndThenConsume(eventStream => {
                        accessResults.WriteUsing("addAll", eventStream.Entries.Select(entry => (IEntry)entry).ToList());
                        Assert.Null(eventStream.Snapshot);
                    }));

            Assert.Equal(2, accessResults.ReadFrom<int>("size"));
            Assert.Equal("4", accessResults.ReadFrom<int, string>("entryId", 0));
            Assert.Equal("5", accessResults.ReadFrom<int, string>("entryId", 1));
        }
        
        public InMemoryEventJournalActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            _world = World.StartWithDefaults("test-journal");
            _dispatcher = new MockDispatcher(new MockConfirmDispatchedResultInterest());
            
            _journal = Journal<string>.Using<InMemoryJournalActor<string>>(_world.Stage, _dispatcher);
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
    
    internal class Test1SourceAdapter : EntryAdapter<Test1Source, IEntry>
    {
        public override Test1Source FromEntry(IEntry entry) => JsonSerialization.Deserialized<Test1Source>(entry.EntryRawData);
        
        public override IEntry ToEntry(Test1Source source, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(typeof(Test1Source), 1, serialization, metadata);
        }

        public override IEntry ToEntry(Test1Source source, int version, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(typeof(Test1Source), 1, serialization, version, metadata);
        }

        public override IEntry ToEntry(Test1Source source, int version, string id, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(id, typeof(Test1Source), 1, serialization, version, metadata);
        }

        public override IEntry ToEntry(Test1Source source, string id, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(id, typeof(Test1Source), 1, serialization, metadata);
        }
    }
    
    internal class Test2SourceAdapter : EntryAdapter<Test2Source, TextEntry>
    {
        public override Test2Source FromEntry(TextEntry entry) => JsonSerialization.Deserialized<Test2Source>(entry.EntryData);

        public override TextEntry ToEntry(Test2Source source, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(typeof(Test2Source), 1, serialization, metadata);
        }

        public override TextEntry ToEntry(Test2Source source, int version, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(typeof(Test2Source), 1, serialization, version, metadata);
        }

        public override TextEntry ToEntry(Test2Source source, int version, string id, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(id, typeof(Test2Source), 1, serialization, version, metadata);
        }

        public override TextEntry ToEntry(Test2Source source, string id, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(source);
            return new TextEntry(id, typeof(Test2Source), 1, serialization, metadata);
        }
    }

    internal class TestResults
    {
        private AccessSafely _access;
        
        internal List<IEntry> Entries { get; } = new List<IEntry>();
        
        internal AccessSafely AfterCompleting(int times)
        {
            _access = AccessSafely.AfterCompleting(times)
                .WritingWith<List<IEntry>>("addAll", values => Entries.AddRange(values))
                .ReadingWith<int, IEntry>("entry", index => Entries[index])
                .ReadingWith<int, string>("entryId", index => Entries[index].Id)
                .ReadingWith("size", () => Entries.Count);

            return _access;
        }
    }
}