// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
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
    public class InMemoryJournalActor<TEntry, TState> : Actor, IJournal<TEntry>
    {
        private InMemoryJournal<TEntry, TState> _journal;

        public InMemoryJournalActor(IDispatcher<Dispatchable<TEntry, TState>> dispatcher)
            => _journal = new InMemoryJournal<TEntry, TState>(dispatcher, Stage.World);
        
        public IJournal<TEntry> Using<TActor, TState1>(Stage stage, IDispatcher<Dispatchable<TEntry, TState1>> dispatcher, params object[] additional) where TActor : Actor
            => additional.Length == 0 ?
                    stage.ActorFor<IJournal<TEntry>>(typeof(TActor), dispatcher) :
                    stage.ActorFor<IJournal<TEntry>>(typeof(TActor), dispatcher, additional);

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, Source<TSource> source, IAppendResultInterest interest, object @object)
            => _journal.Append<TSource, TSnapshotState>(streamName, streamVersion, source, interest, @object);

        public void Append<TSource, TSnapshotState>(string streamName, int streamVersion, Source<TSource> source, Metadata metadata, IAppendResultInterest interest, object @object)
            => _journal.Append<TSource, TSnapshotState>(streamName, streamVersion, source, metadata, interest, @object);

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, Source<TSource> source, TSnapshotState snapshot, IAppendResultInterest interest, object @object)
            => _journal.AppendWith(streamName, streamVersion, source, snapshot, interest, @object);

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, Source<TSource> source, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object)
            => _journal.AppendWith(streamName, streamVersion, source, metadata, snapshot, interest, @object);

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<Source<TSource>> sources, IAppendResultInterest interest, object @object)
            => _journal.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, interest, @object);

        public void AppendAll<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<Source<TSource>> sources, Metadata metadata, IAppendResultInterest interest, object @object)
            => _journal.AppendAll<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata, interest, @object);

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<Source<TSource>> sources, TSnapshotState snapshot, IAppendResultInterest interest, object @object)
            => _journal.AppendAllWith(streamName, fromStreamVersion, sources, snapshot, interest, @object);

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<Source<TSource>> sources, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object)
            => _journal.AppendAllWith(streamName, fromStreamVersion, sources, metadata, snapshot, interest, @object);

        public ICompletes<IJournalReader<TEntry>?> JournalReader(string name)
        {
            var inmemory = _journal.JournalReader(name).Outcome!;
            var actor = ChildActorFor<IJournalReader<TEntry>?>(Definition.Has<InMemoryJournalReaderActor<TEntry>>(Definition.Parameters(inmemory)));
            return Completes().With(actor);
        }

        public ICompletes<IStreamReader<TEntry>?> StreamReader(string name)
        {
            var inmemory = _journal.StreamReader(name).Outcome!;
            var actor = ChildActorFor<IStreamReader<TEntry>?>(Definition.Has<InMemoryStreamReaderActor<TEntry>>(Definition.Parameters(inmemory)));
            return Completes().With(actor);
        }
    }
}