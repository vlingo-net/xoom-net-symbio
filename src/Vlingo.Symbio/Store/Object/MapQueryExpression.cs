// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// A query expression whose parameters are provided in a <code>Dictionary{TK, TV}</code> of name-value pairs.
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public class MapQueryExpression<T> : QueryExpression<T>
    {
        private readonly Dictionary<string, T> _parameters;

        /// <summary>
        /// Answer a new <see cref="FluentMap{TK, TV}"/> with a single <paramref name="key"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="key">the string key</param>
        /// <param name="value">The <typeparamref name="T"/> value</param>
        /// <returns></returns>
        public static FluentMap<string, T> Map(string key, T value)
        {
            var map = new FluentMap<string, T>(1);
            map.Add(key, value);
            return map;
        }

        /// <summary>
        /// Answer a new <code>MapQueryExpression</code> with <paramref name="query"/>, and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression<T> Using(string query, Dictionary<string, T> parameters) => new MapQueryExpression<T>(query, parameters);

        /// <summary>
        /// Answer a new <code>MapQueryExpression</code> with <paramref name="query"/>, and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode">The <see cref="QueryMode"/></param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression<T> Using(string query, QueryMode mode, Dictionary<string, T> parameters) => new MapQueryExpression<T>(query, mode, parameters);

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        public MapQueryExpression(string query, Dictionary<string, T> parameters) : base(query)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode">The <see cref="QueryMode"/></param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        public MapQueryExpression(string query, QueryMode mode, Dictionary<string, T> parameters) : base(query, mode)
        {
            _parameters = parameters;
        }

        public override bool IsMapQueryExpression { get; } = true;
        
        public override string ToString() => $"MapQueryExpression[type={Type.FullName} query={Query} mode={Mode} parameters={string.Join(",",_parameters.Select(p => $"{p.Key} | {p.Value}"))}]";
    }

    /// <summary>
    /// Support a <code>Dictionary{TK, TV}</code> with extension method <code>And(TK, TV)</code>.
    /// </summary>
    /// <typeparam name="TK">The key type</typeparam>
    /// <typeparam name="TV">The value type</typeparam>
    public class FluentMap<TK, TV> : Dictionary<TK, TV>
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
        public FluentMap<TK1, TV1> And<TK1, TV1>(TK1 key, TV1 value)
        {
            Add((TK)(object)key!, (TV)(object)value!);
            return (FluentMap<TK1, TV1>)(object) this!;
        }
    }
}