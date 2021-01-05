// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Actors;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Gap
{
    /// <summary>
    /// Detection and fill up (gap prevention) functionality related to <see cref="IEntryReader{T}"/>
    /// </summary>
    public class GapRetryReader<T>
    {
        private readonly IScheduled<RetryGappedEntries<T>> _actor;
        private Scheduler _scheduler;
        
        public GapRetryReader(Stage stage, Scheduler scheduler)
        {
            _actor = stage.ActorFor<IScheduled<RetryGappedEntries<T>>>(() => new GapsFillUpActor<T>());
            _scheduler = scheduler;
        }
        
        /// <summary>
        /// Single entry variant method of <see cref="M:DetecGaps"/>
        /// </summary>
        /// <param name="entry">The entry</param>
        /// <param name="startIndex">This index refers to <see cref="M:IEntry.Id"/></param>
        /// <param name="count">The number of entries</param>
        /// <returns></returns>
        public IEnumerable<long> DetectGaps(IEntry<T>? entry, long startIndex, long count)
        {
            var entries = entry == null ? Enumerable.Empty<IEntry<T>>() : new []{entry};
            return DetectGaps(entries, startIndex, count);
        }
        
        public IEnumerable<long> DetectGaps(IEnumerable<IEntry<T>> entries, long startIndex, long count)
        {
            var allIds = CollectIds(entries);
            var gapIds = new List<long>();

            for (long index = 0; index < count; index++)
            {
                if (!allIds.Contains(startIndex + index))
                {
                    gapIds.Add(startIndex + index);
                }
            }

            return gapIds;
        }
        
        public void ReadGaps(GappedEntries<T> gappedEntries, int retries, TimeSpan retryInterval, Func<List<long>, List<IEntry<T>>> gappedReader)
        {
            var entries = new RetryGappedEntries<T>(gappedEntries, 1, retries, retryInterval, gappedReader);
            _scheduler.ScheduleOnce(_actor, entries, TimeSpan.Zero, retryInterval);
        }
        
        private List<long> CollectIds(IEnumerable<IEntry<T>>? entries)
        {
            if (entries == null)
            {
                return new List<long>();
            }

            return entries.Select(e => long.Parse(e.Id)).ToList();
        }
    }
}