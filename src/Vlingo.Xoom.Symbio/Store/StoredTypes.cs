// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Concurrent;

namespace Vlingo.Xoom.Symbio.Store;

/// <summary>
/// Gathers all types stored by vlingo-symbio. Main purpose of this class is to cache the stored types.
/// </summary>
public static class StoredTypes
{
    private static readonly ConcurrentDictionary<string, Type> StoredTypesMap =
        new ConcurrentDictionary<string, Type>();

    public static Type ForName(string typeName)
    {
        StoredTypesMap.TryGetValue(typeName, out var loadedType);
        if (loadedType == null)
        {
            loadedType = Type.GetType(typeName);
            StoredTypesMap.TryAdd(typeName, loadedType!);
        }
            
        if (loadedType == null)
        {
            throw new InvalidOperationException($"Cannot get type for type name: {typeName}");
        }

        return loadedType;
    }
}