// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Actors;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Symbio.Tests.Store.Gap
{
    public class RetryActorTest
    {
        private readonly IReader _readerActor;
        
        public RetryActorTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
            
            var world = World.StartWithDefaults("retry-actor-tests");
            _readerActor = world.ActorFor<IReader>(() => new RetryReaderActor());
        }
        
        [Fact]
        public void ReadTest()
        {
            var entry = _readerActor.ReadOne().Await();
            Assert.Equal("0", entry.Id);

            var entry2 = _readerActor.ReadOne().Await();
            Assert.Equal("1", entry2.Id);

            var entries = _readerActor.ReadNext(10).Await();
            Assert.Equal(10, entries.Count);

            var entries2 = _readerActor.ReadNext(50).Await();
            // 4 entries out of 50 didn't get loaded at all
            Assert.Equal(46, entries2.Count);
            
            long previousId = -1;
            foreach (var currentEntry in entries2)
            {
                long currentId = long.Parse(currentEntry.Id);
                Assert.True(previousId < currentId);
                previousId = currentId;
            }
        }
    }
}