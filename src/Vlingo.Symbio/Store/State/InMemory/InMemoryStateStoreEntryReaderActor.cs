// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.State.InMemory
{
    public class InMemoryStateStoreEntryReaderActor<TEntry> : Actor, IStateStoreEntryReader<TEntry>
    {
        private int _currentIndex;
        private readonly List<IEntry<TEntry>> _entriesView;
        private readonly string _name;

        public InMemoryStateStoreEntryReaderActor(IEnumerable<IEntry<TEntry>> entriesView, string name)
        {
            _name = name;
            _entriesView = new List<IEntry<TEntry>>(entriesView);
            _currentIndex = 0;
        }

        public string Beginning => EntryReader.Beginning;

        public string End => EntryReader.End;

        public string Query => EntryReader.Query;

        public int DefaultGapPreventionRetries => EntryReader.DefaultGapPreventionRetries;

        public long DefaultGapPreventionRetryInterval => EntryReader.DefaultGapPreventionRetryInterval;
        
        public void Close()
        {
            _currentIndex = -1;
            _entriesView.Clear();
        }

        public ICompletes<string> Name => Completes().With(_name);
        public ICompletes<IEntry<TEntry>> ReadNext()
        {
            if (_currentIndex < _entriesView.Count)
            {
                return Completes().With(_entriesView[_currentIndex++]);
            }
            
            return Completes().With<IEntry<TEntry>>(default!);
        }

        public ICompletes<IEntry<TEntry>> ReadNext(string fromId)
        {
            SeekTo(fromId);
            return ReadNext();
        }

        public ICompletes<IEnumerable<IEntry<TEntry>>> ReadNext(int maximumEntries)
        {
            var entries = new List<IEntry<TEntry>>(maximumEntries);

            for (int count = 0; count < maximumEntries; ++count) {
                if (_currentIndex < _entriesView.Count)
                {
                    entries.Add(_entriesView[_currentIndex++]);
                }
                else
                {
                    break;
                }
            }
            return Completes().With(entries.AsEnumerable());
        }

        public ICompletes<IEnumerable<IEntry<TEntry>>> ReadNext(string fromId, int maximumEntries)
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

            return Completes().With(currentId);
        }

        public ICompletes<long> Size => Completes().With((long) _entriesView.Count);
        
        private void EndInternal() => _currentIndex = _entriesView.Count;

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