// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Symbio;

public class EntryBundle
{
    public IEntry Entry { get; }
    public ISource? Source { get; }

    public EntryBundle(IEntry entry, ISource source)
    {
        Entry = entry;
        Source = source;
    }

    public EntryBundle(IEntry entry)
    {
        Entry = entry;
        Source = null;
    }

    public IEntry<TEntry> TypedEntry<TEntry>() => (IEntry<TEntry>) Entry;

    public Source<TSource>? TypedSource<TSource>() => Source as Source<TSource>;
}