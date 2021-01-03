// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common.Serialization;
using Vlingo.Symbio.Tests.Store.Journal.InMemory;

namespace Vlingo.Symbio.Tests.Store.State
{
    public class SnapshotStateAdapter : IStateAdapter<SnapshotState, TextState>
    {
        public int TypeVersion { get; } = 1;
        
        public SnapshotState FromRawState(TextState raw) => (SnapshotState)JsonSerialization.Deserialized(raw.Data, raw.Typed);
        

        public TOtherState FromRawState<TOtherState>(TextState raw) => (TOtherState)JsonSerialization.Deserialized(raw.Data, typeof(TOtherState));

        public TextState ToRawState(string id, SnapshotState state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(id, typeof(SnapshotState), TypeVersion, serialization, stateVersion, metadata);
        }

        public TextState ToRawState(SnapshotState state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(TextState.NoOp, typeof(SnapshotState), TypeVersion, serialization, stateVersion, metadata);
        }

        public TextState ToRawState(SnapshotState state, int stateVersion) =>
            ToRawState(state, stateVersion, Metadata.NullMetadata());

        public object ToRawState<T>(T state, int stateVersion, Metadata metadata)
            => ToRawState((SnapshotState)(object)state, stateVersion, metadata);
    }
}