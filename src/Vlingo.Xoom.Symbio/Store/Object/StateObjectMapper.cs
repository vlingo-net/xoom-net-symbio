// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Xoom.Symbio.Store.Object;

/// <summary>
/// Holder and provider of <see cref="StateObject"/> type mappers.
/// The <code>persistMapper</code> and <code>queryMapper</code> are received
/// and held as <code>object</code> instances, but the underlying type
/// is implementation specific (e.g. ADO, EF, NHibernate, etc.).
/// The specific types are cast when retrieving by means of the
/// methods <code>PersistMapper()</code> and <code>QueryMapper()</code>.
/// </summary>
public class StateObjectMapper
{
    private readonly object _persistMapper;
    private readonly object _queryMapper;
    private readonly Type _type;
        
    /// <summary>
    /// Answer a new <code>StateObjectMapper</code> with <typeparamref name="T"/>, <paramref name="persistMapper"/>, and <paramref name="queryMapper"/>.
    /// </summary>
    /// <param name="persistMapper">The object mapper of persistence information</param>
    /// <param name="queryMapper">The object mapper of query information</param>
    /// <typeparam name="T">The type of the persistent object to be mapped</typeparam>
    /// <returns>New instance of <code>StateObjectMapper</code></returns>
    public static StateObjectMapper With<T>(object persistMapper, object queryMapper) => new StateObjectMapper(typeof(T), persistMapper, queryMapper);

    /// <summary>
    /// Construct my state with <paramref name="type"/>, <paramref name="persistMapper"/>, and <paramref name="queryMapper"/>.
    /// </summary>
    /// <param name="type">The <code>Type</code> type of the persistent object to be mapped</param>
    /// <param name="persistMapper">The object mapper of persistence information</param>
    /// <param name="queryMapper">the object mapper of query information</param>
    public StateObjectMapper(Type type, object persistMapper, object queryMapper)
    {
        _type = type;
        _persistMapper = persistMapper;
        _queryMapper = queryMapper;
    }

    /// <summary>
    /// Casts persist mapper as an <typeparamref name="TMapper"/>.
    /// </summary>
    /// <typeparam name="TMapper">Type of the mapper to cast to</typeparam>
    /// <returns>Casted mapper to <typeparamref name="TMapper"/> or throws <exception cref="InvalidCastException"></exception></returns>
    public TMapper PersistMapper<TMapper>() => (TMapper) _persistMapper;
        
    /// <summary>
    /// Casts query mapper as an <typeparamref name="TMapper"/>.
    /// </summary>
    /// <typeparam name="TMapper">Type of the mapper to cast to</typeparam>
    /// <returns>Casted mapper to <typeparamref name="TMapper"/> or throws <exception cref="InvalidCastException"></exception></returns>
    public TMapper QueryMapper<TMapper>() => (TMapper) _queryMapper;
        
    /// <summary>
    /// Gets the current type.
    /// </summary>
    public Type Type => _type;
}