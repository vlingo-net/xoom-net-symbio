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

public sealed class DefaultTextEntryAdapter<TState> : EntryAdapter where TState : ISource
{
    public override ISource FromEntry(IEntry entry)
    {
        try
        {
            var sourceType = StoredTypes.ForName(entry.TypeName);
            var bland = JsonSerialization.Deserialized(entry.EntryRawData, sourceType);
            return (TState) bland!;
        } 
        catch (Exception) 
        {
            throw new InvalidOperationException($"Cannot convert to type: {entry.TypeName}");
        }
    }

    public override IEntry ToEntry(ISource source, Metadata metadata)
    {
        var serialization = JsonSerialization.Serialized(source);
        return new TextEntry(source.GetType(), 1, serialization, 1, metadata);
    }

    public override IEntry ToEntry(ISource source, string id, Metadata metadata)
    {
        var serialization = JsonSerialization.Serialized(source);
        return new TextEntry(id, source.GetType(), 1, serialization, metadata);
    }
        
    public override IEntry ToEntry(ISource source, int version, string id, Metadata metadata)
    {
        var serialization = JsonSerialization.Serialized(source);
        return new TextEntry(id, source.GetType(), 1, serialization, version, metadata);
    }

    public override Type SourceType { get; } = typeof(TState);

    public override IEntry ToEntry(ISource source, int version, Metadata metadata)
    {
        var serialization = JsonSerialization.Serialized(source);
        return new TextEntry(source.GetType(), 1, serialization, version, metadata);
    }
}