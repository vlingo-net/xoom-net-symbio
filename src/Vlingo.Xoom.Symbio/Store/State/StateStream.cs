// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Streams;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Streams;

namespace Vlingo.Xoom.Symbio.Store.State;

public class StateStream<TRawState> : IStream where TRawState : IState
{
    private long _flowElementsRate;
    private readonly Stage _stage;
    private readonly Dictionary<string, TRawState> _states;
    private readonly StateAdapterProvider _stateAdapterProvider;
    private IStateStreamSubscriber? _subscriber;

    public StateStream(Stage stage, Dictionary<string, TRawState> states, StateAdapterProvider stateAdapterProvider)
    {
        _stage = stage;
        _states = states;
        _stateAdapterProvider = stateAdapterProvider;
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

        var publisher = _stage.ActorFor<IPublisher<T>>(() => new StreamPublisher<T>(new StateSource<T>(_states, _stateAdapterProvider, flowElementsRate), configuration));
            
        var subscriber = _stage.ActorFor<ISubscriber<T>>(() => new StateStreamSubscriber<T>(sink, flowElementsRate, this));

        publisher.Subscribe(subscriber);
    }

    public void Stop() => _subscriber?.SubscriptionHook?.Cancel();

    private class StateSource<T> : ISource<T>
    {
        private readonly long _flowElementsRate;
        private readonly IEnumerator<string> _iterator;
        private readonly Dictionary<string, TRawState> _states;
        private readonly StateAdapterProvider _stateAdapterProvider;

        public StateSource(Dictionary<string, TRawState> states, StateAdapterProvider stateAdapterProvider, long flowElementsRate)
        {
            _states = states;
            _iterator = states.Keys.GetEnumerator();
            _stateAdapterProvider = stateAdapterProvider;
            _flowElementsRate = flowElementsRate;
        }
            
        public ISource<T> Empty() => Streams.Source<T>.Empty();

        public ISource<T> Only(IEnumerable<T> elements) => Streams.Source<T>.Only(elements);

        public ISource<long> RangeOf(long startInclusive, long endExclusive) => Streams.Source<T>.RangeOf(startInclusive, endExclusive);

        public long OrElseMaximum(long elements) => Streams.Source<T>.OrElseMaximum(elements);

        public long OrElseMinimum(long elements) => Streams.Source<T>.OrElseMinimum(elements);

        public ISource<T> With(IEnumerable<T> iterable) => Streams.Source<T>.With(iterable);

        public ISource<T> With(IEnumerable<T> iterable, bool slowIterable) => Streams.Source<T>.With(iterable, slowIterable);

        public ISource<T> With(Func<T> supplier) => Streams.Source<T>.With(supplier);

        public ISource<T> With(Func<T> supplier, bool slowSupplier) => Streams.Source<T>.With(supplier, slowSupplier);

        public ICompletes<Elements<T>> Next()
        {
            var next = new List<StateBundle>();
            for (var i = 0; i < _flowElementsRate; i++)
            {
                if (_iterator.MoveNext())
                {
                    var id = _iterator.Current;
                    var state = _states[id];
                    var @object = _stateAdapterProvider.FromRaw<T, TRawState>(state);
                    next.Add(new StateBundle(state, @object!));
                }
                else if (next.Count > 0)
                {
                    // TODO: should have been -> var elements = Elements<T>.Of(next.Cast<T>().ToArray());
                    var partialElements = Elements<T>.Of(next.Select(e => e.Object).Cast<T>().ToArray());
                    return Completes.WithSuccess(partialElements);
                }
                else
                {
                    return Completes.WithSuccess(Elements<T>.Terminated());
                }
            }
                
            // TODO: should have been -> var elements = Elements<T>.Of(next.Cast<T>().ToArray());
            var elements = Elements<T>.Of(next.Select(e => e.Object).Cast<T>().ToArray());
            return Completes.WithSuccess(elements);
        }

        public ICompletes<Elements<T>> Next(int maximumElements) => Next();

        public ICompletes<Elements<T>> Next(long index) => Next();

        public ICompletes<Elements<T>> Next(long index, int maximumElements) => Next();

        public ICompletes<bool> IsSlow() => Completes.WithSuccess(false);
    }
        
    public interface IStateStreamSubscriber
    {
        public ISubscription? SubscriptionHook { get; set; }
    }
        
    public class StateStreamSubscriber<T> : StreamSubscriber<T>, IStateStreamSubscriber
    {
        public StateStreamSubscriber(Sink<T> sink, long requestThreshold, StateStream<TRawState> stateStream) : base(sink, requestThreshold) => 
            stateStream._subscriber = this;

        public override void OnSubscribe(ISubscription? subscription)
        {
            SubscriptionHook = subscription;
            
            base.OnSubscribe(subscription);
        }

        public ISubscription? SubscriptionHook { get; set; }
    }
}