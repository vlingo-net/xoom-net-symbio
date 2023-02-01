// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using IDispatcher = Vlingo.Xoom.Symbio.Store.Dispatch.IDispatcher;

namespace Vlingo.Xoom.Symbio.Store.Journal;

public class NoOpJournalActor<T> : Actor, IJournal<T>
{
    private const string WarningMessage =
        "\n===============================================================================================\n" +
        "                                                                                             \n" +
        " All journal operations are stopped. Please check your DB settings and user credentials.     \n" +
        "                                                                                             \n" +
        "===============================================================================================\n";

    public NoOpJournalActor() => Logger.Warn(WarningMessage);

    public void Append<TSource>(string streamName, int streamVersion, TSource source, IAppendResultInterest interest,
        object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void Append<TSource>(string streamName, int streamVersion, TSource source, Metadata metadata,
        IAppendResultInterest interest, object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, TSnapshotState snapshot,
        IAppendResultInterest interest, object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void AppendWith<TSource, TSnapshotState>(string streamName, int streamVersion, TSource source, Metadata metadata,
        TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, IAppendResultInterest interest,
        object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void AppendAll<TSource>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources, Metadata metadata,
        IAppendResultInterest interest, object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources,
        TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public void AppendAllWith<TSource, TSnapshotState>(string streamName, int fromStreamVersion, IEnumerable<ISource> sources,
        Metadata metadata, TSnapshotState snapshot, IAppendResultInterest interest, object @object) where TSource : ISource
    {
        Logger.Error(WarningMessage);
        ((ICompletes<T>)Completes()).Failed();
    }

    public ICompletes<IJournalReader?> JournalReader(string name)
    {
        Logger.Error(WarningMessage);
        return Common.Completes.WithFailure<IJournalReader?>();
    }

    public ICompletes<IStreamReader?> StreamReader(string name)
    {
        Logger.Error(WarningMessage);
        return Common.Completes.WithFailure<IStreamReader?>();
    }

    public IJournal<T> Using<TActor>(Stage stage, IDispatcher dispatcher, params object[] additional) where TActor : Actor
    {
        Logger.Error(WarningMessage);
        throw new InvalidOperationException(WarningMessage);
    }

    public IJournal<T> Using<TActor>(Stage stage, IEnumerable<IDispatcher> dispatchers, params object[] additional) where TActor : Actor
    {
        Logger.Error(WarningMessage);
        throw new InvalidOperationException(WarningMessage);
    }
}