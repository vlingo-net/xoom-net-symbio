// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vlingo.Xoom.Symbio.Store
{
    /// <summary>
    /// A query expression whose parameters are provided in a <code>IEnumerable</code>.
    /// </summary>
    public class ListQueryExpression : QueryExpression
    {
        private readonly IEnumerable<object> _parameters;

        /// <summary>
        /// Answer a new <code>QueryExpression</code> for <param name="type" />, <paramref name="query"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The concrete type of the state object.</param>
        /// <param name="query">The string expression of the query</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns><see cref="ListQueryExpression"/><code>IEnumerable{object}</code> containing query parameters</returns>
        public static ListQueryExpression Using(Type type, string query, IEnumerable<object> parameters) => new ListQueryExpression(type, query, parameters);

        /// <summary>
        /// Answer a new <code>QueryExpression</code> for <param name="type" />, <paramref name="query"/> and <see cref="QueryMode"/>.
        /// </summary>
        /// <param name="type">The concrete type of the state object.</param>
        /// <param name="query">The string expression of the query</param>
        /// <param name="mode"><see cref="QueryMode"/></param>
        /// <param name="parameters"><code>IEnumerable{object}</code> containing query parameters</param>
        /// <returns><see cref="ListQueryExpression"/></returns>
        public static ListQueryExpression Using(Type type, string query, QueryMode mode, IEnumerable<object> parameters) => new ListQueryExpression(type, query, mode, parameters);

        /// <summary>
        /// Constructs my default state.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>IEnumerable{object}</code> containing query parameters</param>
        public ListQueryExpression(Type type, string query, IEnumerable<object> parameters) : base(type, query) => _parameters = parameters;

        /// <summary>
        /// Constructs my default state.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode"><see cref="QueryMode"/></param>
        /// <param name="parameters"><code>IEnumerable{object}</code> containing query parameters</param>
        public ListQueryExpression(Type type, string query, QueryMode mode, IEnumerable<object> parameters) : base(type, query, mode) => _parameters = parameters;

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="param">The variable arguments containing query parameters</param>
        public ListQueryExpression(Type type, string query, params object[] param) : base(type, query) => _parameters = param.AsEnumerable();

        /// <summary>
        /// Constructs my default state.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode"><see cref="QueryMode"/></param>
        /// <param name="param">The variable arguments containing query parameters</param>
        public ListQueryExpression(Type type, string query, QueryMode mode, params object[] param) : base(type, query, mode) => _parameters = param.AsEnumerable();

        public IEnumerable<object> Parameters => _parameters;

        public override bool IsListQueryExpression { get; } = true;
        
        public override string ToString() => $"ListQueryExpression[type={Type.FullName} query={Query} mode={Mode} parameters={string.Join(",",_parameters.Select(p => $"{p}"))}]";
    }
}