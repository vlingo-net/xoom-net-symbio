// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Symbio.Store.Dispatch;

public class NoOpDispatcher : IDispatcher, IConfirmDispatchedResultInterest
{
    private IDispatcherControl? _control;

    public void ControlWith(IDispatcherControl control) => _control = control;

    public void Dispatch(Dispatchable dispatchable) => _control?.ConfirmDispatched(dispatchable.Id, this);

    public void ConfirmDispatchedResultedIn(Result result, string dispatchId)
    {
    }
}