// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace Vlingo.Symbio
{
    public static class DictionaryExtensions
    {
        public static TValue AddIfAbsent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.TryGetValue(key, out var ret))
            {
                return ret;
            }

            dictionary.Add(key, value);
            return default!;
        }

        public static TValue ComputeIfAbsent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> mappingFunction)
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