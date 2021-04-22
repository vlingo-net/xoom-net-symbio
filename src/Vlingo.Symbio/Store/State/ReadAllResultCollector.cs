// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common;

namespace Vlingo.Symbio.Store.State
{
    /// <summary>
    ///     Collects results of multiple reads using <see cref="M:IStateStoreReader.ReadAll" />StateStoreReader#readAll()}
    /// </summary>
    public class ReadAllResultCollector : IReadResultInterest
    {
        private readonly List<TypedStateBundle> _readBundles;
        private readonly AtomicReference<IOutcome<StorageException, Result>> _readOutcome;
        private readonly AtomicBoolean _success;

        public ReadAllResultCollector()
        {
            _readBundles = new List<TypedStateBundle>();
            _success = new AtomicBoolean(true);
            _readOutcome =
                new AtomicReference<IOutcome<StorageException, Result>>(
                    Success.Of<StorageException, Result>(Result.Success));
        }

        /// <inheritdoc />
        public void ReadResultedIn<TState>(IOutcome<StorageException, Result> outcome, string? id, TState state, int stateVersion, Metadata? metadata, object? @object)
        {
            outcome.AndThen(result =>
                {
                    _readBundles.Add(new TypedStateBundle(id, state, stateVersion, metadata));
                    return result;
                })
                .Otherwise(cause =>
                {
                    _readOutcome.Set(outcome);
                    _success.Set(false);
                    return cause.Result;
                });
        }

        /// <inheritdoc />
        public void ReadResultedIn<TState>(IOutcome<StorageException, Result> outcome, IEnumerable<TypedStateBundle> bundles, object? @object)
        {
        }

        /// <summary>
        ///     Prepares results collectors.
        /// </summary>
        public void Prepare()
        {
            _readBundles.Clear();
            _success.Set(true);
            _readOutcome.Set(Success.Of<StorageException, Result>(Result.Success));
        }
        
        public IOutcome<StorageException, Result>? ReadResultOutcome(int expectedReads)
        {
            if (IsFailure)
            {
                if (!_readBundles.Any() && _readBundles.Count < expectedReads)
                {
                    return Failure.Of<StorageException, Result>(new StorageException(Result.NotAllFound, "Not all states were found."));
                }
            }

            return _readOutcome.Get();
        }
        
        /// <summary>
        /// Gets the result collector
        /// </summary>
        public IEnumerable<TypedStateBundle> ReadBundles => _readBundles;
        
        public bool IsFailure => !IsSuccess;
        
        public bool IsSuccess => _success.Get();
    }
}