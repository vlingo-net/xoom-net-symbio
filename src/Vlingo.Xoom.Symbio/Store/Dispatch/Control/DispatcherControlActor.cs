// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Dispatch.Control
{
    public sealed class DispatcherControlActor : Actor, IDispatcherControl, IScheduled<object?>
    {
        private static readonly long DefaultRedispatchDelay = 2000L;

        private readonly IEnumerable<IDispatcher> _dispatchers;
        private readonly IDispatcherControlDelegate _delegate;
        private readonly ICancellable _cancellable;
        private readonly long _confirmationExpiration;

        public DispatcherControlActor(
            IEnumerable<IDispatcher> dispatchers,
            IDispatcherControlDelegate @delegate,
            long redispatchDelay,
            long checkConfirmationExpirationInterval,
            long confirmationExpiration)
        {
            _dispatchers = dispatchers;
            _delegate = @delegate;
            _confirmationExpiration = confirmationExpiration;
            _cancellable = Scheduler.Schedule(this, null, TimeSpan.FromMilliseconds(redispatchDelay),
                TimeSpan.FromMilliseconds(checkConfirmationExpirationInterval));
            foreach (var dispatcher in _dispatchers)
            {
                dispatcher.ControlWith(this);   
            }
        }

        public DispatcherControlActor(
            IEnumerable<IDispatcher> dispatchers,
            IDispatcherControlDelegate @delegate,
            long checkConfirmationExpirationInterval,
            long confirmationExpiration) : this(dispatchers, @delegate, DefaultRedispatchDelay, checkConfirmationExpirationInterval, confirmationExpiration)
        {
        }
        
        public DispatcherControlActor(
            IDispatcher dispatcher,
            IDispatcherControlDelegate @delegate,
            long checkConfirmationExpirationInterval,
            long confirmationExpiration)
        : this (new []{dispatcher}, @delegate, DefaultRedispatchDelay, checkConfirmationExpirationInterval, confirmationExpiration)
        {
        }

        public void ConfirmDispatched(string dispatchId, IConfirmDispatchedResultInterest interest)
        {
            try
            {
                _delegate.ConfirmDispatched(dispatchId);
                interest.ConfirmDispatchedResultedIn(Result.Success, dispatchId);
            }
            catch (Exception e)
            {
                Logger.Error($"{GetType().FullName} confirmDispatched() failed because: {e.Message}", e);
                interest.ConfirmDispatchedResultedIn(Result.Failure, dispatchId);
            }
        }

        public void DispatchUnconfirmed()
        {
            try
            {
                var now = DateTimeOffset.Now;
                var dispatchables = _delegate.AllUnconfirmedDispatchableStates.ToList();
                foreach (var dispatchable in dispatchables)
                {
                    var then = dispatchable.CreatedOn;
                    var duration = then - now;
                    if (Math.Abs(duration.TotalMilliseconds) > _confirmationExpiration)
                    {
                        foreach (var dispatcher in _dispatchers)
                        {
                            dispatcher.Dispatch(dispatchable);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"{GetType().FullName} dispatchUnconfirmed() failed because: {e.Message}", e);
            }
        }

        public void IntervalSignal(IScheduled<object?> scheduled, object? data) => DispatchUnconfirmed();

        public override void Stop()
        {
            if (_cancellable != null)
            {
                _cancellable.Cancel();
            }

            _delegate.Stop();
            base.Stop();
        }
    }
}