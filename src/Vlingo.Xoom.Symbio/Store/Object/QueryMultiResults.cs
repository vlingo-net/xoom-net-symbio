// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Collections.Generic;

namespace Vlingo.Xoom.Symbio.Store.Object
{
    /// <summary>
    /// The collection of <code>stateObjects</code> results of the completed query and a possible <code>updateId</code>.
    /// </summary>
    public class QueryMultiResults : QueryResult
    {
        public static QueryMultiResults Of(IEnumerable stateObjects) => new QueryMultiResults(stateObjects);
        
        public static QueryMultiResults Of(IEnumerable stateObjects, long updatedId) => new QueryMultiResults(stateObjects, updatedId);
        
        public QueryMultiResults(IEnumerable stateObjects)
        {
            StateObjects = stateObjects;
        }
        
        public QueryMultiResults(IEnumerable stateObjects, long updatedId) : base(updatedId)
        {
            StateObjects = stateObjects;
        }

        public IEnumerable<T> ToStateObjects<T>() => (IEnumerable<T>) StateObjects;
        
        public IEnumerable StateObjects { get; }
    }
}