// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio.Store
{
    public class StorageException : Exception
    {
        public Result Result { get; }

        public StorageException(Result result, string message, Exception exception) : base(message, exception)
        {
            Result = result;
        }
        
        public StorageException(Result result, string message) : base(message)
        {
            Result = result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(GetType()))
            {
                return false;
            }
            return Result == ((StorageException) obj).Result;
        }

        public override int GetHashCode() => 31 * Result.GetHashCode();
    }
}