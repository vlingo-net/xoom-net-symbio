// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Streams;

namespace Vlingo.Xoom.Symbio.Store.Journal.InMemory
{
    public class InMemoryJournalReader : IJournalReader
    {
        private int _currentIndex;
        private readonly IList<IEntry> _journalView;
        private readonly string _name;

        public InMemoryJournalReader(IList<IEntry> journalView, string name)
        {
            _journalView = journalView;
            _name = name;
            _currentIndex = 0;
        }

        public void Close() => _journalView.Clear();

        public ICompletes<IEntry> ReadNext()
        {
            if (_currentIndex < _journalView.Count)
            {
                return Completes.WithSuccess(_journalView[_currentIndex++]);
            }
            return null!;
        }

        public ICompletes<IEntry> ReadNext(string fromId)
        {
            SeekTo(fromId);
            return ReadNext();
        }

        public ICompletes<IEnumerable<IEntry>> ReadNext(int maximumEntries)
        {
            var entries = new List<IEntry>();

            for (int count = 0; count < maximumEntries; ++count)
            {
                if (_currentIndex < _journalView.Count)
                {
                    entries.Add(_journalView[_currentIndex++]);
                }
                else
                {
                    break;
                }
            }
            return Completes.WithSuccess(entries.AsEnumerable());
        }

        public ICompletes<IEnumerable<IEntry>> ReadNext(string fromId, int maximumEntries)
        {
            SeekTo(fromId);
            return ReadNext(maximumEntries);
        }

        public void Rewind() => _currentIndex = 0;

        public ICompletes<string> SeekTo(string id)
        {
            string currentId;

            switch (id)
            {
                case EntryReader.Beginning:
                    Rewind();
                    currentId = ReadCurrentId();
                    break;
                case EntryReader.End:
                    EndInternal();
                    currentId = ReadCurrentId();
                    break;
                case EntryReader.Query:
                    currentId = ReadCurrentId();
                    break;
                default:
                    To(id);
                    currentId = ReadCurrentId();
                    break;
            }

            return Completes.WithSuccess(currentId);
        }
        
        public ICompletes<string> Name => Completes.WithSuccess(_name);

        public ICompletes<long> Size => Completes.WithSuccess<long>(_journalView.Count);

        public ICompletes<IStream> StreamAll() => null!; // provided by InMemoryJournalReaderActor

        public string Beginning { get; } = EntryReader.Beginning;

        public string End { get; } = EntryReader.End;

        public string Query { get; } = EntryReader.Query;

        public int DefaultGapPreventionRetries { get; } = EntryReader.DefaultGapPreventionRetries;

        public long DefaultGapPreventionRetryInterval { get; } = EntryReader.DefaultGapPreventionRetryInterval;
        
        private void EndInternal() => _currentIndex = _journalView.Count;

        private string ReadCurrentId()
        {
            if (_currentIndex < _journalView.Count)
            {
                var currentId = _journalView[_currentIndex].Id;
                return currentId;
            }
            
            return "-1";
        }

        private void To(string id)
        {
            Rewind();
            while (_currentIndex < _journalView.Count)
            {
                var entry = _journalView[_currentIndex];
                if (entry.Id.Equals(id))
                {
                    return;
                }
                ++_currentIndex;
            }
        }
    }
}