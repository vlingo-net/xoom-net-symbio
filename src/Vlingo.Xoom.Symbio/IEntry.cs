// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Symbio.Store;

namespace Vlingo.Xoom.Symbio
{
    public interface IEntry
    {
        /// <summary>
        /// Returns a type for a given type name.
        /// </summary>
        /// <param name="type">Name of the type to load. Use fully qualified type name.</param>
        /// <returns>Type for the given name</returns>
        /// <exception cref="InvalidOperationException">If the type cannot be found for a given type name</exception>
        Type TypedFrom(string type);

        /// <summary>
        /// Gets current entry's id.
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// Gets my associated (possibly null) Metadata
        /// </summary>
        Metadata Metadata { get; }
        
        /// <summary>
        /// Gets my typeName.
        /// </summary>
        string TypeName { get; }
        
        /// <summary>
        /// Gets my type name.
        /// </summary>
        string Type { get; }
        
        /// <summary>
        /// Gets my type version.
        /// </summary>
        int TypeVersion { get; }
        
        /// <summary>
        /// Gets entry version that is the state version with which I am associated.
        /// </summary>
        int EntryVersion { get; }
        
        /// <summary>
        /// Gets whether or not I have non-empty Metadata.
        /// </summary>
        bool HasMetadata { get; }
        
        /// <summary>
        /// Gets whether or not I am a completely empty Entry.
        /// </summary>
        bool IsEmpty { get; }
        
        /// <summary>
        /// Gets whether or not I am a NullEntry.
        /// </summary>
        bool IsNull { get; }
        
        /// <summary>
        /// Gets the Type.
        /// </summary>
        Type Typed { get; }
        
        string EntryRawData { get; }
    }
    
    /// <summary>
    /// Entry represents a journal entry
    /// </summary>
    /// <typeparam name="T">The concrete of <c>IEntry{T}</c> stored and read, which may be a string, byte[] or object</typeparam>
    public interface IEntry<T> : IComparable<IEntry<T>>, IEntry
    {
        /// <summary>
        /// Returns an empty <see>
        ///     <cref>IEnumerable{IEntry{T}}</cref>
        /// </see>
        /// .
        /// </summary>
        /// <typeparam name="T">The type used in <c>IEntry{T}</c></typeparam>
        /// <returns><see>
        ///     <cref>IEnumerable{IEntry{T}}</cref>
        /// </see>
        /// </returns>
        IEnumerable<IEntry<T>> None { get; }

        /// <summary>
        /// Gets current entry's data.
        /// </summary>
        T EntryData { get; }

        /// <summary>
        /// Gets a copy of myself with the id.
        /// </summary>
        /// <param name="id">The identity to assign to my copy.</param>
        /// <returns>A copy of myself with new identity.</returns>
        IEntry<T> WithId(string id);
    }

    public static class Entry<T>
    {
        public static int DefaultVersion => -1;

        public static IEnumerable<IEntry<T>> None => new IEntry<T>[0]; // Enumerable.Empty<IEntry<T>>(); throws an exception on deserialization with Netwtonsoft 

        public static Type TypedFrom(string type) => StoredTypes.ForName(type)!;
    }
}