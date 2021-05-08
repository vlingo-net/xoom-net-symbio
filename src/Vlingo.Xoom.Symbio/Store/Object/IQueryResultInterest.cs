// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Object
{
    /// <summary>
    /// Defines the result of querying one or more persistent objects.
    /// </summary>
    public interface IQueryResultInterest
    {
        /// <summary>
        /// Implemented by the interest of a given Object Store for a query operation with zero or more results.
        /// </summary>
        /// <param name="outcome">The <code>IOutcome{StorageException, Result}</code> of the query</param>
        /// <param name="results">The <code>MultiQueryResults</code> of the query, with zero or more objects and a possible updateId</param>
        /// <param name="object">The object passed to Query() that is sent back to the receiver</param>
        void QueryAllResultedIn(IOutcome<StorageException, Result> outcome, QueryMultiResults results, object? @object);
        
        /// <summary>
        /// Implemented by the interest of a given Object Store for an object query operation with a single result.
        /// </summary>
        /// <param name="outcome">The <code>IOutcome{StorageException, Result}</code> of the query</param>
        /// <param name="result">The <code>SingleQueryResult</code> of the query, with zero or one object and a possible updateId</param>
        /// <param name="object">the object passed to Query() that is sent back to the receiver</param>
        void QueryObjectResultedIn(IOutcome<StorageException, Result> outcome, QuerySingleResult result, object? @object);
    }
}