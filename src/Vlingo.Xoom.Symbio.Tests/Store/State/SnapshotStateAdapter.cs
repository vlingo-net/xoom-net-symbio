// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Common.Serialization;
using Vlingo.Xoom.Symbio.Tests.Store.Journal.InMemory;

namespace Vlingo.Xoom.Symbio.Tests.Store.State
{
    public class SnapshotStateAdapter : IStateAdapter<SnapshotState, IState>
    {
        public int TypeVersion { get; } = 1;
        
        public SnapshotState FromRawState(IState raw) => (SnapshotState)JsonSerialization.Deserialized(raw.RawData, raw.Typed);
        

        public TOtherState FromRawState<TOtherState>(IState raw) => JsonSerialization.Deserialized<TOtherState>(raw.RawData);

        public IState ToRawState(string id, SnapshotState state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(id, typeof(SnapshotState), TypeVersion, serialization, stateVersion, metadata);
        }

        public IState ToRawState(SnapshotState state, int stateVersion, Metadata metadata)
        {
            var serialization = JsonSerialization.Serialized(state);
            return new TextState(TextState.NoOp, typeof(SnapshotState), TypeVersion, serialization, stateVersion, metadata);
        }

        public IState ToRawState(SnapshotState state, int stateVersion) =>
            ToRawState(state, stateVersion, Metadata.NullMetadata());

        public object ToRawState<T>(T state, int stateVersion, Metadata metadata)
            => ToRawState((SnapshotState)(object)state, stateVersion, metadata);
    }
}