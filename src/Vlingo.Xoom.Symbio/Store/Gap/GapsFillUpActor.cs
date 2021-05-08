// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Gap
{
    public class GapsFillUpActor<T> : Actor, IScheduled<RetryGappedEntries<T>>
    {
        public void IntervalSignal(IScheduled<RetryGappedEntries<T>> scheduled, RetryGappedEntries<T> data)
        {
            var gappedReader = data.GappedReader;
            var fillups = gappedReader(data.GappedEntries.GapIds);
            var nextGappedEntries = data.GappedEntries.FillupWith(fillups);

            if (!nextGappedEntries.ContainsGaps || !data.MoreRetries)
            {
                var eventually = data.GappedEntries.CompletesEventually;
                if (nextGappedEntries.Count == 1)
                {
                    // Only one entry has to be returned.
                    eventually.With(nextGappedEntries.GetFirst().OrElse(null!));
                }
                else
                {
                    eventually.With(nextGappedEntries.SortedLoadedEntries);
                }
            }
            else
            {
                var nextData = data.NextRetry(nextGappedEntries);
                Scheduler.ScheduleOnce(scheduled, nextData, TimeSpan.Zero, data.RetryInterval);
            }
        }
    }
}