// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Symbio.Store.Dispatch;

namespace Vlingo.Symbio.Store.Journal
{
    public interface IJournal
    {
        /// <summary>
        /// Appends the single <see cref="Source{T}"/> as an <see cref="IEntry{T}"/> to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="streamVersion"/>. The <see cref="Source{T}"/>
        /// is translated to a corresponding <see cref="IEntry{T}"/> with an unknown id. If there is a registered
        /// <code>JournalListener{T}</code>, it will be informed of the newly appended <see cref="IEntry{T}"/> and it
        /// will have an assigned valid id.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="streamVersion">The int version of the stream to append</param>
        /// <param name="source">The <see cref="Source{T}"/> to append as an <see cref="IEntry{T}"/></param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        void Append<TSource>(string streamName, int streamVersion, TSource source, IAppendResultInterest interest, object @object) where TSource : ISource;

        /// <summary>
        /// Appends the single <see cref="Source{T}"/> as an <see cref="IEntry{T}"/> to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="streamVersion"/>. The <see cref="Source{T}"/>
        /// is translated to a corresponding <see cref="IEntry{T}"/> with an unknown id. If there is a registered
        /// <code>JournalListener{T}</code>, it will be informed of the newly appended <see cref="IEntry{T}"/> and it
        /// will have an assigned valid id.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="streamVersion">The int version of the stream to append</param>
        /// <param name="source">The <see cref="Source{T}"/> to append as an <see cref="IEntry{T}"/></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <see cref="Source{T}"/></param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        void Append<TSource>(string streamName, int streamVersion, TSource source, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : ISource;

        /// <summary>
        /// Appends the single <see cref="Source{T}"/> as an <see cref="IEntry{T}"/> to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="streamVersion"/>, and storing
        /// the full state <paramref name="snapshot"/>. The corresponding <see cref="IEntry{T}"/> is internally
        /// assigned an id. The entry and snapshot are consistently persisted together or neither
        /// at all. If there is a registered <code>JournalListener{T}</code>, it will be informed of
        /// the newly appended <see cref="IEntry{T}"/> with an assigned valid id and the new <typeparamref name="TSnapshotState"/> snapshot.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="streamVersion">The int version of the stream to append</param>
        /// <param name="source">The <see cref="Source{T}"/> to append as an <see cref="IEntry{T}"/></param>
        /// <param name="snapshot">The current <typeparamref name="TSnapshotState"/> state of the stream before the source is applied</param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource;

        /// <summary>
        /// Appends the single <see cref="Source{T}"/> as an <see cref="IEntry{T}"/> to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="streamVersion"/>, and storing
        /// the full state <paramref name="snapshot"/>. The corresponding <see cref="IEntry{T}"/> is internally
        /// assigned an id. The entry and snapshot are consistently persisted together or neither
        /// at all. If there is a registered <code>JournalListener{T}</code>, it will be informed of
        /// the newly appended <see cref="IEntry{T}"/> with an assigned valid id and the new <typeparamref name="TSnapshotState"/> snapshot.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="streamVersion">The int version of the stream to append</param>
        /// <param name="source">The <see cref="Source{T}"/> to append as an <see cref="IEntry{T}"/></param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <see cref="Source{T}"/></param>
        /// <param name="snapshot">The current <typeparamref name="TSnapshotState"/> state of the stream before the source is applied</param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource;
        
        /// <summary>
        /// Appends all <see cref="Source{T}"/> instances as an <see cref="IEntry{T}"/> to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="fromStreamVersion"/>. If there is a registered
        /// <code>JournalListener{T}</code>, it will be informed of the newly appended <see cref="IEntry{T}"/> instances and each
        /// will have an assigned valid id.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="fromStreamVersion">The int version of the stream to append</param>
        /// <param name="sources">The list of <see cref="Source{T}"/> to append as a list of <see cref="IEntry{T}"/> instances</param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, IAppendResultInterest interest, object @object) where TSource : ISource;
        
        /// <summary>
        /// Appends all <see cref="Source{T}"/> instances as an <see cref="IEntry{T}"/> to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="fromStreamVersion"/>. If there is a registered
        /// <code>JournalListener{T}</code>, it will be informed of the newly appended <see cref="IEntry{T}"/> instances and each
        /// will have an assigned valid id.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="fromStreamVersion">The int version of the stream to append</param>
        /// <param name="sources">The list of <see cref="Source{T}"/> to append as a list of <see cref="IEntry{T}"/> instances</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <see cref="Source{T}"/></param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : ISource;
        
