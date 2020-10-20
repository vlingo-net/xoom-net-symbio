// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Symbio
{
    public class Metadata : IComparable<Metadata>
    {
        public static object EmptyObject => new DummyObject();
        
        public static Metadata NullMetadata() => new Metadata(EmptyObject, string.Empty, string.Empty);
        
        public static Metadata WithObject(object @object) => new Metadata(@object, string.Empty, string.Empty);
        
        public static Metadata WithOperation(string operation) => new Metadata(EmptyObject, string.Empty, operation);
        
        public static Metadata WithValue(string value) => new Metadata(EmptyObject, value, string.Empty);
        
        public static Metadata With(string value, string operation) => new Metadata(EmptyObject, value, operation);
        
        public static Metadata With(object @object, string value, string operation) => new Metadata(@object, value, operation);

        public static Metadata With<TOperation>(object @object, string value) => With<TOperation>(@object, value, true);

        public static Metadata With<TOperation>(object @object, string value, bool compact)
        {
            var operation = compact ? typeof(TOperation).Name : typeof(TOperation).FullName!;
            return new Metadata(@object, value, operation);
        }

        public Metadata(object @object, string value, string operation)
        {
            Object = @object ?? EmptyObject;
            Value = value ?? string.Empty;
            Operation = operation ?? string.Empty;
        }

        public Metadata(string value, string operation) : this(EmptyObject, value, operation)
        {
        }

        public Metadata() : this(EmptyObject, string.Empty, string.Empty)
        {
        }

        public object Object { get; }
        
        public string Operation { get; }
        
        public string Value { get; }

        public bool HasObject => Object != EmptyObject;

        public bool HasOperation => Operation != string.Empty;

        public bool HasValue => Value != string.Empty;

        public bool IsEmpty => !HasOperation && !HasValue;
        
        public int CompareTo(Metadata? other)
        {
            if (!Object.Equals(other?.Object))
            {
                return 1;
            }

            if (Value == other.Value)
            {
                return string.Compare(Operation, other.Operation, StringComparison.Ordinal);
            }

            return string.Compare(other.Value, Value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var otherMetadata = (Metadata) obj;

            return Value.Equals(otherMetadata.Value) &&
                   Operation.Equals(otherMetadata.Operation) &&
                   Object.Equals(otherMetadata.Object);
        }

        public override int GetHashCode() => 31 * Value.GetHashCode() + Operation.GetHashCode() + Object.GetHashCode();

        public override string ToString() => $"[Value={Value} Operation={Operation} Object={Object}]";

        private class DummyObject
        {
            public override bool Equals(object? obj) => ToString() == obj?.ToString();

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString() => "(empty)";
        }
    }
}