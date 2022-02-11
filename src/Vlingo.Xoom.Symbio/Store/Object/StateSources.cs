// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vlingo.Xoom.Symbio.Store.Object;

/// <summary>
/// StateSources records the mapping between <see cref="StateObject"/> and the
/// <see cref="Source{T}"/> of its creation or mutation.
/// </summary>
/// <typeparam name="T">The type of the state.</typeparam>
/// <typeparam name="TSource">The type of the underlying source</typeparam>
public class StateSources<T, TSource> where T : StateObject where TSource : ISource
{
    public StateSources(T stateObject): this(stateObject, Enumerable.Empty<ISource>())
    {
    }
        
    public StateSources(T stateObject, IEnumerable<ISource> sources)
    {
        StateObject = stateObject ?? throw new ArgumentNullException(nameof(stateObject),"stateObject is required");
        Sources = sources.ToList() ?? throw new ArgumentNullException(nameof(sources),"sources is required");
    }
        
    public static StateSources<T, TSource> Of(T stateObject) => new StateSources<T, TSource>(stateObject, Enumerable.Empty<ISource>());
        
    public static StateSources<T, TSource> Of(T stateObject, ISource source) => new StateSources<T, TSource>(stateObject, new []{source});
        
    public static StateSources<T, TSource> Of(T stateObject, IEnumerable<ISource> sources) => new StateSources<T, TSource>(stateObject, sources);
        
    public T StateObject { get; }
        
    public IEnumerable<ISource> Sources { get; }

    public override bool Equals(object? obj)
    {
        if (this == obj)
        {
            return true;
        }

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
            
        var that = (StateSources<T, TSource>) obj;
        if (!StateObject.Equals(that.StateObject) || Sources.Count() != that.Sources.Count())
        {
            return false;
        }

        var sources = Sources.ToList();
        var otherSources = that.Sources.ToList();
        for (var i = 0; i < sources.Count; i++)
        {
            if (!sources[i].Equals(otherSources[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        var partialHash = 31 * StateObject.GetHashCode();
        var sourcesHash = 0;
        foreach (var source in Sources)
        {
            sourcesHash += source.GetHashCode();
        }

        return partialHash + sourcesHash;
    }
        
    public override string ToString() =>
        "StateSources{" +
        "stateObject=" + StateObject +
        ", sources=" + Sources +
        '}';
}