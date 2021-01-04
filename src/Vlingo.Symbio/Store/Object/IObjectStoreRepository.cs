// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// A repository for over an <see cref="IObjectStore"/>
    /// </summary>
    public interface IObjectStoreRepository
    {
        IObjectStore ObjectStore { get; }
    }
}