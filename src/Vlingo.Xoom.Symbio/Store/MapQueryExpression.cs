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
    /// A query expression whose parameters are provided in a <code>Dictionary{TK, TV}</code> of name-value pairs.
    /// </summary>
    public class MapQueryExpression : QueryExpression
    {
        private readonly IDictionary<string, object> _parameters;

        /// <summary>
        /// Answer a new <see cref="FluentMap{TK, TV}"/> with a single <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">the string key</param>
        /// <param name="value">The state object value</param>
        /// <returns></returns>
        public static FluentMap<string, object> Map(string key, object value)
        {
            var map = new FluentMap<string, object>(1);
            map.Add(key, value);
            return map;
        }

        /// <summary>
        /// Answer a new <code>MapQueryExpression</code> with <paramref name="query"/>, and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The underlying storage type</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression Using(Type type, string query, IDictionary<string, object> parameters)
            => new MapQueryExpression(type, query, parameters);
        
        /// <summary>
        /// Answer a new <code>MapQueryExpression</code> with <paramref name="query"/>, and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <typeparam name="T">The underlying storage type</typeparam>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression Using<T>(string query, IDictionary<string, object> parameters)
            => new MapQueryExpression(typeof(T), query, parameters);

        /// <summary>
        /// Answer a new <code>MapQueryExpression</code> with <paramref name="query"/>, and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The underlying storage type</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode">The <see cref="QueryMode"/></param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression Using(Type type, string query, QueryMode mode, Dictionary<string, object> parameters)
            => new MapQueryExpression(type, query, mode, parameters);

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>IDictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        public MapQueryExpression(Type type, string query, IDictionary<string, object> parameters) : base(type, query)
            => _parameters = parameters;

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode">The <see cref="QueryMode"/></param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        public MapQueryExpression(Type type, string query, QueryMode mode, Dictionary<string, object> parameters) : base(type, query, mode)
            => _parameters = parameters;

        public IDictionary<string, object> Parameters => _parameters;

        public override bool IsMapQueryExpression { get; } = true;
        
        public override string ToString() => $"MapQueryExpression[type={Type.FullName} query={Query} mode={Mode} parameters={string.Join(",",_parameters.Select(p => $"{p.Key} | {p.Value}"))}]";
    }
    
    /// <summary>
    /// Support a <code>Dictionary{TK, TV}</code> with extension method <code>And(TK, TV)</code>.
    /// </summary>
    /// <typeparam name="TK">The key type</typeparam>
    /// <typeparam name="TV">The value type</typeparam>
    public class FluentMap<TK, TV> : Dictionary<TK, TV> where TK : notnull
    {
        /// <summary>
        /// Answer a new <code>FluentMap{TK, TV}</code> with <paramref name="key"/> and <paramref name="value"/> as the first entry.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns><code>FluentMap{TK, TV}</code></returns>
        public static FluentMap<TK, TV> Has(TK key, TV value) => new FluentMap<TK, TV>().And(key, value);

        /// <summary>
        /// Constructs my default state.
        /// </summary>
        public FluentMap()
        {
        }
        
        /// <summary>
        /// Constructs my default state to have the <paramref name="initialCapacity"/> for elements.
        /// </summary>
        /// <param name="initialCapacity">The int initial capacity for elements</param>
        public FluentMap(int initialCapacity) : base(initialCapacity)
        {
        }


        /// <summary>
        /// Answer myself after adding <paramref name="value"/> at <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TK1"/> typed key</param>
        /// <param name="value">The <typeparamref name="TV1"/> typed value</param>
        /// <typeparam name="TK1">The key type, which is same as K but for specific casting</typeparam>
        /// <typeparam name="TV1">The value type, which is same as V but for specific casting</typeparam>
        /// <returns><code>FluentMap{TK, TV}</code></returns>
        public FluentMap<TK1, TV1> And<TK1, TV1>(TK1 key, TV1 value) where TK1 : notnull
        {
            Add((TK)(object)key!, (TV)(object)value!);
            return (FluentMap<TK1, TV1>)(object) this!;
        }
    }
}