        /// <summary>
        /// Appends all <see cref="Source{T}"/> instances as an <see cref="IEntry{T}"/> instances to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="fromStreamVersion"/>, and storing
        /// the full state <paramref name="snapshot"/>. The corresponding <see cref="IEntry{T}"/> is internally
        /// assigned an id. The entries and snapshot are consistently persisted together or none
        /// at all. If there is a registered <code>JournalListener{T}</code>, it will be informed of
        /// the newly appended <see cref="IEntry{T}"/> with an assigned valid ids and the new <typeparamref name="TSnapshotState"/> snapshot.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="fromStreamVersion">The int version of the stream to append</param>
        /// <param name="sources">The list of <see cref="Source{T}"/> to append as a list of <see cref="IEntry{T}"/> instances</param>
        /// <param name="snapshot">The current <typeparamref name="TSnapshotState"/> state of the stream before the source is applied</param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource;
        
        /// <summary>
        /// Appends all <see cref="Source{T}"/> instances as an <see cref="IEntry{T}"/> instances to the end of the journal
        /// creating an association to <paramref name="streamName"/> with <paramref name="fromStreamVersion"/>, and storing
        /// the full state <paramref name="snapshot"/>. The corresponding <see cref="IEntry{T}"/> is internally
        /// assigned an id. The entries and snapshot are consistently persisted together or none
        /// at all. If there is a registered <code>JournalListener{T}</code>, it will be informed of
        /// the newly appended <see cref="IEntry{T}"/> with an assigned valid ids and the new <typeparamref name="TSnapshotState"/> snapshot.
        /// </summary>
        /// <param name="streamName">The string name of the stream to append</param>
        /// <param name="fromStreamVersion">The int version of the stream to append</param>
        /// <param name="sources">The list of <see cref="Source{T}"/> to append as a list of <see cref="IEntry{T}"/> instances</param>
        /// <param name="metadata">The <see cref="Metadata"/> associated with the <see cref="Source{T}"/></param>
        /// <param name="snapshot">The current <typeparamref name="TSnapshotState"/> state of the stream before the source is applied</param>
        /// <param name="interest">The Actor-backed <see cref="IAppendResultInterest"/> used to convey the result of the append</param>
        /// <param name="object">The object from the sender that is to be included in the <see cref="IAppendResultInterest"/> response</param>
        /// <typeparam name="TSource">The <see cref="Source{T}"/> type</typeparam>
        /// <typeparam name="TSnapshotState">The snapshot state type</typeparam>
        void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource;

        /// <summary>
        /// Eventually answers the <see cref="IJournalReader{TEntry}"/> named <paramref name="name"/> for this journal. If
        /// the reader named <paramref name="name"/> does not yet exist, it is first created. Readers
        /// with different names enables reading from different positions and for different
        /// reasons. For example, some readers may be interested in publishing <see cref="IEntry{T}"/>
        /// instances messaging while others may be projecting and building pipelines
        /// of new streams.
        /// </summary>
        /// <param name="name">The string name of the <see cref="IJournalReader{TEntry}"/> to answer</param>
        /// <returns><see cref="ICompletes{T}"/> of <see cref="IJournalReader{TEntry}"/></returns>
        ICompletes<IJournalReader<IEntry>?> JournalReader(string name);
        
        /// <summary>
        /// Eventually answers the <see cref="IStreamReader"/> named <paramref name="name"/> for this journal. If
        /// the reader named <paramref name="name"/> does not yet exist, it is first created. Readers
        /// with different names enables reading from different positions and for different
        /// reasons. For example, some streams may be very busy while others are not.
        /// </summary>
        /// <param name="name">The string name of the <see cref="IStreamReader"/> to answer</param>
        /// <returns><see cref="ICompletes{T}"/> of <see cref="IStreamReader"/></returns>
        ICompletes<IStreamReader?> StreamReader(string name);
    }
    
    /// <summary>
    /// The top-level journal used within a Bounded Context (microservice) to store all of
    /// its <see cref="IEntry{T}"/> instances for <code>EventSourced</code> and <code>CommandSourced</code> components. Each use of
    /// the journal appends some number of <see cref="IEntry{T}"/> instances and perhaps a single snapshot <see cref="State{T}"/>.
    /// The journal may also be queried for a <see cref="IJournalReader{T}"/> and a <see cref="IStreamReader"/>.
    /// Assuming that all successfully appended <see cref="IEntry{T}"/> instances should be dispatched in some way
    /// after each write transaction, you should register an <code>StreamJournalListener{T}</code> when first
    /// creating your <see cref="IStreamReader"/>.
    /// </summary>
    /// <typeparam name="T">The concrete type of <see cref="IEntry{T}"/> and <see cref="State{T}"/> stored, which maybe be string, byte[], or object</typeparam>
    public interface IJournal<T> : IJournal
    {
        /// <summary>
        /// Answer a new <code>IJournal{T}</code>
        /// </summary>
        /// <param name="stage">The Stage within which the <code>IJournal{T}</code> is created</param>
        /// <param name="dispatcher">The <see cref="IDispatcher{TDispatchable}"/></param>
        /// <param name="additional">The object[] of additional parameters</param>
        /// <typeparam name="TActor">The concrete type of the Actor implementing the <code>IJournal{T}</code> protocol</typeparam>
        /// <typeparam name="TState">The raw snapshot state type</typeparam>
        /// <typeparam name="TEntry">The concrete type of journal entries</typeparam>
        /// <returns><code>IJournal{T}</code></returns>
        IJournal<T> Using<TActor, TEntry, TState>(Stage stage, IDispatcher<Dispatchable<TEntry, TState>> dispatcher, params object[] additional) where TActor : Actor  where TEntry : IEntry<T> where TState : class, IState;
        
