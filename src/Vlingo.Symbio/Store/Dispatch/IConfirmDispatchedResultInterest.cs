// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.Dispatch
{
    /// <summary>
    /// Defines the means to communicate the confirmation of a previously
    /// dispatched storage <code>State{T}</code> results to the receiver.
    /// </summary>
    public interface IConfirmDispatchedResultInterest
    {
        /// <summary>
        /// Sends the confirmation of a dispatched <see cref="State{T}"/>.
        /// </summary>
        /// <param name="result">The <see cref="Result"/> of the dispatch confirmation</param>
        /// <param name="dispatchId">The string unique identity of the dispatched <see cref="State{T}"/></param>
        void ConfirmDispatchedResultedIn(Result result, string dispatchId);
    }
}