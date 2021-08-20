// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.State
{
    public sealed class NoOpStateStoreActor<TRawState> : Actor, IStateStore where TRawState : class, IState
    {
        private const string WarningMessage =
            "\n===============================================================================================\n" +
            "                                                                                             \n" +
            " All journal operations are stopped. Please check your DB settings and user credentials.     \n" +
            "                                                                                             \n" +
            "===============================================================================================\n";

        public NoOpStateStoreActor()
        {
            Logger.Warn(WarningMessage);
            Stop();
        }
        
        public void Read<TState>(string id, IReadResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Read<TState>(string id, IReadResultInterest interest, object? @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void ReadAll<TState>(IEnumerable<TypedStateBundle> bundles, IReadResultInterest interest, object? @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources,
            IWriteResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata,
            IWriteResultInterest interest)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState>(string id, TState state, int stateVersion, IWriteResultInterest interest, object @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources,
            IWriteResultInterest interest, object @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState>(string id, TState state, int stateVersion, Metadata metadata, IWriteResultInterest interest,
            object? @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public void Write<TState, TSource>(string id, TState state, int stateVersion, IEnumerable<TSource> sources, Metadata metadata,
            IWriteResultInterest interest, object? @object)
        {
            Logger.Error(WarningMessage);
            ((ICompletes<TRawState>)Completes()).Failed();
        }

        public ICompletes<IStateStoreEntryReader> EntryReader<TEntry>(string name) where TEntry : IEntry
        {
            Logger.Error(WarningMessage);
            return Common.Completes.WithFailure<IStateStoreEntryReader>();
        }

        public override IDeadLetters? DeadLetters
        {
            get
            {
                Logger.Error(WarningMessage);
                return base.DeadLetters;
            }
        }
    }
}