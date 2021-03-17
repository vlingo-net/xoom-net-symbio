// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
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
        /// Persists the new <paramref name="stateSources"/> with sources.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateSources"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateSources"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateSources"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="stateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="stateSources">The Object to persist with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="stateSources"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void Persist<TState, TSource>(StateSources<TState, TSource> stateSources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;

        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;

        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="allStateSources"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;

        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="allStateSources"/> and sources</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="allStateSources"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, long updateId, IPersistResultInterest interest) where TState : StateObject where TSource : ISource;
               
        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources as new or updated depending on the value of <paramref name="updateId"/>.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
        
        /// <summary>
        /// Persists the new <paramref name="allStateSources"/> with sources and <see cref="Metadata"/>.
        /// </summary>
        /// <param name="allStateSources">The Objects to persist, each with the domain events that were its source of truth</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <paramref name="allStateSources"/> and sources</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="interest">The <see cref="IPersistResultInterest"/> to which the result is dispatched</param>
        /// <param name="object">An object sent to the <see cref="IPersistResultInterest"/> when the persist has succeeded or failed</param>
        void PersistAll<TState, TSource>(IEnumerable<StateSources<TState, TSource>> allStateSources, Metadata metadata, long updateId, IPersistResultInterest interest, object? @object) where TState : StateObject where TSource : ISource;
    }
}