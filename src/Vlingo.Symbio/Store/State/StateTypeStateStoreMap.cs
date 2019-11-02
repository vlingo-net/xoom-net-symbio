// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Vlingo.Symbio.Store.State
{
    public static class StateTypeStateStoreMap
    {
        private static readonly ConcurrentDictionary<string, string> StateStoreNames = new ConcurrentDictionary<string, string>();

        public static IEnumerable<string> AllStoreNames => StateStoreNames.Values;

        public static void StateTypeToStoreName<T>(string storeName) =>
            StateStoreNames.AddOrUpdate(typeof(T).FullName, storeName, (key, value) => storeName);

        public static string? StoreNameFrom<T>() => StoreNameFrom(typeof(T).FullName);
        
        public static string? StoreNameFrom(string typeName) => StateStoreNames.TryGetValue(typeName, out var name) ? name : null;
        
        /// <summary>
        /// USED FOR TEST ONLY
        /// </summary>
        public static void Reset() => StateStoreNames.Clear();
    }
}