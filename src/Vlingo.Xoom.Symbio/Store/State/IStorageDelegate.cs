// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Symbio.Store.State
{
    /// <summary>
    /// Defines the interface through which basic abstract storage implementations
    /// delegate to the technical implementations. See any of the existing concrete
    /// implementations for details.
    /// </summary>
    public interface IStorageDelegate
    {
        IStorageDelegate Copy();

        void Close();
        
        bool IsClosed { get; }
        
        Advice? EntryReaderAdvice { get; }

        void Initialize();
        
        string? OriginalId { get; }

        TState StateFrom<TState, TResult>(TResult result, string id);
        
        TState StateFrom<TState, TResult>(TResult result, string id, int columnOffset);
    }
}