// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Reactive.Streams;
using Vlingo.Xoom.Streams;

namespace Vlingo.Xoom.Symbio.Store
{
    public class EntryStreamSubscriber<T> : StreamSubscriber<T>
    {
        public ISubscription? SubscriptionHook { get; private set; }

        public EntryStreamSubscriber(Sink<T> sink, long requestThreshold) : base(sink, requestThreshold)
        {
        }

        public override void OnSubscribe(ISubscription? subscription)
        {
            SubscriptionHook = subscription;
            base.OnSubscribe(subscription);
        }
    }
}