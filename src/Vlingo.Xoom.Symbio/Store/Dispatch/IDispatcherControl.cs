// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Symbio.Store.Dispatch
{
    /// <summary>
    /// Defines the means to confirm previously dispatched results, and to
    /// re-dispatch those that have not been successfully confirmed.
    /// </summary>
    public interface IDispatcherControl
    {
        /// <summary>
        /// Confirm that the <paramref name="dispatchId"/> has been dispatched.
        /// </summary>
        /// <param name="dispatchId">The string unique identity of the dispatched state</param>
        /// <param name="interest">The <see cref="IConfirmDispatchedResultInterest"/></param>
        void ConfirmDispatched(string dispatchId, IConfirmDispatchedResultInterest interest);

        /// <summary>
        /// Attempt to dispatch any unconfirmed dispatchables.
        /// </summary>
        void DispatchUnconfirmed();

        /// <summary>
        /// Stop attempting to dispatch unconfirmed dispatchables.
        /// </summary>
        void Stop();
    }
}