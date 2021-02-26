// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Journal.InMemory
{
    public class InMemoryStreamReader<TEntry> : IStreamReader
    {
        private readonly List<BaseEntry> _journalView;
        private readonly Dictionary<string, State<TEntry>> _snapshotsView;
        private readonly Dictionary<string, Dictionary<int, int>> _streamIndexesView;
        private readonly string _name;

        public InMemoryStreamReader(List<BaseEntry> journalView, Dictionary<string, Dictionary<int, int>> streamIndexesView, Dictionary<string, State<TEntry>> snapshotsView, string name)
        {
            _journalView = journalView;
            _streamIndexesView = streamIndexesView;
            _snapshotsView = snapshotsView;
            _name = name;
        }

        public ICompletes<EntityStream> StreamFor(string streamName) => StreamFor(streamName, 1);

        public ICompletes<EntityStream> StreamFor(string streamName, int fromStreamVersion)
        {
            var version = fromStreamVersion;
            if (_snapshotsView.TryGetValue(streamName, out var snapshot))
            {
                if (snapshot.DataVersion > version)
                {
                    version = snapshot.DataVersion;
                }
                else
                {
                    snapshot = null; // reading from beyond snapshot
                }
            }
            
            var entries = new List<BaseEntry>();
            if (_streamIndexesView.TryGetValue(streamName, out var versionIndexes))
            {
                while (versionIndexes.TryGetValue(version, out var journalIndex))
                {
                    var entry = _journalView[journalIndex];
                    entries.Add(entry);
                    ++version;
                }
            }
            return Completes.WithSuccess(new EntityStream(streamName, version - 1, entries, snapshot));
        }

        public string Name => _name;
    }
}