// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Xoom.Symbio;

/// <summary>
/// Adapts the native <see cref="Source{T}"/> state to the raw <see cref="IEntry{T}"/>,
/// and the raw <see cref="IEntry{T}"/> to the native <see cref="Source{T}"/>.
/// <para>
/// Note that the <c>id</c> provided herein is the identity assigned by the storage mechanism.
/// This may be a sequence number or an alphanumeric value. The important thing to note is
/// that this is provided by storage but may subsequently be used to represent time or order.
/// </para>
/// </summary>
public interface IEntryAdapter
{
    ISource AnyTypeFromEntry(IEntry entry);
        
    /// <summary>
    /// Gets the <see cref="Source{T}"/> native state from the <see cref="IEntry{T}"/> state.
    /// </summary>
    /// <param name="entry">The <see cref="IEntry{T}"/> to adapt from.</param>
    /// <returns>Adapted <see cref="Source{T}"/>.</returns>
    ISource FromEntry(IEntry entry);

    /// <summary>
    /// Gets the <see cref="IEntry{T}"/> state from the <see cref="Source{T}"/> native state.
    /// </summary>
    /// <param name="source">The <see cref="Source{T}"/> native state.</param>
    /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
    IEntry ToEntry(ISource source);
        
    /// <summary>
    /// Gets the <see cref="IEntry{T}"/> state from the <see cref="Source{T}"/> native state.
    /// </summary>
    /// <param name="source">The <see cref="Source{T}"/> native state.</param>
    /// <param name="metadata">The <see cref="Metadata"/> for this entry.</param>
    /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
    IEntry ToEntry(ISource source, Metadata metadata);

    /// <summary>
    /// Gets the <see cref="IEntry{T}"/> state from the <see cref="Source{T}"/> native state.
    /// </summary>
    /// <param name="source">The <see cref="Source{T}"/> native state.</param>
    /// <param name="version">The int state version with which source is associated</param>
    /// <param name="metadata">The <see cref="Metadata"/> for this entry.</param>
    /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
    IEntry ToEntry(ISource source, int version, Metadata metadata);

    /// <summary>
    /// Gets the <see cref="IEntry{T}"/> state with its <paramref name="id"/> from the <see cref="Source{T}"/> native state.
    /// </summary>
    /// <param name="source">The <see cref="Source{T}"/> native state.</param>
    /// <param name="id">The string unique identity to assign to the <see cref="IEntry{T}"/>.</param>
    /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
    IEntry ToEntry(ISource source, string id);

    /// <summary>
    /// Gets the <see cref="IEntry{T}"/> state with its <paramref name="id"/> from the <see cref="Source{T}"/> native state.
    /// </summary>
    /// <param name="source">The <see cref="Source{T}"/> native state.</param>
    /// <param name="id">The string unique identity to assign to the <see cref="IEntry{T}"/>.</param>
    /// <param name="metadata">The <see cref="Metadata"/> for this entry.</param>
    /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
    IEntry ToEntry(ISource source, string id, Metadata metadata);

    /// <summary>
    /// Gets the <see cref="IEntry{T}"/> state with its <paramref name="id"/> from the <see cref="Source{T}"/> native state.
    /// </summary>
    /// <param name="source">The <see cref="Source{T}"/> native state.</param>
    /// <param name="version">The int state version with which source is associated</param>
    /// <param name="id">The string unique identity to assign to the <see cref="IEntry{T}"/>.</param>
    /// <param name="metadata">The <see cref="Metadata"/> for this entry.</param>
    /// <returns>Adapted <see cref="IEntry{T}"/>.</returns>
    IEntry ToEntry(ISource source, int version, string id, Metadata metadata);
        
    Type SourceType { get; }
}

public abstract class EntryAdapter : IEntryAdapter
{
    public ISource AnyTypeFromEntry(IEntry entry)
    {
        var source = FromEntry(entry);
        return source;
    }
        
    public abstract ISource FromEntry(IEntry entry);

    public abstract IEntry ToEntry(ISource source, Metadata metadata);
        
    public abstract IEntry ToEntry(ISource source, int version, Metadata metadata);
        
    public abstract IEntry ToEntry(ISource source, int version, string id, Metadata metadata);
        
    public abstract Type SourceType { get; }

    public virtual IEntry ToEntry(ISource source, string id, Metadata metadata) =>
        ToEntry(source, Entry<ISource>.DefaultVersion, id, metadata);
        
    public virtual IEntry ToEntry(ISource source) => ToEntry(source, Metadata.NullMetadata());

    public virtual IEntry ToEntry(ISource source, string id) => ToEntry(source, id, Metadata.NullMetadata());
}