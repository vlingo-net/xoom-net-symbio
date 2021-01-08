// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio.Store
{
    /// <summary>
    /// The base query expression.
    /// </summary>
    public class QueryExpression
    {
        /// <summary>
        /// Answer a new <code>QueryExpression</code> for <typeparam name="T" /> and <paramref name="query"/>.
        /// </summary>
        /// <param name="query">The string expression of the query</param>
        /// <returns><see cref="QueryExpression"/></returns>
        public static QueryExpression Using<T>(string query) => new QueryExpression(typeof(T), query);

        /// <summary>
        /// Answer a new <code>QueryExpression</code> for <typeparam name="T" />, <paramref name="query"/> and <see cref="QueryMode"/>.
        /// </summary>
        /// <param name="query">The string expression of the query</param>
        /// <param name="mode"><see cref="QueryMode"/></param>
        /// <returns><see cref="QueryExpression"/></returns>
        public static QueryExpression Using<T>(string query, QueryMode mode) => new QueryExpression(typeof(T), query, mode);

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="type">The concrete type of the state object.</param>
        /// <param name="query">The describing the query</param>
        public QueryExpression(Type type, string query) : this(type, query, QueryMode.ReadOnly)
        {
        }

        /// <summary>
        /// Constructs my default state.
        /// </summary>
        /// <param name="type">The concrete type of the state object.</param>
        /// <param name="query">The describing the query</param>
        /// <param name="mode">The QueryMode</param>
        public QueryExpression(Type type, string query, QueryMode mode)
        {
            Type = type;
            Query = query;
            Mode = mode;
        }

        /// <summary>
        /// Answer myself as a <see cref="ListQueryExpression"/>.
        /// </summary>
        /// <returns><see cref="ListQueryExpression"/></returns>
        public ListQueryExpression AsListQueryExpression() => (ListQueryExpression) this;
        
        /// <summary>
        /// Answer myself as a <see cref="MapQueryExpression"/>.
        /// </summary>
        /// <returns><see cref="MapQueryExpression"/></returns>
        public MapQueryExpression AsMapQueryExpression() => (MapQueryExpression) this;

        /// <summary>
        /// Gets whether or not I am a <see cref="ListQueryExpression"/>.
        /// </summary>
        public virtual bool IsListQueryExpression { get; } = false;
        
        /// <summary>
        /// Gets whether or not I am a <see cref="MapQueryExpression"/>.
        /// </summary>
        public virtual bool IsMapQueryExpression { get; } = false;
        
        public QueryMode Mode { get; }
        
        public string Query { get; }

        public Type Type { get; }

        public override string ToString() => $"QueryExpression[type={Type.FullName} query={Query} mode={Mode}]";
    }
}