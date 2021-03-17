// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Common.Version;

namespace Vlingo.Symbio
{
    public interface ISource
    {
        /// <summary>
        /// Gets my <c>Id</c> as a string. By default my id is empty. Override to provide an actual id.
        /// </summary>
        string Id { get; }
    }
    
    /// <summary>
    /// Abstract base of any concrete type that is a source of truth. The concrete
    /// type is represented by the <paramref name="{T}"/> parameter and is also extends me.
    /// </summary>
    /// <typeparam name="T">The type of source of truth</typeparam>
    public abstract class Source<T> : ISource
    {
        private readonly long _dateTimeSourced;
        private readonly int _sourceTypeVersion;
        
        /// <summary>
        /// Gets an instance of the <see cref="Source{T}.NullSource{TNested}"/>
        /// </summary>
        public static Source<T> Nulled => new NullSource<T>();

        /// <summary>
        /// Gets <see><cref>IEnumerable{Source{T}}</cref></see> of the array of <paramref name="sources" />.
        /// </summary>
        /// <param name="sources">Individual sources</param>
        /// <returns>All sources passed in as a new enumerable</returns>
        public static IEnumerable<Source<T>> All(params Source<T>[] sources) => sources;

        /// <summary>
        /// Gets <see><cref>IEnumerable{Source{T}}</cref></see> of non null <paramref name="sources" />.
        /// </summary>
        /// <param name="sources">Sources from which a new non null sources are returned.</param>
        /// <returns>An enumerable of Non null sources.</returns>
        public static IEnumerable<Source<T>> All(IEnumerable<Source<T>> sources) => sources.Where(s => !s.IsNull);

        /// <summary>
        /// Gets empty sources
        /// </summary>
        /// <returns>Empty list.</returns>
        public static IEnumerable<T> None() => Enumerable.Empty<T>();

        /// <summary>
        /// Gets whether or not I am a Null Object, which is by default <c>false</c>.
        /// </summary>
        public virtual bool IsNull => false;

        /// <summary>
        /// Gets my type name, which is the simple name of my concrete <see cref="System.Type"/>.
        /// </summary>
        public string TypeName => GetType().Name;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj != null && obj.GetType() != GetType())
            {
                return false;
            }
            
            return obj != null && Id.Equals(((Source<T>) obj).Id);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var id = Id;
            return
                $"Source [id={(string.IsNullOrEmpty(id) ? "(none)" : id)} " +
                $"dateTimeSourced={DateTimeOffset.FromUnixTimeMilliseconds(_dateTimeSourced).UtcDateTime.ToShortTimeString()} " +
                $"sourceTypeVersion={SemanticVersion.ToString(_sourceTypeVersion)}]";
        }

        /// <summary>
        /// Construct my default state.
        /// </summary>
        protected Source(): this(SemanticVersion.ToValue(1, 0, 0))
        {
        }

        /// <summary>
        /// Construct my default state.
        /// </summary>
        /// <param name="sourceTypeVersion">the int type version of my concrete extender</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="sourceTypeVersion"/> is less or equal to 0.</exception>
        protected Source(int sourceTypeVersion)
        {
            if (sourceTypeVersion <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceTypeVersion), "The version should be greater than 0.");
            }
            
            _dateTimeSourced = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _sourceTypeVersion = sourceTypeVersion;
        }

        /// <summary>
        /// Null Object pattern for <see cref="Source{T}"/> instances.
        /// </summary>
        /// <typeparam name="TNested">The concrete type of Source</typeparam>
        private class NullSource<TNested> : Source<TNested>
        {
            /// <summary>
            /// Gets true that I am a  <see cref="Source{T}.NullSource{TNested}"/>.
            /// </summary>
            public override bool IsNull => true;
        }

        public virtual string Id => string.Empty;
    }
}