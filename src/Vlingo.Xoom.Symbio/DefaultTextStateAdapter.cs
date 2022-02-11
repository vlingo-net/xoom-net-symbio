// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Common.Serialization;
using Vlingo.Xoom.Symbio.Store;

namespace Vlingo.Xoom.Symbio;

public sealed class DefaultTextStateAdapter : StateAdapter<object, TextState>
{
    public override int TypeVersion => 1;

    public override object FromRawState(TextState raw)
    {
        try
        {
            var stateType = StoredTypes.ForName(raw.Type);
            return JsonSerialization.Deserialized(raw.Data, stateType)!;
        } 
        catch (Exception e) 
        {
            throw new InvalidOperationException($"Cannot convert to type: {raw.Type}", e);
        }
    }

    public override TOtherState FromRawState<TOtherState>(TextState raw) =>
        (TOtherState)JsonSerialization.Deserialized(raw.Data, typeof(TOtherState))!;

    public override TextState ToRawState(string id, object state, int stateVersion, Metadata metadata)
    {
        var serialization = JsonSerialization.Serialized(state);
        return new TextState(id, state.GetType(), TypeVersion, serialization, stateVersion, metadata);
    }
}