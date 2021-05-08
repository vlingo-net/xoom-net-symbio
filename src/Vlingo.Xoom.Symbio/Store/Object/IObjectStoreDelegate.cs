// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Symbio.Store.Dispatch;

namespace Vlingo.Xoom.Symbio.Store.Object
{
    public interface IObjectStoreDelegate : IDispatcherControlDelegate
    {
        /// <summary>
        /// Gets all registered <see cref="StateObjectMapper"/>s
        /// </summary>
        IEnumerable<StateObjectMapper> RegisteredMappers { get; }

        /// <summary>
        /// Register the <paramref name="mapper"/> for a given persistent type.
        /// </summary>
        /// <param name="mapper">The <see cref="StateObjectMapper"/></param>
        void RegisterMapper(StateObjectMapper mapper);

        /// <summary>
        /// Close me.
        /// </summary>
        void Close();
        
        /// <summary>
        /// Copy this delegate.
        /// </summary>
        /// <returns>A copy of this delegate</returns>
        IObjectStoreDelegate Copy();
        
        /// <summary>
        /// Begin store transaction.
        /// </summary>
        void BeginTransaction();
        
        /// <summary>
        /// Complete store transaction.
        /// </summary>
        void CompleteTransaction();
        
        /// <summary>
        /// Fail store transaction.
        /// </summary>
        void FailTransaction();

        /// <summary>
        /// Persists the <paramref name="stateObjects"/> with <paramref name="metadata"/>.
        /// </summary>
        /// <param name="stateObjects">All the <paramref name="stateObjects"/> to persist</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the stateObjects and sources</param>
        /// <returns>The persisted collection of states, created from <paramref name="stateObjects"/></returns>
        IEnumerable<IState> PersistAll(IEnumerable<StateObject> stateObjects, long updateId, Metadata metadata);
        
        /// <summary>
        /// Persists the <paramref name="stateObject"/> with <paramref name="metadata"/>.
        /// </summary>
        /// <param name="stateObject">The <paramref name="stateObject"/> to persist</param>
        /// <param name="updateId">The long identity to facilitate update; &lt; 0 for create &gt; 0 for update</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the stateObjects and sources</param>
        /// <returns>The persisted state, created from <paramref name="stateObject"/></returns>
        IState Persist(StateObject stateObject, long updateId, Metadata metadata);

        /// <summary>
        /// Persist the <code>IEnumerable{IEntry}</code> of entries, that originated from sources.
        /// </summary>
        /// <param name="entries"><code>IEnumerable{TEntry}</code></param>
        void PersistEntries(IEnumerable<IEntry> entries);

        /// <summary>
        /// Persist the <code>Dispatchable</code> that originated from write.
        /// </summary>
        /// <param name="dispatchable"><see cref="Dispatchable"/></param>
        void PersistDispatchable(Dispatchable dispatchable);

        /// <summary>
        /// Executes the query defined by <paramref name="expression"/> that may result in zero to many objects.
        /// </summary>
        /// <param name="expression">The <see cref="QueryExpression"/></param>
        /// <returns><see cref="QueryMultiResults"/> with objects that matches the expression.</returns>
        QueryMultiResults QueryAll(QueryExpression expression);
        
        /// <summary>
        /// Executes the query defined by <paramref name="expression"/> that may result in one objects.
        /// </summary>
        /// <param name="expression">The <see cref="QueryExpression"/></param>
        /// <returns><see cref="QuerySingleResult"/> with object that matches the expression.</returns>
        QuerySingleResult QueryObject(QueryExpression expression);
    }
}