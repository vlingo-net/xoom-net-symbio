// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Reactive.Streams;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Streams;

namespace Vlingo.Xoom.Symbio.Store
{
    public class EntryReaderStream : IStream
    {
        private readonly EntryAdapterProvider _entryAdapterProvider;
        private long _flowElementsRate;
        private readonly IEntryReader _entryReader;
        private readonly Stage _stage;
#pragma warning disable 649
        private EntryStreamSubscriber<object>? _subscriber;
#pragma warning restore 649

        public EntryReaderStream(Stage stage, IEntryReader entryReader, EntryAdapterProvider entryAdapterProvider)
        {
            _stage = stage;
            _entryReader = entryReader;
            _entryAdapterProvider = entryAdapterProvider;
        }
        
        public void Request(long flowElementsRate)
        {
            _flowElementsRate = flowElementsRate;

            _subscriber?.SubscriptionHook?.Request(_flowElementsRate);
        }

        public void FlowInto<T>(Sink<T> sink) => FlowInto(sink, Stream.DefaultFlowRate, Stream.DefaultProbeInterval);

        public void FlowInto<T>(Sink<T> sink, long flowElementsRate) => FlowInto(sink, flowElementsRate, Stream.DefaultProbeInterval);

        public void FlowInto<T>(Sink<T> sink, long flowElementsRate, int probeInterval)
        {
            _flowElementsRate = flowElementsRate;

            var configuration =
                PublisherConfiguration.With(
                    probeInterval,
                    Streams.Streams.DefaultMaxThrottle,
                    Streams.Streams.DefaultBufferSize,
                    Streams.Streams.OverflowPolicy.DropCurrent);

            var entryReaderSource = _stage.ActorFor<ISource<T>>(() => new EntryReaderSource<T>(_entryReader, _entryAdapterProvider, flowElementsRate));

            var publisher = _stage.ActorFor<IPublisher<T>>(() => new StreamPublisher<T>(entryReaderSource, configuration));

            var subscriber = _stage.ActorFor<ISubscriber<T>>(() => new EntryStreamSubscriber<T>(sink, flowElementsRate));

            publisher.Subscribe(subscriber);
        }

        public void Stop() => _subscriber?.SubscriptionHook?.Cancel();
    }
}