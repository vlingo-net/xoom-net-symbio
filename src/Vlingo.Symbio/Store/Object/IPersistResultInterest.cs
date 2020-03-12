// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// Defines the result of persisting to the store with a persistent object.
    /// </summary>
    public interface IPersistResultInterest
    {
        /// <summary>
        /// Implemented by the interest of a given Object Store for persist operation results.
        /// </summary>
        /// <param name="outcome">The <code>IOutcome{StorageException, Result}</code> of the persist operation</param>
        /// <param name="stateObject">The object to persist; for PersistAll() this will be a <code>IEnumerable{Object}</code></param>
        /// <param name="possible">The int number of possible objects to persist</param>
        /// <param name="actual">The int number of actual objects persisted</param>
        /// <param name="object">the object passed to Persist() that is sent back to the receiver, or null if not passed</param>
        void PersistResultedIn(IOutcome<StorageException, Result> outcome, object? stateObject, int possible, int actual, object? @object);
    }
}