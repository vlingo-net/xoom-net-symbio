// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common.Serialization;

namespace Vlingo.Symbio.Tests.Store.State
{
    public class Entity1
    {
        public string Id { get; }
        
        public int Value { get; }

        public Entity1(string id, int value)
        {
            Id = id;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            return Id.Equals(((Entity1) obj).Id);
        }

        public override int GetHashCode() => 31 * Id.GetHashCode();

        public override string ToString() => $"Entity1[id={Id} value={Value}]";
    }
    
    public class Entity1StateAdapter : IStateAdapter<Entity1, string>
    {
        public int TypeVersion { get; } = 1;
        
        public Entity1 FromRawState(State<string> raw) => JsonSerialization.Deserialized<Entity1>(raw.Data);

        public State<TOtherState> FromRawState<TOtherState>(State<string> raw) => JsonSerialization.Deserialized<State<TOtherState>>(raw.Data);

        public State<string> ToRawState(string id, Entity1 state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(id, typeof(Entity1), TypeVersion, serialization, stateVersion, metadata);
        }

        public State<string> ToRawState(Entity1 state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(state.Id, typeof(Entity1), TypeVersion, serialization, stateVersion, metadata);
        }

        public State<string> ToRawState(Entity1 state, int stateVersion) => ToRawState(state, stateVersion, Metadata.With("value", "op"));
    }
}