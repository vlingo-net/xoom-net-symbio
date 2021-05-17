// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Journal.InMemory
{
    public class InMemoryJournalReaderActor : Actor, IJournalReader
    {
        private readonly IJournalReader _reader;

        public InMemoryJournalReaderActor(IJournalReader reader) => _reader = reader;

        public void Close() => _reader.Close();

        public ICompletes<IEntry> ReadNext() => Completes().With(_reader.ReadNext().Outcome);

        public ICompletes<IEntry> ReadNext(string fromId) => Completes().With(_reader.ReadNext(fromId).Outcome);

        public ICompletes<IEnumerable<IEntry>> ReadNext(int maximumEntries) => Completes().With(_reader.ReadNext(maximumEntries).Outcome);

        public ICompletes<IEnumerable<IEntry>> ReadNext(string fromId, int maximumEntries) => Completes().With(_reader.ReadNext(fromId, maximumEntries).Outcome);

        public void Rewind() => _reader.Rewind();

        public ICompletes<string> SeekTo(string id) => Completes().With(_reader.SeekTo(id).Outcome);

        public ICompletes<string> Name => Completes().With(_reader.Name.Outcome);
        
        public ICompletes<long> Size => Completes().With(_reader.Size.Outcome);
        
        public string Beginning { get; } = EntryReader.Beginning;

        public string End { get; } = EntryReader.End;

        public string Query { get; } = EntryReader.Query;

        public int DefaultGapPreventionRetries { get; } = EntryReader.DefaultGapPreventionRetries;

        public long DefaultGapPreventionRetryInterval { get; } = EntryReader.DefaultGapPreventionRetryInterval;
    }
}