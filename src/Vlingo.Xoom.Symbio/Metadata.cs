// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Symbio;

public class Metadata : IComparable<Metadata>
{
    //[Obsolete]
    public static object EmptyObject => new DummyObject();
        
    public static Metadata NullMetadata() => new(new Dictionary<string, string>(), string.Empty, string.Empty);
    
    [Obsolete]
    public static Metadata WithObject(object @object) => new(@object, string.Empty, string.Empty);
    
    public static Metadata WithProperties(IReadOnlyDictionary<string, string> properties) => new(properties, string.Empty, string.Empty);
    
    public static Metadata WithOperation(string operation) => new(new Dictionary<string, string>(), string.Empty, operation);
    
    public static Metadata WithValue(string value) => new(new Dictionary<string, string>(), value, string.Empty);
    
    public static Metadata With(string value, string operation) => new(new Dictionary<string, string>(), value, operation);
    
    public static Metadata With(IReadOnlyDictionary<string, string> properties, string value, string operation) =>
        new(properties, value, operation);

    public static Metadata With<TOperation>(IReadOnlyDictionary<string, string> properties, string value) => 
        With<TOperation>(properties, value, true);

    public static Metadata With<TOperation>(IReadOnlyDictionary<string, string> properties, string value, bool compact)
    {
        var operation = compact ? typeof(TOperation).Name : typeof(TOperation).FullName;
        return new Metadata(properties, value, operation);
    }
    
    //[Obsolete]
    public static Metadata With(object @object, string value, string operation) => new(@object, value, operation);

    [Obsolete]
    public static Metadata With<TOperation>(object @object, string value) => With<TOperation>(@object, value, true);

    [Obsolete]
    public static Metadata With<TOperation>(object @object, string value, bool compact)
    {
        var operation = compact ? typeof(TOperation).Name : typeof(TOperation).FullName!;
        return new Metadata(@object, value, operation);
    }

    //[Obsolete]
    public Metadata(object? @object, string? value, string? operation)
    {
        Object = @object ?? EmptyObject;
        Value = value ?? string.Empty;
        Operation = operation ?? string.Empty;
        Properties = new Dictionary<string, string>(0);
    }
    
    public Metadata(IReadOnlyDictionary<string, string>? properties, string? value, string? operation)
    {
        //Object = EmptyObject;
        Properties = properties ?? new Dictionary<string, string>();
        Value = value ?? string.Empty;
        Operation = operation ?? string.Empty;
    }

    public Metadata(string value, string operation) : this(new Dictionary<string, string>(), value, operation)
    {
    }

    public Metadata() : this(new Dictionary<string, string>(), string.Empty, string.Empty)
    {
    }

    //[Obsolete("Object is deprecated and will be removed in future versions. Use the IReadonlyDictionary of Properties instead.")]
    public object? Object { get; }
    
    public IReadOnlyDictionary<string,string> Properties { get; }

    //[Obsolete]
    public Optional<object?> OptionalObject => HasObject ? Optional.Of(Object) : Optional.Empty<object?>();
        
    public string Operation { get; }
        
    public string Value { get; }

    //[Obsolete]
    public bool HasObject => Object != EmptyObject;
    
    public bool HasProperties => Properties.Any();

    public bool HasOperation => Operation != string.Empty;

    public bool HasValue => Value != string.Empty;

    public bool IsEmpty => !HasOperation && !HasValue;
        
    public int CompareTo(Metadata? other)
    {
        if (!Properties.OrderBy(kvp => kvp.Key)
                .SequenceEqual(
                    other == null ? new Dictionary<string, string>() : other.Properties.OrderBy(kvp => kvp.Key)))
        {
            return 1;
        }

        // if (!Object.Equals(other?.Object))
        // {
        //     return 1;
        // }

        if (Value == other?.Value)
        {
            return string.Compare(Operation, other.Operation, StringComparison.Ordinal);
        }

        return string.Compare(other?.Value, Value, StringComparison.Ordinal);
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
               Properties.Equals(otherMetadata.Properties);
        //Object.Equals(otherMetadata.Object);
    }

    public override int GetHashCode() => 31 * Value.GetHashCode() + Operation.GetHashCode() + /*Object.GetHashCode()*/ Properties.GetHashCode();

    public override string ToString() => $"[Value={Value} Operation={Operation} Properties={Properties}]";

    private class DummyObject
    {
        public override string ToString() => "(empty)";
    }
}