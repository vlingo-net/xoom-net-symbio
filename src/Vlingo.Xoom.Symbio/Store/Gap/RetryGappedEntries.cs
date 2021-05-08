// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;

namespace Vlingo.Xoom.Symbio.Store.Gap
{
    public class RetryGappedEntries<T>
    {
        private readonly int _currentRetry;
        private readonly int _retries;
        
        public TimeSpan RetryInterval { get; }

        public Func<List<long>, List<IEntry<T>>> GappedReader { get; }
        
        public GappedEntries<T> GappedEntries { get; }

        public RetryGappedEntries(GappedEntries<T> gappedEntries, int currentRetry, int retries, TimeSpan retryInterval, Func<List<long>, List<IEntry<T>>> gappedReader)
        {
            GappedEntries = gappedEntries;
            _currentRetry = currentRetry;
            _retries = retries;
            RetryInterval = retryInterval;
            GappedReader = gappedReader;
        }
        
        public bool MoreRetries => _currentRetry < _retries;
        
        public RetryGappedEntries<T> NextRetry(GappedEntries<T> nextGappedEntries)
            => new RetryGappedEntries<T>(nextGappedEntries, _currentRetry + 1, _retries, RetryInterval, GappedReader);
    }
}