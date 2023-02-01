// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Text;
using Xunit;

namespace Vlingo.Xoom.Symbio.Tests;

public class StateTest
{
    [Fact]
    public void TestEmptyBinaryState()
    {
        var emptyState = new BinaryState();
        Assert.True(emptyState.IsBinary);
        Assert.True(emptyState.IsEmpty);
    }
        
    [Fact]
    public void TestBasicBinaryState()
    {
        var state = "test-state";
        var bytes = Encoding.UTF8.GetBytes(state);
        var basicState = new BinaryState("123", typeof(string), 1, bytes, 1);
        Assert.True(basicState.IsBinary);
        Assert.False(basicState.IsText);
        Assert.False(basicState.IsEmpty);
        Assert.Equal("123", basicState.Id);
        Assert.Equal(typeof(string).AssemblyQualifiedName, basicState.Type);
        Assert.Equal(1, basicState.TypeVersion);
        Assert.Equal(state, Encoding.UTF8.GetString(basicState.Data));
        Assert.False(basicState.HasMetadata);
    }
        
    [Fact]
    public void TestBinaryStateWithMetadataOperation()
    {
        var state = "test-state";
        var bytes = Encoding.UTF8.GetBytes(state);
        var metadataOperationState = new BinaryState("123", typeof(string), 1, bytes, 1, Metadata.WithOperation("op"));
        Assert.True(metadataOperationState.IsBinary);
        Assert.False(metadataOperationState.IsText);
        Assert.False(metadataOperationState.IsEmpty);
        Assert.Equal("123", metadataOperationState.Id);
        Assert.Equal(typeof(string).AssemblyQualifiedName, metadataOperationState.Type);
        Assert.Equal(1, metadataOperationState.TypeVersion);
        Assert.Equal(state, Encoding.UTF8.GetString(metadataOperationState.Data));
        Assert.True(metadataOperationState.HasMetadata);
        Assert.True(metadataOperationState.Metadata.HasOperation);
        Assert.Equal("op", metadataOperationState.Metadata.Operation);
        Assert.False(metadataOperationState.Metadata.HasValue);
    }
        
    [Fact]
    public void TestBinaryStateWithMetadataValue()
    {
        var state = "test-state";
        var bytes = Encoding.UTF8.GetBytes(state);
        var metadataValueState = new BinaryState("123", typeof(string), 1, bytes, 1, Metadata.WithValue("value"));
        Assert.True(metadataValueState.IsBinary);
        Assert.False(metadataValueState.IsText);
        Assert.False(metadataValueState.IsEmpty);
        Assert.Equal("123", metadataValueState.Id);
        Assert.Equal(typeof(string).AssemblyQualifiedName, metadataValueState.Type);
        Assert.Equal(1, metadataValueState.TypeVersion);
        Assert.Equal(state, Encoding.UTF8.GetString(metadataValueState.Data));
        Assert.True(metadataValueState.HasMetadata);
        Assert.True(metadataValueState.Metadata.HasValue);
        Assert.Equal("value", metadataValueState.Metadata.Value);
        Assert.False(metadataValueState.Metadata.HasOperation);
    }
        
    [Fact]
    public void TestBinaryStateWithMetadata()
    {
        var state = "test-state";
        var bytes = Encoding.UTF8.GetBytes(state);
        var metadataState = new BinaryState("123", typeof(string), 1, bytes, 1, Metadata.With("value", "op"));
        Assert.True(metadataState.IsBinary);
        Assert.False(metadataState.IsText);
        Assert.False(metadataState.IsEmpty);
        Assert.Equal("123", metadataState.Id);
        Assert.Equal(typeof(string).AssemblyQualifiedName, metadataState.Type);
        Assert.Equal(1, metadataState.TypeVersion);
        Assert.Equal(state, Encoding.UTF8.GetString(metadataState.Data));
        Assert.True(metadataState.HasMetadata);
        Assert.True(metadataState.Metadata.HasValue);
        Assert.Equal("value", metadataState.Metadata.Value);
        Assert.True(metadataState.Metadata.HasOperation);
        Assert.Equal("op", metadataState.Metadata.Operation);
    }
        
    [Fact]
    public void TestEmptyTextState()
    {
        var emptyState = new TextState();
        Assert.False(emptyState.IsBinary);
        Assert.True(emptyState.IsText);
        Assert.True(emptyState.IsEmpty);
    }
}