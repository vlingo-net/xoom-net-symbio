// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Symbio.Store.Dispatch;

/// <summary>
/// Defines the support for dispatching.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Register the <paramref name="control"/> with the receiver.
    /// </summary>
    /// <param name="control">The <see cref="IDispatcherControl"/> to register</param>
    void ControlWith(IDispatcherControl control);

    /// <summary>
    /// Dispatch the <see cref="Dispatchable"/> instance.
    /// </summary>
    /// <param name="dispatchable">The <see cref="Dispatchable"/> instance to this dispatch</param>
    void Dispatch(Dispatchable dispatchable);
}