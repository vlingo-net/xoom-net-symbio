// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Common;
using Vlingo.Symbio.Store;
using Vlingo.Symbio.Store.Object;
using Vlingo.Xoom.Actors.TestKit;

namespace Vlingo.Symbio.Tests.Store.Object.InMemory
{
    public class MockQueryResultInterest : IQueryResultInterest
    {
        private AccessSafely _access = AccessSafely.AfterCompleting(1);
        private readonly List<object> _stateObjects = new List<object>();

        public void QueryAllResultedIn(IOutcome<StorageException, Result> outcome, QueryMultiResults results, object @object)
            => _access.WriteUsing("addAll", results.StateObjects);

        public void QueryObjectResultedIn(IOutcome<StorageException, Result> outcome, QuerySingleResult result, object @object)
        {
            outcome
                .AndThen(good => good)
                .Otherwise(bad => throw new InvalidOperationException($"Bogus outcome: {bad.Message}"));

            _access.WriteUsing("add", result.StateObject);
        }
        
        public AccessSafely AfterCompleting(int times)
        {
            _access =
                AccessSafely
                    .AfterCompleting(times)
                    .WritingWith<object>("add", value => _stateObjects.Add(value))
                    .WritingWith<object>("addAll", values => _stateObjects.AddRange((List<StateObject>)values))
                    .ReadingWith<int, object>("object", index => _stateObjects[index])
                    .ReadingWith("size", () => _stateObjects.Count);

            return _access;
        }
    }
}