// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Common;

namespace Vlingo.Symbio.Store.Object
{
    /// <summary>
    /// Defines protocol for reading from an <see cref="IObjectStore"/>
    /// </summary>
    public interface IObjectStoreReader
    {
        /// <summary>
        /// Gets the value given to non-identity ids.
        /// </summary>
        long NoId { get; }

        /// <summary>
        /// Answer whether or not <paramref name="id"/> is the <code>NoId</code>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><code>True</code> if the <paramref name="id"/> is not an identity, otherwise false.</returns>
        bool IsNoId(long id);

        /// <summary>
        /// Answer whether or not <paramref name="id"/> is an identity.
        /// </summary>
        /// <param name="id">id the identity to check</param>
        /// <returns><code>True</code> if the <paramref name="id"/> is an identity, otherwise false.</returns>
        bool IsId(long id);

        ICompletes<IEntryReader<TNewEntry>> EntryReader<TNewEntry>(string name) where TNewEntry : IEntry;

        /// <summary>
        /// Executes the query defined by <paramref name="expression"/> that may result in one object,
        /// and sends the result to <paramref name="interest"/>.
        /// </summary>
        /// <param name="expression">The expression of <see cref="QueryExpression"/></param>
        /// <param name="interest">The <see cref="IQueryResultInterest"/></param>
        void QueryAll(QueryExpression expression, IQueryResultInterest interest);

        /// <summary>
        /// Executes the query defined by <paramref name="expression"/> that may result in one object,
        /// and sends the result to <paramref name="interest"/>.
        /// </summary>
        /// <param name="expression">The expression of <see cref="QueryExpression"/></param>
        /// <param name="interest">The <see cref="IQueryResultInterest"/></param>
        /// <param name="object">An object sent to the <see cref="IQueryResultInterest"/> when the query has succeeded or failed</param>
        void QueryAll(QueryExpression expression, IQueryResultInterest interest, object? @object);

        /// <summary>
        /// Executes the query defined by <paramref name="expression"/> that may result in one object,
        /// and sends the result to <paramref name="interest"/>.
        /// </summary>
        /// <param name="expression">The <see cref="QueryExpression"/></param>
        /// <param name="interest">The <see cref="IQueryResultInterest"/></param>
        void QueryObject(QueryExpression expression, IQueryResultInterest interest);

        /// <summary>
        /// Executes the query defined by <paramref name="expression"/> that may result in one object,
        /// and sends the result to <paramref name="interest"/>.
        /// </summary>
        /// <param name="expression">The <see cref="QueryExpression"/></param>
        /// <param name="interest">The <see cref="IQueryResultInterest"/></param>
        /// <param name="object">An object sent to the <see cref="IQueryResultInterest"/> when the query has succeeded or failed</param>
        void QueryObject(QueryExpression expression, IQueryResultInterest interest, object? @object);
    }

    public static class ObjectStoreReader
    {
        public static long NoId { get; } = -1L;

        public static bool IsNoId(long id) => NoId == id;
        
        public static bool IsId(long id) => id > NoId;
    }
}