        /// <summary>
        /// Answer a new <code>IJournal{T}</code>
        /// </summary>
        /// <param name="stage">The Stage within which the <code>IJournal{T}</code> is created</param>
        /// <param name="dispatchers">The <see cref="T:IEnumerable{IDispatcher{TDispatchable}}"/></param>
        /// <param name="additional">The object[] of additional parameters</param>
        /// <typeparam name="TActor">The concrete type of the Actor implementing the <code>IJournal{T}</code> protocol</typeparam>
        /// <typeparam name="TState">The raw snapshot state type</typeparam>
        /// <typeparam name="TEntry">The concrete type of journal entries</typeparam>
        /// <returns><code>IJournal{T}</code></returns>
        IJournal<T> Using<TActor, TEntry, TState>(Stage stage, IEnumerable<IDispatcher<Dispatchable<TEntry, TState>>> dispatchers, params object[] additional) where TActor : Actor  where TEntry : IEntry<T> where TState : class, IState;
    }
    
    /// <summary>
    /// The binary journal as type <code>IJournal{byte[]}</code>.
    /// </summary>
    public interface IBinaryJournal : IJournal<byte[]>
    {}
    
    /// <summary>
    /// The object journal as type <code>IJournal{object}</code>.
    /// </summary>
    public interface IObjectJournal : IJournal<object>
    {}
    
    /// <summary>
    /// The text journal as type <code>IJournal{string}</code>.
    /// </summary>
    public interface ITextJournal : IJournal<string>
    {}
    
    public abstract class Journal<T> : IJournal<T>
    {
        public static IJournal<T> Using<TActor, TEntry, TState>(Stage stage,
            IDispatcher<Dispatchable<TEntry, TState>> dispatcher, params object[] additional)
            where TActor : Actor
            where TEntry : IEntry<T>
            where TState : class, IState
            => Using<TActor, TEntry, TState>(stage, new[] {dispatcher}, additional);

        public static IJournal<T> Using<TActor, TEntry, TState>(Stage stage, IEnumerable<IDispatcher<Dispatchable<TEntry, TState>>> dispatchers, params object[] additional)
            where TActor : Actor
            where TEntry : IEntry<T>
            where TState : class, IState
            => additional.Length == 0 ?
                stage.ActorFor<IJournal<T>>(typeof(TActor), dispatchers) :
                stage.ActorFor<IJournal<T>>(typeof(TActor), dispatchers, additional);
        
        public static IJournal<T> Using<TActor>(Stage stage, params object[] additional)
            where TActor : Actor => additional.Length == 0 ?
                stage.ActorFor<IJournal<T>>(typeof(TActor)) :
                stage.ActorFor<IJournal<T>>(typeof(TActor), additional);

        IJournal<T> IJournal<T>.Using<TActor, TEntry, TState>(Stage stage, IDispatcher<Dispatchable<TEntry, TState>> dispatcher, params object[] additional)
            => Using<TActor, TEntry, TState>(stage, dispatcher, additional);
        
        IJournal<T> IJournal<T>.Using<TActor, TEntry, TState>(Stage stage, IEnumerable<IDispatcher<Dispatchable<TEntry, TState>>> dispatchers, params object[] additional)
            => Using<TActor, TEntry, TState>(stage, dispatchers, additional);

        public virtual void Append<TSource>(string streamName, int streamVersion, TSource source, IAppendResultInterest interest, object @object) where TSource : ISource 
            => Append<TSource>(streamName, streamVersion, source, Metadata.NullMetadata(), interest, @object);

        public abstract void Append<TSource>(string streamName, int streamVersion, TSource source, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : ISource;

        public virtual void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
            => AppendWith(streamName, streamVersion, source, Metadata.NullMetadata(), snapshot, interest, @object);

        public abstract void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource;

        public virtual void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, IAppendResultInterest interest, object @object) where TSource : ISource
            => AppendAll<TSource>(streamName, fromStreamVersion, sources, Metadata.NullMetadata(), interest, @object);

        public abstract void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata, IAppendResultInterest interest, object @object) where TSource : ISource;

        public virtual void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
            => AppendAllWith<TSource, TSnapshotState>(streamName, fromStreamVersion, sources, Metadata.NullMetadata(), snapshot, interest, @object);

        public abstract void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource;

        public abstract ICompletes<IJournalReader<IEntry>?> JournalReader(string name);

        public abstract ICompletes<IStreamReader?> StreamReader(string name);
    }
}