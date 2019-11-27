// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// The base query expression.
    /// </summary>
    public class QueryExpression<T>
    {
        /// <summary>
        /// Answer a new <code>QueryExpression</code> for <typeparam name="T" /> and <paramref name="query"/>.
        /// </summary>
        /// <param name="query">The string expression of the query</param>
        /// <returns><see cref="QueryExpression{T}"/></returns>
        public static QueryExpression<T> Using(string query) => new QueryExpression<T>(query);

        /// <summary>
        /// Answer a new <code>QueryExpression</code> for <typeparam name="T" />, <paramref name="query"/> and <see cref="QueryMode"/>.
        /// </summary>
        /// <param name="query">The string expression of the query</param>
        /// <param name="mode"><see cref="QueryMode"/></param>
        /// <returns><see cref="QueryExpression{T}"/></returns>
        public static QueryExpression<T> Using(string query, QueryMode mode) => new QueryExpression<T>(query);

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="query">The describing the query</param>
        public QueryExpression(string query) : this(query, QueryMode.ReadOnly)
        {
        }

        /// <summary>
        /// Constructs my default state.
        /// </summary>
        /// <param name="query">The describing the query</param>
        /// <param name="mode">The QueryMode</param>
        public QueryExpression(string query, QueryMode mode)
        {
            Query = query;
            Mode = mode;
        }

        /// <summary>
        /// Answer myself as a <see cref="ListQueryExpression{T}"/>.
        /// </summary>
        /// <returns><see cref="ListQueryExpression{T}"/></returns>
        public ListQueryExpression<T> AsListQueryExpression() => (ListQueryExpression<T>) this;
        
        /// <summary>
        /// Answer myself as a <see cref="MapQueryExpression{T}"/>.
        /// </summary>
        /// <returns><see cref="MapQueryExpression{T}"/></returns>
        public MapQueryExpression<T> AsMapQueryExpression() => (MapQueryExpression<T>) this;

        /// <summary>
        /// Gets whether or not I am a <see cref="ListQueryExpression{T}"/>.
        /// </summary>
        public virtual bool IsListQueryExpression { get; } = false;
        
        /// <summary>
        /// Gets whether or not I am a <see cref="MapQueryExpression{T}"/>.
        /// </summary>
        public virtual bool IsMapQueryExpression { get; } = false;
        
        public QueryMode Mode { get; }
        
        public string Query { get; }

        public Type Type { get; } = typeof(T);

        public override string ToString() => $"QueryExpression[type={Type.FullName} query={Query} mode={Mode}]";
    }
}