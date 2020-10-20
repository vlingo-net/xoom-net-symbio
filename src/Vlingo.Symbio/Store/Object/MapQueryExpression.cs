// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// A query expression whose parameters are provided in a <code>Dictionary{TK, TV}</code> of name-value pairs.
    /// </summary>
    public class MapQueryExpression : QueryExpression
    {
        private readonly Dictionary<string, object> _parameters;

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
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression Using<T>(string query, Dictionary<string, object> parameters) where T : StateObject => new MapQueryExpression(typeof(T), query, parameters);

        /// <summary>
        /// Answer a new <code>MapQueryExpression</code> with <paramref name="query"/>, and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode">The <see cref="QueryMode"/></param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        /// <returns>MapQueryExpression</returns>
        public static MapQueryExpression Using<T>(string query, QueryMode mode, Dictionary<string, object> parameters) where T : StateObject => new MapQueryExpression(typeof(T), query, mode, parameters);

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        public MapQueryExpression(Type type, string query, Dictionary<string, object> parameters) : base(type, query)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Constructs my default state with <code>QueryMode.ReadOnly</code>.
        /// </summary>
        /// <param name="type">The concrete type of state object</param>
        /// <param name="query">The string describing the query</param>
        /// <param name="mode">The <see cref="QueryMode"/></param>
        /// <param name="parameters"><code>Dictionary{TK, TV></code> containing query parameters of name-value pairs</param>
        public MapQueryExpression(Type type, string query, QueryMode mode, Dictionary<string, object> parameters) : base(type, query, mode)
        {
            _parameters = parameters;
        }

        public Dictionary<string, object> Parameters => _parameters;

        public override bool IsMapQueryExpression { get; } = true;
        
        public override string ToString() => $"MapQueryExpression[type={Type.FullName} query={Query} mode={Mode} parameters={string.Join(",",_parameters.Select(p => $"{p.Key} | {p.Value}"))}]";
    }

    /// <summary>
    /// Support a <code>Dictionary{TK, TV}</code> with extension method <code>And(TK, TV)</code>.
    /// </summary>
    /// <typeparam name="Tk">The key type</typeparam>
    /// <typeparam name="Tv">The value type</typeparam>
    public class FluentMap<Tk, Tv> : Dictionary<Tk, Tv> where Tk : notnull
    {
        /// <summary>
        /// Answer a new <code>FluentMap{TK, TV}</code> with <paramref name="key"/> and <paramref name="value"/> as the first entry.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns><code>FluentMap{TK, TV}</code></returns>
        public static FluentMap<Tk, Tv> Has(Tk key, Tv value) => new FluentMap<Tk, Tv>().And(key, value);

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
        /// <param name="key">The <typeparamref name="Tk1"/> typed key</param>
        /// <param name="value">The <typeparamref name="Tv1"/> typed value</param>
        /// <typeparam name="Tk1">The key type, which is same as K but for specific casting</typeparam>
        /// <typeparam name="Tv1">The value type, which is same as V but for specific casting</typeparam>
        /// <returns><code>FluentMap{TK, TV}</code></returns>
        public FluentMap<Tk1, Tv1> And<Tk1, Tv1>(Tk1 key, Tv1 value) where Tk1 : notnull
        {
            Add((Tk)(object)key!, (Tv)(object)value!);
            return (FluentMap<Tk1, Tv1>)(object) this!;
        }
    }
}