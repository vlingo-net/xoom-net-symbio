// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Symbio.Store.State;
using Xunit;

namespace Vlingo.Symbio.Tests.Store.State
{
    public class StateTypeStateStoreMapTest
    {
        [Fact]
        public void TestExistingMappings()
        {
            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity1).FullName, typeof(Entity1));
            
            Assert.Equal(typeof(Entity1).FullName, StateTypeStateStoreMap.StoreNameFrom(typeof(Entity1)));
            Assert.Equal(typeof(Entity1).FullName, StateTypeStateStoreMap.StoreNameFrom(typeof(Entity1).FullName));

            Assert.Null(StateTypeStateStoreMap.StoreNameFrom(typeof(Entity2)));
            Assert.Null(StateTypeStateStoreMap.StoreNameFrom(typeof(Entity2).FullName));

            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity2).FullName, typeof(Entity2));

            Assert.Equal(typeof(Entity2).FullName, StateTypeStateStoreMap.StoreNameFrom(typeof(Entity2)));
            Assert.Equal(typeof(Entity2).FullName, StateTypeStateStoreMap.StoreNameFrom(typeof(Entity2).FullName));

            Assert.Equal(typeof(Entity1).FullName, StateTypeStateStoreMap.StoreNameFrom(typeof(Entity1)));
            Assert.Equal(typeof(Entity1).FullName, StateTypeStateStoreMap.StoreNameFrom(typeof(Entity1).FullName));
        }

        [Fact]
        public void TestNonExistingMappings()
        {
            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity1).FullName, typeof(Entity1));
            StateTypeStateStoreMap.StateTypeToStoreName(typeof(Entity2).FullName, typeof(Entity2));

            Assert.Null(StateTypeStateStoreMap.StoreNameFrom("123"));
            Assert.Null(StateTypeStateStoreMap.StoreNameFrom(typeof(string).FullName));
        }

        public StateTypeStateStoreMapTest()
        {
            StateTypeStateStoreMap.Reset();
        }
    }
}