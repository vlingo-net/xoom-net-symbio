// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common.Serialization;
using Vlingo.Symbio.Tests.Store.Journal.InMemory;

namespace Vlingo.Symbio.Tests.Store.State
{
    public class SnapshotStateAdapter : IStateAdapter<SnapshotState, string>
    {
        public int TypeVersion { get; } = 1;
        
        public SnapshotState FromRawState(State<string> raw) => (SnapshotState)JsonSerialization.Deserialized(raw.Data, raw.Typed);
        

        public TOtherState FromRawState<TOtherState>(State<string> raw) => (TOtherState)JsonSerialization.Deserialized(raw.Data, typeof(TOtherState));

        public State<string> ToRawState(string id, SnapshotState state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(id, typeof(SnapshotState), TypeVersion, serialization, stateVersion, metadata);
        }

        public State<string> ToRawState(SnapshotState state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(TextState.NoOp, typeof(SnapshotState), TypeVersion, serialization, stateVersion, metadata);
        }

        public State<string> ToRawState(SnapshotState state, int stateVersion) =>
            ToRawState(state, stateVersion, Metadata.NullMetadata());

        public object ToRawState<T>(T state, int stateVersion, Metadata metadata)
            => ToRawState((SnapshotState)(object)state, stateVersion, metadata);
    }
}