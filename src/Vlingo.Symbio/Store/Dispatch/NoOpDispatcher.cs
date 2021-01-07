// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.Dispatch
{
    public class NoOpDispatcher : IDispatcher<Dispatchable<IEntry<string>, TextState>>, IConfirmDispatchedResultInterest
    {
        private IDispatcherControl? _control;

        public void ControlWith(IDispatcherControl control) => _control = control;

        public void Dispatch(Dispatchable<IEntry<string>, TextState> dispatchable) => _control?.ConfirmDispatched(dispatchable.Id, this);

        public void ConfirmDispatchedResultedIn(Result result, string dispatchId)
        {
        }
    }
}