// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace Vlingo.Xoom.Symbio
{
    public static class DictionaryExtensions
    {
        public static TValue AddIfAbsent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var ret))
            {
                return ret;
            }

            dictionary.Add(key, value);
            
            return default!;
        }

        public static TValue ComputeIfAbsent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> mappingFunction) where TKey : notnull
        {
            TValue v = default!;
            if (!dictionary.ContainsKey(key))
            {
                var newValue = mappingFunction(key);
                if (newValue != null)
                {
                    dictionary.Add(key, newValue);
                    return newValue;
                }
            }
            else
            {
                return dictionary[key];
            }

            return v;
        }
    }
}