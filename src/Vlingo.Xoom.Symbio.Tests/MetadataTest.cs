// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Xunit;

namespace Vlingo.Xoom.Symbio.Tests;

public class MetadataTest
{
    [Fact]
    public void TestMetadataEmpty()
    {
        var metadata = new Metadata();
        Assert.False(metadata.HasValue);
        Assert.False(metadata.HasOperation);
    }

    [Fact]
    public void TestMetadataObject()
    {
        var @object = new object();
        var metadata = Metadata.WithObject(@object);
        Assert.True(metadata.HasObject);
        Assert.Equal(@object, metadata.Object);
        Assert.False(metadata.HasValue);
        Assert.False(metadata.HasOperation);
    }

    [Fact]
    public void TestMetadataValue()
    {
        var metadata = Metadata.WithValue("value");
        Assert.True(metadata.HasValue);
        Assert.Equal("value", metadata.Value);
        Assert.False(metadata.HasOperation);
    }

    [Fact]
    public void TestMetadataOperation()
    {
        var metadata = Metadata.WithOperation("op");
        Assert.False(metadata.HasValue);
        Assert.True(metadata.HasOperation);
        Assert.Equal("op", metadata.Operation);
    }

    [Fact]
    public void TestMetadataValueOperation()
    {
        var metadata = Metadata.With("value", "op");
        Assert.True(metadata.HasValue);
        Assert.Equal("value", metadata.Value);
        Assert.True(metadata.HasOperation);
        Assert.Equal("op", metadata.Operation);
    }
}