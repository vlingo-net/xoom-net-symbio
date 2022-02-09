// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio.Store.Dispatch
{
    /// <summary>
    /// Defines the data holder for identity and state that has been
    /// successfully stored and is then dispatched to registered
    /// interests.
    /// </summary>
    public class Dispatchable
    {
        public Dispatchable(string id, DateTimeOffset createdOn, IState? state, IEnumerable<IEntry> entries)
        {
            Id = id;
            CreatedOn = createdOn;
            State = Optional.OfNullable(state!);
            Entries = new List<IEntry>(entries);
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
        /// My concrete <see cref="State{T}"/> type.
        /// </summary>
        public Optional<IState> State { get; }

        /// <summary>
        /// My <code>List{TEntry}</code> to dispatch
        /// </summary>
        public List<IEntry> Entries { get; }

        public bool HasEntries => Entries.Any();

        public TNewState TypedState<TNewState>() where TNewState : IState => (TNewState) State.Get()!;

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

            var that = (Dispatchable) obj;
            return Id.Equals(that.Id);
        }

        public override int GetHashCode() => 31 * Id.GetHashCode();
    }
}