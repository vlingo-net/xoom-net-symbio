// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Streams;

namespace Vlingo.Xoom.Symbio.Store;

/// <summary>
/// The <see cref="IEntry{T}"/> reader for a given storage type. The specific storage type provides its typed instance.
/// This reads sequentially over all <see cref="IEntry{T}"/> instances in the entire storage, from the
/// first written <see cref="IEntry{T}"/> to the current last written <see cref="IEntry{T}"/>, and is prepared to read
/// all newly appended <see cref="IEntry{T}"/> instances beyond that point when they become available.
/// <para>
/// The <c>IEntryReader</c> implementor may choose to provide the optional "gap prevention." Gaps
/// may occur in databases that support sequences or auto-increment columns used as a total ordering
/// for the <see cref="Journal"/> or other <see cref="IEntry{T}"/> store. This happens when a sequenced value is
/// obtained, but the table row inserts are not serialized in the same order. The threads inserting
/// race to the physical writes, causing logical ordering with gaps for small time windows. Thus, a
/// range query may see gaps in the sequences if run during inserts at the tail of the table rows.
/// Performing gap prevention involves time-delayed retries, allowing the database inserts to fill
/// the gaps caused by thread races. The gaps are easy to detect, but since they may also be caused
/// by transactions that roll back where with sequence values that are wasted/lost, following the
/// number of retries with time delays the reader should still provide results with gaps. This
/// recognizes that following best effort the gaps may never close, and thus be warranted.
/// </para>
/// </summary>
public interface IEntryReader
{
    /// <summary>
    /// A means to seek to the first id position of the storage. This constant
    /// must be honored by all <see cref="IEntry{T}"/> storage implementations regardless
    /// of internal id type.
    /// </summary>
    string Beginning { get; }
        
    /// <summary>
    /// A means to seek past the last id position of the storage. This constant
    /// must be honored by all <see cref="IEntry{T}"/> storage implementations regardless
    /// of internal id type.
    /// </summary>
    string End { get; }
        
    /// <summary>
    /// A means to query the current id position of the journal without repositioning.
    /// This constant must be honored by all <see cref="IEntry{T}"/> storage implementations
    /// regardless of internal id type.
    /// </summary>
    string Query { get; }
        
    /// <summary>
    /// Gets the default number of retries used to prevent gaps in feed items.
    /// </summary>
    int DefaultGapPreventionRetries { get; }
        
    /// <summary>
    /// Gets the default interval between retries used to prevent gaps in feed items.
    /// </summary>
    long DefaultGapPreventionRetryInterval { get; }

    /// <summary>
    /// Closes this reader.
    /// </summary>
    void Close();
        
    /// <summary>
    /// Gets eventually the name of this reader.
    /// </summary>
    ICompletes<string> Name { get; }

    /// <summary>
    /// Eventually answers the next available <see cref="IEntry{T}"/> instance or null if none is currently available.
    /// The next <see cref="IEntry{T}"/> instance is relative to the one previously read by the same reader
    /// instance, or the first <see cref="IEntry{T}"/> instance in the storage if none have previously
    /// been read. Note that this is the least efficient read, because only one <see cref="IEntry{T}"/> will
    /// be answered, but it may be useful for test purposes or a storage that is
    /// appended to slowly.
    /// </summary>
    /// <returns>The <see cref="ICompletes{T}"/> next available entry or null if none</returns>
    ICompletes<IEntry> ReadNext();

    /// <summary>
    /// Eventually answers the next available <see cref="IEntry{T}"/> instance or null if none is currently available.
    /// The next <see cref="IEntry{T}"/> instance is relative to the one previously read by the same reader
    /// instance, or the first <see cref="IEntry{T}"/> instance in the storage if none have previously
    /// been read. Note that this is the least efficient read, because only one <see cref="IEntry{T}"/> will
    /// be answered, but it may be useful for test purposes or a storage that is
    /// appended to slowly.
    /// </summary>
    /// <param name="fromId">The string id of the <see cref="IEntry{T}"/> instance to which the seek prepares to next read</param>
    /// <returns>The <see cref="ICompletes{T}"/> next available entry or null if none</returns>
    ICompletes<IEntry> ReadNext(string fromId);

    /// <summary>
    /// Eventually answers the next available <see cref="IEntry{T}"/> instances as a <see cref="IEnumerable{T}"/>, which may be
    /// empty if none are currently available. The next <see cref="IEntry{T}"/> instances are relative to the one(s)
    /// previously read by the same reader instance, or the first <see cref="IEntry{T}"/> instance in the storage
    /// if none have previously been read. Note that this is the most efficient read, because up to
    /// <paramref name="maximumEntries"/> <see cref="IEntry{T}"/> instances will be answered. The <paramref name="maximumEntries"/> should be used to indicate
    /// the total number of <see cref="IEntry{T}"/> instances that can be consumed in a timely fashion by the sender,
    /// which is a natural back-pressure mechanism.
    /// </summary>
    /// <param name="maximumEntries">The int indicating the maximum number of <see cref="IEntry{T}"/> instances to read</param>
    /// <returns>The <see cref="ICompletes{T}"/> enumerable of <code>T</code> of at most maximumEntries or empty if none</returns>
    ICompletes<IEnumerable<IEntry>> ReadNext(int maximumEntries);

