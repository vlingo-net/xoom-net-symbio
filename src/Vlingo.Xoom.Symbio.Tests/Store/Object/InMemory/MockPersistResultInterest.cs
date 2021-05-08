// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Symbio.Store;
using Vlingo.Xoom.Symbio.Store.Object;
using Vlingo.Xoom.Actors.TestKit;

namespace Vlingo.Xoom.Symbio.Tests.Store.Object.InMemory
{
    public class MockPersistResultInterest : IPersistResultInterest
    {
        private AccessSafely _access = AccessSafely.AfterCompleting(1);
        private readonly List<object> _stateObjects = new List<object>();
        
        public void PersistResultedIn(IOutcome<StorageException, Result> outcome, object stateObject, int possible, int actual, object @object)
        {
            if (actual == 1)
            {
                _access.WriteUsing("add", stateObject);
            }
            else if (actual > 1)
            {
                _access.WriteUsing("addAll", (List<Person>)stateObject);
            }
            else
            {
                throw new InvalidOperationException($"Possible is:{possible} Actual is: {actual}");
            }
        }
        
        public AccessSafely AfterCompleting(int times)
        {
            _access =
                AccessSafely
                    .AfterCompleting(times)
                    .WritingWith<object>("add", value => _stateObjects.Add(value))
                    .WritingWith<List<Person>>("addAll", values => _stateObjects.AddRange(values))
                    .ReadingWith<int, object>("object", index => _stateObjects[index])
                    .ReadingWith("size", () => _stateObjects.Count);

            return _access;
        }
    }
}