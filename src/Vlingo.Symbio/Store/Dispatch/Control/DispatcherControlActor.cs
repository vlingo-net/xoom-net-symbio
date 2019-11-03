// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Dispatch.Control
{
    public sealed class DispatcherControlActor<TEntry, TState> : Actor, IDispatcherControl, IScheduled<object?>
    {
        private static readonly long DefaultRedispatchDelay = 2000L;
        
        private readonly IDispatcher<Dispatchable<TEntry, TState>> _dispatcher;
        private readonly IDispatcherControlDelegate<TEntry, TState> _delegate;
        private readonly long _checkConfirmationExpirationInterval;
        private readonly ICancellable _cancellable;
        private readonly long _confirmationExpiration;

        public DispatcherControlActor(
            IDispatcher<Dispatchable<TEntry, TState>> dispatcher,
            IDispatcherControlDelegate<TEntry, TState> @delegate,
            long checkConfirmationExpirationInterval,
            long confirmationExpiration)
        {
            _dispatcher = dispatcher;
            _delegate = @delegate;
            _checkConfirmationExpirationInterval = checkConfirmationExpirationInterval;
            _confirmationExpiration = confirmationExpiration;
            _cancellable = Scheduler.Schedule(this, null, TimeSpan.FromMilliseconds(DefaultRedispatchDelay), TimeSpan.FromMilliseconds(checkConfirmationExpirationInterval));
            _dispatcher.ControlWith(this);
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
                        _dispatcher.Dispatch(dispatchable);
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