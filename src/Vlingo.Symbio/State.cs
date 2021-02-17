// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Common.Serialization;
using Vlingo.Symbio.Store;

namespace Vlingo.Symbio
{
    public interface IState
    {
        string Id { get; }
        
        int DataVersion { get; }
        
        Metadata Metadata { get; }
        
        string Type { get; }
        
        public Type Typed { get; }
        
        int TypeVersion { get; }
        
        string RawData { get; }
    }

    public abstract class State<T> : IComparable<State<T>>, IState
    {
        public static string NoOp = string.Empty;
        
        protected static readonly byte[] EmptyBytesData = new byte[0];
        protected static readonly T EmptyObjectData = default!;
        protected static readonly string EmptyTextData = string.Empty;
        
        public BinaryState? AsBinaryState() => this as BinaryState;

        public ObjectState<T> AsObjectState() => (ObjectState<T>) this;

        public TextState? AsTextState() => this as TextState;

        public string Id { get; }
        
        public T Data { get; }
        
        public string RawData { get; }
        
        public int DataVersion { get; }
        
        public Metadata Metadata { get; }
        
        public string Type { get; }
        
        public int TypeVersion { get; }

        public bool HasMetadata => !Metadata.IsEmpty;
        
        public virtual bool IsBinary => false;
        
        public virtual bool IsObject => false;
        
        public virtual bool IsText => false;

        public virtual bool IsEmpty => false;

        public virtual bool IsNull => false;

        public Type Typed => StoredTypes.ForName(Type);

        public int CompareTo(State<T>? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            var result = string.Compare(Id, other.Id, StringComparison.InvariantCulture);
            if (result == 0)
            {
                result = string.Compare(Type, other.Type, StringComparison.InvariantCulture);
            }
            
            if (result == 0)
            {
                result = TypeVersion.CompareTo(other.TypeVersion);
            }
            
            if (result == 0)
            {
                result = DataVersion.CompareTo(other.DataVersion);
            }

            if (result == 0)
            {
                result = Comparer<Metadata>.Default.Compare(Metadata, other.Metadata);
            }

            return result;
        }
        
        public override int GetHashCode() => 31 * Id.GetHashCode();


        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            return Id.Equals(((BaseEntry<T>) obj).Id);
        }
        
        public override string ToString()
        {
            return
                $"{GetType().Name}[id={Id} type={Type} typeVersion={TypeVersion} " +
                $"data={(IsText || IsObject ? Data?.ToString() : "(binary)")} " +
                $"dataVersion={DataVersion} metadata={Metadata}]";
        }
        
        protected State(string id, Type type, int typeVersion, T data, int dataVersion, Metadata metadata)
        {
            if (id == null) throw new ArgumentNullException(nameof(id), "State id must not be null.");
            if (type == null) throw new ArgumentNullException(nameof(type), "State type must not be null.");
            if (string.IsNullOrEmpty(type.AssemblyQualifiedName)) throw new ArgumentNullException(nameof(type.AssemblyQualifiedName), "State type.AssemblyQualifiedName must not be null.");
            if (typeVersion <= 0) throw new ArgumentOutOfRangeException(nameof(typeVersion), "State typeVersion must be greater than 0.");
            if (data == null) throw new ArgumentNullException(nameof(data), "State data must not be null.");
            if (dataVersion <= 0) throw new ArgumentOutOfRangeException(nameof(dataVersion), "State dataVersion must be greater than 0.");

            Id = id;
            Type = type.AssemblyQualifiedName;
            TypeVersion = typeVersion;
            Data = data;
            RawData = JsonSerialization.Serialized(data);
            DataVersion = dataVersion;
            Metadata = metadata ?? Metadata.NullMetadata();
        }
        
        protected State(string id, Type type, int typeVersion, T data, int dataVersion) 
            : this(id, type, typeVersion, data, dataVersion, Metadata.NullMetadata())
        {
        }
        
        private int CompareData(State<T> state1, State<T> state2)
        {
            if (state1.IsText && state2.IsText)
            {
                return string.Compare((string)(object)state1.Data!, (string)(object)state2.Data!, StringComparison.InvariantCulture);
            }
            
            if (state1.IsBinary && state2.IsBinary)
            {
                var data1 = (byte[])(object)state1.Data!;
                var data2 = (byte[])(object)state2.Data!;
                if (data1.Length == data2.Length)
                {
                    for (int idx = 0; idx < data1.Length; ++idx)
                    {
                        if (data1[idx] != data2[idx])
                        {
                            return 1;
                        }
                    }
                    return 0;
                }
                return 1;
            }
            return 1;
        }
    }
    
    public sealed class BinaryState : State<byte[]>
    {
        private static readonly BinaryState Null = new BinaryState();
        
        public BinaryState(string id, Type type, int typeVersion, byte[] data, int dataVersion, Metadata metadata) : base(id, type, typeVersion, data, dataVersion, metadata)
        {
        }
        
        public BinaryState(string id, Type type, int typeVersion, byte[] data, int dataVersion) : base(id, type, typeVersion, data, dataVersion)
        {
        }

        public BinaryState() : base(NoOp, typeof(object), 1, EmptyBytesData, 1, Metadata.NullMetadata())
        {
        }

        public override bool IsBinary => true;

        public override bool IsEmpty => Data.Length == 0;

        public override bool IsNull => Equals(this, Null);
    }
    
    public sealed class ObjectState<T> : State<T>
    {
        private static readonly ObjectState<T> Null = new ObjectState<T>();
        
        public ObjectState(string id, Type type, int typeVersion, T data, int dataVersion, Metadata metadata) : base(id, type, typeVersion, data, dataVersion, metadata)
        {
        }
        
        public ObjectState(string id, Type type, int typeVersion, T data, int dataVersion) : base(id, type, typeVersion, data, dataVersion)
        {
        }

        public ObjectState() : base(NoOp, typeof(T), 1, EmptyObjectData, 1, Metadata.NullMetadata())
        {
        }

        public override bool IsObject => true;

        public override bool IsEmpty => Data!.Equals(EmptyObjectData);
        
        public override bool IsNull => Equals(this, Null);
    }
    
    public sealed class TextState : State<string>
    {
        private static readonly TextState Null = new TextState();
        
        public TextState(string id, Type type, int typeVersion, string data, int dataVersion, Metadata metadata) : base(id, type, typeVersion, data, dataVersion, metadata)
        {
        }
        
        public TextState(string id, Type type, int typeVersion, string data, int dataVersion) : base(id, type, typeVersion, data, dataVersion)
        {
        }

        public TextState() : base(NoOp, typeof(string), 1, EmptyTextData, 1, Metadata.NullMetadata())
        {
        }

        public override bool IsText => true;

        public override bool IsEmpty => string.IsNullOrEmpty(Data);

        public override bool IsNull => Equals(this, Null);
    }
}