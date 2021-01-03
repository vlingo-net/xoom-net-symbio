// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Store.Journal.InMemory
{
    public class InMemoryJournalActor<T, TEntry, TState> : Actor, IJournal<T> where TEntry : IEntry<T> where TState : class, IState
    {
        private InMemoryJournal<T, TEntry, TState> _journal;

        public InMemoryJournalActor(IDispatcher<Dispatchable<TEntry, TState>> dispatcher)
            => _journal = new InMemoryJournal<T, TEntry, TState>(dispatcher, Stage.World);
        
        public IJournal<T> Using<TActor, TNewEntry, TNewState>(Stage stage, IDispatcher<Dispatchable<TNewEntry, TNewState>> dispatcher, params object[] additional) where TActor : Actor where TNewEntry : IEntry<T> where TNewState : class, IState
            => additional.Length == 0 ?
                    stage.ActorFor<IJournal<T>>(typeof(TActor), dispatcher) :
                    stage.ActorFor<IJournal<T>>(typeof(TActor), dispatcher, additional);

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, IAppendResultInterest interest, object @object) where TSource : Source =>
            _journal.Append<TSource, TSnapshotState>(streamName, streamVersion, source, interest, @object);

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : Source =>
            _journal.Append<TSource, TSnapshotState>(streamName, streamVersion, source, metadata, interest, @object);

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : Source =>
            _journal.AppendWith(streamName, streamVersion, source, snapshot, interest, @object);

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : Source
            => _journal.AppendWith(streamName, streamVersion, source, metadata, snapshot, interest, @object);

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<TSource> sources, IAppendResultInterest interest, object @object) where TSource : Source =>
            _journal.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, interest, @object);

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<TSource> sources, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : Source
            => _journal.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata, interest, @object);

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<TSource> sources, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : Source
            => _journal.AppendAllWith(streamName, fromStreamVersion, sources, snapshot, interest, @object);

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<TSource> sources, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : Source
            => _journal.AppendAllWith(streamName, fromStreamVersion, sources, metadata, snapshot, interest, @object);

        public ICompletes<IJournalReader<TNewEntry>?> JournalReader<TNewEntry>(string name) where TNewEntry : IEntry
        {
            var inmemory = _journal.JournalReader<TNewEntry>(name).Outcome!;
            var actor = ChildActorFor<IJournalReader<TNewEntry>?>(() => new InMemoryJournalReaderActor<TNewEntry>(inmemory));
            return Completes().With(actor);
        }

        public ICompletes<IStreamReader<T>?> StreamReader(string name)
        {
            var inmemory = _journal.StreamReader(name).Outcome!;
            var actor = ChildActorFor<IStreamReader<T>?>(() => new InMemoryStreamReaderActor<T>(inmemory));
            return Completes().With(actor);
        }
    }
}