// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Symbio.Store;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Tests.Store.Dispatch
{
    public class MockConfirmDispatchedResultInterest : IConfirmDispatchedResultInterest
    {
        public void ConfirmDispatchedResultedIn(Result result, string dispatchId)
        {
            // not used
        }
    }
}