// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// A base type for persistent object states.
    /// </summary>
    [Serializable]
    public abstract class StateObject
    {
        private static long _initialVersion = 0L;
        
        /// <summary>
        /// My surrogate (non-business) identity used by the database.
        /// </summary>
        private long _persistenceId = Unidentified;

        /// <summary>
        /// My persistent version, indicating how many state
        /// mutations I have suffered over my lifetime. The default
        /// value is <see cref="_initialVersion"/>.
        /// </summary>
        private long _version = _initialVersion;

        /// <summary>
        /// Answer <paramref name="stateObject"/> as a <code>StateObject</code>.
        /// </summary>
        /// <param name="stateObject"></param>
        /// <returns>Casted <paramref name="stateObject"/> to <code>StateObject</code> or throws <exception cref="InvalidCastException"></exception></returns>
        public static StateObject From(object stateObject) => (StateObject) stateObject;

        /// <summary>
        /// Increments my <see cref="Version"/>. This method is necessary for application-managed optimistic concurrency control, but should
        /// not be used when the persistence mechanism manages this attribute on behalf of the application.
        /// </summary>
        public void IncrementVersion() => _version++;
        
        /// <summary>
        /// Answer a <see cref="IEnumerable{T}"/> that may be used as query parameters.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        public IEnumerable<object> QueryList() => Enumerable.Empty<object>();
        
        /// <summary>
        /// Answer a <code>IDictionary{string, object}</code> that may be used as query parameters.
        /// </summary>
        /// <returns><code>IDictionary{string, object}</code></returns>
        public IDictionary<string, object> QueryMap() => new Dictionary<string, object>();

        internal void SetPersistenceId(long persistenceId) => _persistenceId = persistenceId;

        public override bool Equals(object? obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = (StateObject) obj;
            
            if (_persistenceId != other._persistenceId)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var prime = 31;
            var result = 1;
            result = prime * result + (int) ((ulong)_persistenceId ^ ((ulong)_persistenceId >> 32));
            return result;
        }

        /// <summary>
        /// Gets the value of the unidentified id. May be used by subclasses to indicate they have not yet been persisted.
        /// </summary>
        public static long Unidentified { get; } = -1;

        /// <summary>
        /// Gets my persistence id.
        /// </summary>
        public long PersistenceId => _persistenceId;

        /// <summary>
        /// Gets a value whether or not I am uniquely identified or still awaiting identity assignment.
        /// </summary>
        public bool IsIdentified => _persistenceId != Unidentified;

        /// <summary>
        /// Gets my persistence version, which can be used to implement optimistic concurrency conflict detection.
        /// </summary>
        public long Version => _version;

        /// <summary>
        /// Construct my default state.
        /// </summary>
        protected StateObject()
        {
        }
        
        /// <summary>
        /// Construct my default state with <paramref name="persistenceId"/> and <paramref name="version"/>
        /// </summary>
        /// <param name="persistenceId">The long unique identity used for my persistence</param>
        /// <param name="version">The persistent version</param>
        protected StateObject(long persistenceId, long version)
        {
            _persistenceId = persistenceId;
            _version = version;
        }
        
        /// <summary>
        /// Construct my default state with <paramref name="persistenceId"/>
        /// </summary>
        /// <param name="persistenceId">The long unique identity used for my persistence</param>
        protected StateObject(long persistenceId)
        {
            _persistenceId = persistenceId;
        }
    }
}