// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Common;

namespace Vlingo.Symbio.Store.Journal
{
    /// <summary>
    /// The means by which the <see cref="IJournal{T}"/> informs the sender of the result of any given append.
    /// </summary>
    public interface IAppendResultInterest
    {
        /// <summary>
        /// Conveys the <code>IOutcome{TError, TResult}</code> of a single appended <see cref="Source{T}"/> and a possible state <paramref name="snapshot"/>.
        /// </summary>
        /// <param name="outcome">The <see cref="IOutcome{TError, TResult}"/> either failure or success</param>
        /// <param name="streamName">The String name of the stream appended</param>
        /// <param name="streamVersion">The int version of the stream appended</param>
        /// <param name="source">The <see cref="Source{T}"/> that was appended</param>
        /// <param name="snapshot">The possible <see cref="Optional{T}"/> that may have persisted as the stream's most recent snapshot</param>
        /// <param name="object">The object supplied by the sender to be sent back in this result</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, Source<TSource> source, Optional<TSnapshotState> snapshot, object @object);

        /// <summary>
        /// Conveys the <code>IOutcome{TError, TResult}</code> of a single appended <see cref="Source{T}"/> and a possible state <paramref name="snapshot"/>.
        /// </summary>
        /// <param name="outcome">The <see cref="IOutcome{TError, TResult}"/> either failure or success</param>
        /// <param name="streamName">The String name of the stream appended</param>
        /// <param name="streamVersion">The int version of the stream appended</param>
        /// <param name="source">The <see cref="Source{T}"/> that was appended</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <see cref="Source{T}"/></param>
        /// <param name="snapshot">The possible <see cref="Optional{T}"/> that may have persisted as the stream's most recent snapshot</param>
        /// <param name="object">The object supplied by the sender to be sent back in this result</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, Source<TSource> source, Metadata metadata, Optional<TSnapshotState> snapshot, object @object);
        
        /// <summary>
        /// Conveys the <code>IOutcome{TError, TResult}</code> of attempting to append multiple <see cref="Source{T}"/>s and a possible state <paramref name="snapshot"/>.
        /// </summary>
        /// <param name="outcome">The <see cref="IOutcome{TError, TResult}"/> either failure or success</param>
        /// <param name="streamName">The String name of the stream appended</param>
        /// <param name="streamVersion">The int version of the stream appended</param>
        /// <param name="sources">The multiple <see cref="Source{T}"/>s that were appended</param>
        /// <param name="snapshot">The possible <see cref="Optional{T}"/> that may have persisted as the stream's most recent snapshot</param>
        /// <param name="object">The object supplied by the sender to be sent back in this result</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendAllResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, IEnumerable<Source<TSource>> sources, Optional<TSnapshotState> snapshot, object @object);

        /// <summary>
        /// Conveys the <code>IOutcome{TError, TResult}</code> of attempting to append multiple <see cref="Source{T}"/>s and a possible state <paramref name="snapshot"/>.
        /// </summary>
        /// <param name="outcome">The <see cref="IOutcome{TError, TResult}"/> either failure or success</param>
        /// <param name="streamName">The String name of the stream appended</param>
        /// <param name="streamVersion">The int version of the stream appended</param>
        /// <param name="sources">The multiple <see cref="Source{T}"/>s that were appended</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <see cref="Source{T}"/></param>
        /// <param name="snapshot">The possible <see cref="Optional{T}"/> that may have persisted as the stream's most recent snapshot</param>
        /// <param name="object">The object supplied by the sender to be sent back in this result</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendAllResultedIn<TSource, TSnapshotState>(IOutcome<StorageException, Result> outcome, string streamName, int streamVersion, IEnumerable<Source<TSource>> sources, Metadata metadata, Optional<TSnapshotState> snapshot, object @object);
    }
}