// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    /// Used for writing and reading multiple states in one batch.
    /// </summary>
    public class TypedStateBundle
    {
        public string? Id { get; }
        public Type? Type { get; }
        public object? State { get; }
        public int StateVersion { get; }
        public Metadata? Metadata { get; }

        public TypedStateBundle(string? id, Type? type, object? state, int stateVersion, Metadata? metadata)
        {
            Id = id;
            Type = type;
            State = state;
            StateVersion = stateVersion;
            Metadata = metadata;
        }

        public TypedStateBundle(string? id, object? state, int stateVersion, Metadata? metadata)
            : this(id, null, state, stateVersion, metadata)
        {
        }

        public TypedStateBundle(string? id, Type type) : this(id, type, null, 0, null)
        {
        }
    }
    
    public class TypedStateBundle<T> : TypedStateBundle
    {
        public T TypedState => (T) State!;
        
        public TypedStateBundle(string? id, Type? type, object? state, int stateVersion, Metadata? metadata)
            : base(id, type, state, stateVersion, metadata)
        {
        }

        public TypedStateBundle(string? id, object state, int stateVersion, Metadata metadata)
            : base(id, state, stateVersion, metadata)
        {
        }

        public TypedStateBundle(string? id, Type type) : base(id, type)
        {
        }
    }
}