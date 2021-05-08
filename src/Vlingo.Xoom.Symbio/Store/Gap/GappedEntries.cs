// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Gap
{
    public class GappedEntries<T>
    {
        public GappedEntries(IEnumerable<IEntry<T>> loadedEntries, IEnumerable<long> gapIds, ICompletesEventually completesEventually)
        {
            CompletesEventually = completesEventually;
            LoadedEntries = loadedEntries.ToList();
            GapIds = gapIds.ToList();
        }
        
        /// <summary>
        /// Successfully loaded entries up to now.
        /// </summary>
        public List<IEntry<T>> LoadedEntries { get; }

        public List<IEntry<T>> SortedLoadedEntries => LoadedEntries.OrderBy(e => long.Parse(e.Id)).ToList();
        
        /// <summary>
        /// List of ids failed to be loaded (gaps).
        /// </summary>
        public List<long> GapIds { get; }
        
        /// <summary>
        /// <see cref="ICompletesEventually"/> object necessary to asynchronously send the final list of loaded entries.
        /// </summary>
        public ICompletesEventually CompletesEventually { get; }

        public bool ContainsGaps => GapIds.Any();

        /// <summary>
        /// Gets combined size of loaded and gapped entries.
        /// </summary>
        public int Count => LoadedEntries.Count + GapIds.Count;
        
        /// <summary>
        /// Get first successfully loaded entry.
        /// </summary>
        /// <returns>First successfully loaded entry</returns>
        public Optional<IEntry<T>> GetFirst()
        {
            if (LoadedEntries.Count > 0)
            {
                return Optional.Of(LoadedEntries[0]);
            }

            return Optional.Empty<IEntry<T>>();
        }
        
        public GappedEntries<T> FillupWith(IEnumerable<IEntry<T>> fillups)
        {
            var newLoadedEntries = new List<IEntry<T>>(LoadedEntries);
            var newGapIds = new List<long>(GapIds);
            foreach (var fillup in fillups)
            {
                var fillupId = long.Parse(fillup.Id);
                newGapIds.Remove(fillupId);
                newLoadedEntries.Add(fillup);
            }

            return new GappedEntries<T>(newLoadedEntries, newGapIds, CompletesEventually);
        }
    }
}