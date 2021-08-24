// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using IDispatcher = Vlingo.Xoom.Symbio.Store.Dispatch.IDispatcher;

namespace Vlingo.Xoom.Symbio.Store.Journal.InMemory
{
    public class InMemoryJournalActor<T> : Actor, IJournal<T>
    {
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private readonly InMemoryJournal<T> _journal;

        public InMemoryJournalActor(IDispatcher dispatcher)
        {
            _journal = new InMemoryJournal<T>(dispatcher, Stage.World);
            _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
        }

        public InMemoryJournalActor(IEnumerable<IDispatcher> dispatchers)
        {
            _journal = new InMemoryJournal<T>(dispatchers, Stage.World);
            _entryAdapterProvider = EntryAdapterProvider.Instance(Stage.World);
        }

        public IJournal<T> Using<TActor>(Stage stage, IDispatcher dispatcher, params object[] additional) where TActor : Actor
            => additional.Length == 0 ?
                    stage.ActorFor<IJournal<T>>(typeof(TActor), dispatcher) :
                    stage.ActorFor<IJournal<T>>(typeof(TActor), dispatcher, additional);

        public IJournal<T> Using<TActor>(Stage stage, IEnumerable<IDispatcher> dispatchers, params object[] additional) where TActor : Actor
            => additional.Length == 0 ?
                stage.ActorFor<IJournal<T>>(typeof(TActor), dispatchers) :
                stage.ActorFor<IJournal<T>>(typeof(TActor), dispatchers, additional);

        public void Append<TSource>(string streamName, int streamVersion, TSource source, IAppendResultInterest interest, object @object) where TSource : ISource =>
            _journal.Append(streamName, streamVersion, source, interest, @object);

        public void Append<TSource>(string streamName, int streamVersion, TSource source, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : ISource =>
            _journal.Append(streamName, streamVersion, source, metadata, interest, @object);

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource =>
            _journal.AppendWith(streamName, streamVersion, source, snapshot, interest, @object);

        public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
            => _journal.AppendWith(streamName, streamVersion, source, metadata, snapshot, interest, @object);

        public void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, IAppendResultInterest interest, object @object) where TSource : ISource =>
            _journal.AppendAll<TSource>(streamName, fromStreamVersion, sources, interest, @object);

        public void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : ISource
            => _journal.AppendAll<TSource>(streamName, fromStreamVersion, sources, metadata, interest, @object);

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
            => _journal.AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, snapshot, interest, @object);

        public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
            => _journal.AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, metadata, snapshot, interest, @object);

        public ICompletes<IJournalReader?> JournalReader(string name)
        {
            var inmemory = _journal.JournalReader(name).Outcome!;
            var actor = ChildActorFor<IJournalReader?>(() => new InMemoryJournalReaderActor(inmemory, _entryAdapterProvider));
            return Completes().With(actor);
        }

        public ICompletes<IStreamReader?> StreamReader(string name)
        {
            var inmemory = _journal.StreamReader(name).Outcome!;
            var actor = ChildActorFor<IStreamReader?>(() => new InMemoryStreamReaderActor(inmemory));
            return Completes().With(actor);
        }
    }
}