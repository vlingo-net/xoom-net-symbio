// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Symbio.Store.State
{
    public class NoOpStorageDelegate : IStorageDelegate
    {
        public IStorageDelegate Copy() => new NoOpStorageDelegate();

        public void Close()
        {
        }

        public bool IsClosed => false;

        public Advice? EntryReaderAdvice => null;

        public string? OriginalId => null;
        
        public TState StateFrom<TState, TResult>(TResult result, string id) => default!;
    }
}