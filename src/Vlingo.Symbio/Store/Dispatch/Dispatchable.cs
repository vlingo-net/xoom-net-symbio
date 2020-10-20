// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Dispatch
{
    /// <summary>
    /// Defines the data holder for identity and state that has been
    /// successfully stored and is then dispatched to registered
    /// interests.
    /// </summary>
    /// <typeparam name="TEntry">The concrete <see cref="IEntry{T}"/> type of the entries</typeparam>
    /// <typeparam name="TState">The concrete <see cref="State{T}"/> type of the storage</typeparam>
    public class Dispatchable<TEntry, TState> where TEntry : IEntry where TState : class, IState
    {
        public Dispatchable(string id, DateTimeOffset createdOn, TState? state, IEnumerable<TEntry> entries)
        {
            Id = id;
            CreatedOn = createdOn;
            State = Optional.OfNullable(state!);
            Entries = new List<TEntry>(entries);
        }

        /// <summary>
        /// My String unique identity.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The moment when I was persistently created.
        /// </summary>
        public DateTimeOffset CreatedOn { get; }

        /// <summary>
        /// My <typeparamref name="TState"/> concrete <see cref="State{T}"/> type.
        /// </summary>
        public Optional<TState> State { get; }

        /// <summary>
        /// My <code>List{TEntry}</code> to dispatch
        /// </summary>
        public List<TEntry> Entries { get; }

        public bool HasEntries => Entries != null && Entries.Any();

        public TNewState TypedState<TNewState>() where TNewState : IState => (TNewState) (object) State.Get()!;

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

            var that = (Dispatchable<TEntry, TState>) obj;
            return Id.Equals(that.Id);
        }

        public override int GetHashCode() => 31 * Id.GetHashCode();
    }
}