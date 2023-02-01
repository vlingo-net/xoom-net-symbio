// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Symbio.Store.Object;

/// <summary>
/// Abstract base of query result types.
/// </summary>
public abstract class QueryResult
{
    public long UpdateId { get; }

    public bool IsUpdatable => ObjectStoreReader.IsId(UpdateId);
        
    protected QueryResult() : this(ObjectStoreReader.NoId)
    {
    }
        
    protected QueryResult(long updateId) => UpdateId = updateId;
}