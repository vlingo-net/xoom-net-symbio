// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Object.InMemory
{
    public class InMemoryObjectStoreEntryReaderActor : Actor, IObjectStoreEntryReader<string>
    {
        private int _currentIndex;
        private readonly List<IEntry<string>> _entriesView;
        private readonly string _name;

        public InMemoryObjectStoreEntryReaderActor(List<IEntry<string>> entriesView, string name)
        {
            _entriesView = entriesView;
            _name = name;
            _currentIndex = 0;
        }

        public void Close()
        {
        }
        
        public ICompletes<IEntry<string>> ReadNext()
        {
            if (_currentIndex < _entriesView.Count)
            {
                return Completes().With(_entriesView[_currentIndex++]);
            }
            
            return Completes().With<IEntry<string>>(null!);
        }

        public ICompletes<IEntry<string>> ReadNext(string fromId)
        {
            SeekTo(fromId);
            return ReadNext();
        }

        public ICompletes<IEnumerable<IEntry<string>>> ReadNext(int maximumEntries)
        {
            var entries = new List<IEntry<string>>(maximumEntries);

            for (var count = 0; count < maximumEntries; ++count)
            {
                if (_currentIndex < _entriesView.Count) {
                    entries.Add(_entriesView[_currentIndex++]);
                }
                else
                {
                    break;
                }
            }
            return Completes().With<IEnumerable<IEntry<string>>>(entries);
        }

        public ICompletes<IEnumerable<IEntry<string>>> ReadNext(string fromId, int maximumEntries)
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
                    ToEnd();
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

            return Completes().With(currentId);
        }

        public string Beginning { get; } = EntryReader.Beginning;
        
        public string End { get; } = EntryReader.End;
        
        public string Query { get; } = EntryReader.Query;
        
        public int DefaultGapPreventionRetries { get; } = EntryReader.DefaultGapPreventionRetries;
        
        public long DefaultGapPreventionRetryInterval { get; } = EntryReader.DefaultGapPreventionRetryInterval;

        public ICompletes<string> Name => Completes().With(_name);

        public ICompletes<long> Size => Completes().With((long) _entriesView.Count);

        private void ToEnd() => _currentIndex = _entriesView.Count - 1;

        private string ReadCurrentId()
        {
            if (_currentIndex < _entriesView.Count)
            {
                var currentId = _entriesView[_currentIndex].Id;
                return currentId;
            }
            return "-1";
        }
        
        private void To(string id)
        {
            Rewind();
            while (_currentIndex < _entriesView.Count)
            {
                var entry = _entriesView[_currentIndex];
                if (entry.Id.Equals(id))
                {
                    return;
                }
                ++_currentIndex;
            }
        }
    }
}