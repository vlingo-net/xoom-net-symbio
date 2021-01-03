// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Actors.TestKit;
using Vlingo.Common;
using Vlingo.Symbio.Store;
using Vlingo.Symbio.Store.Journal;

namespace Vlingo.Symbio.Tests.Store.Journal.InMemory
{
    public class MockAppendResultInterest<TEntry, TState> : IAppendResultInterest
    {
        private AccessSafely _access;
        private List<JournalData<TEntry, TState>> _entries = new List<JournalData<TEntry, TState>>();
        
        public void AppendResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, TSource source, Optional<TSnapshotState> snapshot, object @object) where TSource : Source
        {
            outcome.AndThen(result => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, null, result, new List<TSource> { source }, snapshot));
                return result;
            }).Otherwise(cause => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, cause, Result.Error, new List<TSource> { source }, snapshot));
                return cause.Result;
            });
        }

        public void AppendResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, TSource source, Metadata metadata, Optional<TSnapshotState> snapshot, object @object) where TSource : Source
        {
            outcome.AndThen(result => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, null, result, new List<TSource> { source }, snapshot));
                return result;
            }).Otherwise(cause => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, cause, Result.Error, new List<TSource> { source }, snapshot));
                return cause.Result;
            });
        }

        public void AppendAllResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, IEnumerable<TSource> sources, Optional<TSnapshotState> snapshot, object @object) where TSource : Source
        {
            outcome.AndThen(result => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, null, result, sources.ToList(), snapshot));
                return result;
            }).Otherwise(cause => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, cause, Result.Error, sources.ToList(), snapshot));
                return cause.Result;
            });
        }

        public void AppendAllResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, IEnumerable<TSource> sources, Metadata metadata, Optional<TSnapshotState> snapshot, object @object) where TSource : Source
        {
            outcome.AndThen(result => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, null, result, sources.ToList(), snapshot));
                return result;
            }).Otherwise(cause => {
                _access.WriteUsing("appendResultedIn",
                    new JournalData<TSource, TSnapshotState>(streamName, streamVersion, cause, Result.Error, sources.ToList(), snapshot));
                return cause.Result;
            });
        }
        
        public AccessSafely AfterCompleting(int times)
        {
            _access = AccessSafely.AfterCompleting(times)
                .WritingWith<JournalData<TEntry, TState>>("appendResultedIn", j => _entries.Add(j))
                .ReadingWith("appendResultedIn", () => _entries)
                .ReadingWith("size", () => _entries.Count);

            return _access;
        }
        
        public int ReceivedAppendsSize => _access.ReadFrom<int>("size");

        public IEnumerable<JournalData<TEntry, TState>> Entries => _access.ReadFrom<List<JournalData<TEntry, TState>>>("appendResultedIn");
    }
}