    /// <summary>
    /// Eventually answers the next available <see cref="IEntry{T}"/> instances as a <see cref="IEnumerable{T}"/>, which may be
    /// empty if none are currently available. The next <see cref="IEntry{T}"/> instances are relative to the one(s)
    /// previously read by the same reader instance, or the first <see cref="IEntry{T}"/> instance in the storage
    /// if none have previously been read. Note that this is the most efficient read, because up to
    /// <paramref name="maximumEntries"/> <see cref="IEntry{T}"/> instances will be answered. The <paramref name="maximumEntries"/> should be used to indicate
    /// the total number of <see cref="IEntry{T}"/> instances that can be consumed in a timely fashion by the sender,
    /// which is a natural back-pressure mechanism.
    /// </summary>
    /// <param name="fromId">The string id of the <see cref="IEntry{T}"/> instance to which the seek prepares to next read</param>
    /// <param name="maximumEntries">The int indicating the maximum number of <see cref="IEntry{T}"/> instances to read</param>
    /// <returns>The <see cref="ICompletes{T}"/> enumerable of <code>T</code> of at most maximumEntries or empty if none</returns>
    ICompletes<IEnumerable<IEntry>> ReadNext(string fromId, int maximumEntries);

    /// <summary>
    /// Rewinds the reader so that the next available <see cref="IEntry{T}"/> is the first one in the storage.
    /// Sending <code>Rewind()</code> is the same as sending <code>SeekTo(beginning)</code>.
    /// </summary>
    void Rewind();

    /// <summary>
    /// Eventually answers the new position of the reader after attempting to seek to the <see cref="IEntry{T}"/> of the
    /// given id, such that the next available <see cref="IEntry{T}"/> is the one of the given id. If the id does not (yet)
    /// exist, the position is set to just beyond the last <see cref="IEntry{T}"/> instance in the journal (see <code>End</code>), or to
    /// being the first (see <code>Beginning</code>) if none currently exist. For example, if the storage id type is a
    /// long, passing <code>"-1"</code> will cause the position to be set just beyond the last <see cref="IEntry{T}"/> instance in
    /// the storage, or to the beginning if no instances exist. Passing the <code>string "="</code> (see <code>Query</code>)
    /// answers the current id position without attempting to seek in either direction. (Seeking relative
    /// to the current id position is implementation specific and is not expected to be supported by any
    /// given implementation.)
    /// </summary>
    /// <param name="id">the String id of the <see cref="IEntry{T}"/> instance to which the seek prepares to next read</param>
    /// <returns><see cref="ICompletes{T}"/> where <code>T is string</code></returns>
    ICompletes<string> SeekTo(string id);
        
    /// <summary>
    /// Eventually answer the size in <see cref="IEntry{T}"/> instances. If the size
    /// is not known or not queryable, the value of <code>-1L</code> is answered.
    /// </summary>
    ICompletes<long> Size { get; }
        
    /// <summary>
    /// Answer a new <see cref="IStream"/> for flowing all <see cref="IEntry{T}"/> instances in total time order.
    /// </summary>
    /// <returns><see cref="ICompletes{IStream}"/></returns>
    ICompletes<IStream> StreamAll();
}

/// <summary>
/// Default values for <see cref="IEntryReader"/>
/// </summary>
public static class EntryReader
{
    public const string Beginning = "<";
        
    public const string End = ">";
        
    public const string Query = "=";

    public const int DefaultGapPreventionRetries = 3;

    public const long DefaultGapPreventionRetryInterval = 10L;
}

/// <summary>
/// Provides advice for the specific implementation.
/// </summary>
public class Advice
{
    public object Configuration { get; }
        
    public Type EntryReaderClass { get; }
        
    public string QueryCount { get; }
        
    public string QueryLatestOffset { get; }
        
    public string QueryEntryBatchExpression { get; }
        
    public string QueryEntryIdsExpression { get; }
        
    public string QueryEntryExpression { get; }
        
    public string QueryUpdateCurrentOffset { get; }

    public T SpecificConfiguration<T>() => (T) Configuration;

    public Advice(
        object configuration,
        Type entryReaderClass,
        string queryCount,
        string queryLatestOffset,
        string queryEntryBatchExpression,
        string queryEntryIdsExpression,
        string queryEntryExpression,
        string queryUpdateCurrentOffset)
    {
        Configuration = configuration;
        EntryReaderClass = entryReaderClass;
        QueryCount = queryCount;
        QueryLatestOffset = queryLatestOffset;
        QueryEntryBatchExpression = queryEntryBatchExpression;
        QueryEntryIdsExpression = queryEntryIdsExpression;
        QueryEntryExpression = queryEntryExpression;
        QueryUpdateCurrentOffset = queryUpdateCurrentOffset;
    }
        
    /// <summary>
    /// Provides values to assist in detecting and preventing
    /// gaps between entries in a stream.
    /// </summary>
    public static class GapPrevention
    {
        /// <summary>
        /// Gets the number of retries used to prevent gaps in <see cref="IEntry{T}"/> instances
        /// due to race conditions in database sequences and transactions.
        /// </summary>
        public static int Retries => EntryReader.DefaultGapPreventionRetries;

        /// <summary>
        /// the interval between retries used to prevent gaps in <see cref="IEntry{T}"/>
        /// instances due to race conditions in database sequences and transactions.
        /// </summary>
        public static long RetryInterval => EntryReader.DefaultGapPreventionRetryInterval;
    }
}