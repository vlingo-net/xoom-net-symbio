// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// The single <code>stateObject</code> result of the completed query and a possible <code>updateId</code>.
    /// </summary>
    public class QuerySingleResult : QueryResult
    {
        public static QuerySingleResult Of(object? stateObject) => new QuerySingleResult(stateObject);

        public static QuerySingleResult Of(object? stateObject, long updateId) => new QuerySingleResult(stateObject, updateId);
        
        public QuerySingleResult(object? stateObject) => StateObject = stateObject;

        public QuerySingleResult(object? stateObject, long updateId) : base(updateId) => StateObject = stateObject;

        public object? StateObject { get; }
        
        public T ToStateObject<T>() => StateObject != null ? (T) StateObject : default;
    }
}