// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Symbio.Store.Journal.InMemory
{
    public class InMemoryStreamReaderActor : Actor, IStreamReader
    {
        private readonly IStreamReader _reader;

        public InMemoryStreamReaderActor(IStreamReader reader) => _reader = reader;

        public override void Start()
        {
            Logger.Debug($"Starting InMemoryStreamReaderActor named: {_reader.Name}");
            base.Start();
        }

        public ICompletes<EntityStream> StreamFor(string streamName) => Completes().With(_reader.StreamFor(streamName).Outcome);

        public ICompletes<EntityStream> StreamFor(string streamName, int fromStreamVersion) => Completes().With(_reader.StreamFor(streamName, fromStreamVersion).Outcome);

        public string Name => _reader.Name;
    }
}