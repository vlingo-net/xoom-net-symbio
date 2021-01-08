// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace Vlingo.Symbio.Store.Gap
{
    public class RetryGappedEntries<T, TEntry> where TEntry : IEntry<T>
    {
        private readonly int _currentRetry;
        private readonly int _retries;
        
        public TimeSpan RetryInterval { get; }

        public Func<List<long>, List<TEntry>> GappedReader { get; }
        
        public GappedEntries<T, TEntry> GappedEntries { get; }

        public RetryGappedEntries(GappedEntries<T, TEntry> gappedEntries, int currentRetry, int retries, TimeSpan retryInterval, Func<List<long>, List<TEntry>> gappedReader)
        {
            GappedEntries = gappedEntries;
            _currentRetry = currentRetry;
            _retries = retries;
            RetryInterval = retryInterval;
            GappedReader = gappedReader;
        }
        
        public bool MoreRetries => _currentRetry < _retries;
        
        public RetryGappedEntries<T, TEntry> NextRetry(GappedEntries<T, TEntry> nextGappedEntries)
            => new RetryGappedEntries<T, TEntry>(nextGappedEntries, _currentRetry + 1, _retries, RetryInterval, GappedReader);
    }
}