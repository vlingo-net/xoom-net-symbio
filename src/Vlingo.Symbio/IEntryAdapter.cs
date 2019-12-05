// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio
{
    /// <summary>
    /// Adapts the native <see cref="Source{T}"/> state to the raw <see cref="IEntry{T}"/>,
    /// and the raw <see cref="IEntry{T}"/> to the native <see cref="Source{T}"/>.
    /// <para>
    /// Note that the <c>id</c> provided herein is the identity assigned by the storage mechanism.
    /// This may be a sequence number or an alphanumeric value. The important thing to note is
    /// that this is provided by storage but may subsequently be used to represent time or order.
    /// </para>
    /// </summary>
    /// <typeparam name="TSource">The native <see cref="Source{T}"/> type.</typeparam>
    /// <typeparam name="TEntry">The raw <see cref="IEntry{T}"/></typeparam>
    public interface IEntryAdapter<TSource, TEntry> where TEntry : IEntry where TSource : Source
    {
        /// <summary>
        /// Gets the <see cref="Source{T}"/> native state from the <see cref="IEntry{T}"/> state.
        /// </summary>
        /// <param name="entry">The <see cref="IEntry{T}"/> to adapt from.</param>
        /// <returns>Adapted <see cref="Source{T}"/>.</returns>
        TSource FromEntry(TEntry entry);

        /// <summary>
        /// Gets the <see cref="IEntry{T}"/> state from the <see cref="Source{T}"/> native state.
        /// </summary>
        /// <param name="source">The <see cref="Source{T}"/> native state.</param>
        /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
        TEntry ToEntry(TSource source);
        
        /// <summary>
        /// Gets the <see cref="IEntry{T}"/> state from the <see cref="Source{T}"/> native state.
        /// </summary>
        /// <param name="source">The <see cref="Source{T}"/> native state.</param>
        /// <param name="metadata">The <see cref="Metadata"/> for this entry.</param>
        /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
        TEntry ToEntry(TSource source, Metadata metadata);

        /// <summary>
        /// Gets the <see cref="IEntry{T}"/> state with its <paramref name="id"/> from the <see cref="Source{T}"/> native state.
        /// </summary>
        /// <param name="source">The <see cref="Source{T}"/> native state.</param>
        /// <param name="id">The string unique identity to assign to the <see cref="IEntry{T}"/>.</param>
        /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
        TEntry ToEntry(TSource source, string id);

            /// <summary>
        /// Gets the <see cref="IEntry{T}"/> state with its <paramref name="id"/> from the <see cref="Source{T}"/> native state.
        /// </summary>
        /// <param name="source">The <see cref="Source{T}"/> native state.</param>
        /// <param name="id">The string unique identity to assign to the <see cref="IEntry{T}"/>.</param>
        /// <param name="metadata">The <see cref="Metadata"/> for this entry.</param>
        /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
        TEntry ToEntry(TSource source, string id, Metadata metadata);
    }

    public abstract class EntryAdapter<TSource, TEntry> : IEntryAdapter<TSource, TEntry> where TEntry : IEntry where TSource : Source
    {
        public abstract TSource FromEntry(TEntry entry);

        public abstract TEntry ToEntry(TSource source, Metadata metadata);

        public abstract TEntry ToEntry(TSource source, string id, Metadata metadata);

        public virtual TEntry ToEntry(TSource source) => ToEntry(source, Metadata.NullMetadata());

        public virtual TEntry ToEntry(TSource source, string id) => ToEntry(source, id, Metadata.NullMetadata());
    }
}