// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Xoom.Common;
using Vlingo.Symbio.Store.Gap;

namespace Vlingo.Symbio.Tests.Store.Gap
{
    public class RetryReaderActor : Actor, IReader
    {
        private readonly GapRetryReader<string> _reader;
        private int _offset = 0;

        public RetryReaderActor() => _reader = Reader();

        public GapRetryReader<string> Reader()
        {
            if (_reader == null)
            {
                var reader = new GapRetryReader<string>(Stage, Scheduler);
                return reader;
            }

            return _reader;
        }
        
        public ICompletes<IEntry<string>> ReadOne()
        {
            // Simulate failed read of one entry
            IEntry<string> entry = null;
            var gapIds = _reader.DetectGaps(entry, _offset, 1);
            var gappedEntries = new GappedEntries<string>(new List<IEntry<string>>(), gapIds, CompletesEventually());

            _reader.ReadGaps(gappedEntries, 3, TimeSpan.FromMilliseconds(10), ReadIds);
            _offset++;

            return (ICompletes<IEntry<string>>) Completes();
        }

        public ICompletes<List<IEntry<string>>> ReadNext(int count)
        {
            var entries = new List<IEntry<string>>();
            for (var i = 0; i < count; i++)
            {
                // every 3rd entry is loaded successfully
                if (i % 3 == 0)
                {
                    var entry = new TextEntry((_offset + i).ToString(), typeof(object), 1, $"Entry_{_offset}{i}");
                    entries.Add(entry);
                }
            }

            var gapIds = _reader.DetectGaps(entries, _offset, count);
            var gappedEntries = new GappedEntries<string>(entries, gapIds, CompletesEventually());
            _offset += count;
            _reader.ReadGaps(gappedEntries, 3, TimeSpan.FromMilliseconds(10), ReadIds);

            return (ICompletes<List<IEntry<string>>>) Completes();
        }
        
        private List<IEntry<string>> ReadIds(List<long> ids)
        {
            var entries = new List<IEntry<string>>();
            if (ids.Count < 3)
            {
                // Read all requested ids
                foreach (var id in ids)
                {
                    var entry = new TextEntry(id.ToString(), typeof(object), 1, $"Entry_{id}");
                    entries.Add(entry);
                }
            }
            else
            {
                for (var i = 0; i < ids.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        // Read every second id
                        var id = ids[i];
                        var entry = new TextEntry(id.ToString(), typeof(object), 1, "Entry_" + id);
                        entries.Add(entry);
                    }
                }
            }

            return entries;
        }
    }
}