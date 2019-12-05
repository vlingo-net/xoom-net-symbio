// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// Defines protocol for writing to an <see cref="IObjectStore"/>.
    /// </summary>
    public interface IObjectStoreWriter
    {
        /// <summary>
        /// Persists the new <paramref name="stateObject"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState>(TState stateObject, IPersistResultInterest interest) where TState : StateObject;

        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState>(TState stateObject, Metadata metadata, IPersistResultInterest interest) where TState : StateObject;

        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, IPersistResultInterest interest) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, Metadata metadata, IPersistResultInterest interest) where TState : StateObject where TSource : Source;

        /// <summary>
        /// Persists the new <paramref name="stateObject"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState>(TState stateObject, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState>(TState stateObject, Metadata metadata, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, Metadata metadata, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;

        /// <summary>
        /// Persists the <paramref name="stateObject"/> as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState>(TState stateObject, long updateId, IPersistResultInterest interest) where TState : StateObject;

        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState>(TState stateObject, Metadata metadata, long updateId, IPersistResultInterest interest) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/> as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, Metadata metadata, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the <paramref name="stateObject"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState>(TState stateObject, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState>(TState stateObject, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/> as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObject"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObject">The concrete implementation of <see cref="StateObject"/> to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObject"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(TState stateObject, IEnumerable<TSource> sources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;

        /// <summary>
        /// Persists the new <paramref name="stateObjects"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, IPersistResultInterest interest) where TState : StateObject;

        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata, IPersistResultInterest interest) where TState : StateObject;

        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, IPersistResultInterest interest) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, Metadata metadata, IPersistResultInterest interest) where TState : StateObject where TSource : Source;

        /// <summary>
        /// Persists the new <paramref name="stateObjects"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, Metadata metadata, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;

        /// <summary>
        /// Persists the <paramref name="stateObjects"/> as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, long updateId, IPersistResultInterest interest) where TState : StateObject;

        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata, long updateId, IPersistResultInterest interest) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/> as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, Metadata metadata, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the <paramref name="stateObjects"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState>(IEnumerable<TState> stateObjects, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/> as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;
        
        /// <summary>
        /// Persists the new <paramref name="stateObjects"/> with <see cref="IEnumerable{T}"/> and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateObjects">The concrete implementation of <see cref="StateObject"/>s to persist</param>
        /// <param name="sources">The domain events to journal related to <code>stateObject</code></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateObjects"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<TState> stateObjects, IEnumerable<TSource> sources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : Source;
    }
}