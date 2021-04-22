// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Common;

namespace Vlingo.Symbio.Tests.Store.Gap
{
    public interface IReader
    {
        public ICompletes<IEntry<string>> ReadOne();
        public ICompletes<List<IEntry<string>>> ReadNext(int count);
    }
}