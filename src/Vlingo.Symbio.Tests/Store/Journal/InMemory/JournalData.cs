// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Common;
using Vlingo.Symbio.Store;

namespace Vlingo.Symbio.Tests.Store.Journal.InMemory
{
    public class JournalData<TEntry, TState>
    {
        public JournalData(string streamName, int streamVersion, Exception errorCauses, Result result, List<TEntry> sources, Optional<TState> snapshot)
        {
            StreamName = streamName;
            StreamVersion = streamVersion;
            ErrorCauses = errorCauses;
            Result = result;
            Sources = sources;
            Snapshot = snapshot;
        }
        
        public string StreamName { get; }
        
        public int StreamVersion{ get; }
        
        public List<TEntry> Sources { get; }
        
        public Optional<TState> Snapshot { get; }

        public Exception ErrorCauses { get; }
        
        public Result Result { get; }
    }
}