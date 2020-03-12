# vlingo-net-symbio

[![Build status](https://ci.appveyor.com/api/projects/status/ug298v7ucwsvpj84?svg=true)](https://ci.appveyor.com/project/VlingoNetOwner/vlingo-net-symbio)
[![NuGet](https://img.shields.io/nuget/v/Vlingo.Symbio.svg)](https://www.nuget.org/packages/Vlingo.Symbio)
[![Gitter](https://badges.gitter.im/vlingo-platform-net/community.svg)](https://gitter.im/vlingo-platform-net/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

The reactive, scalable, and resilient CQRS storage and projection tool for services and applications built on the vlingo/platform.

### Name
The name "symbio" highlights the symbiotic relationship between domain models and persistence mechanisms.
Domain models must be persisted and individual parts must be reconstituted to memory when needed. Persistence
mechanisms crave data to store. Hence we can conclude that the two need and benefit from the other.

Interestingly too is that the name "symbio" ends with the letters, i and o, for input and output.
The `StateStorage`, introduced next, produces domain model output to disk, and input from disk back to
the domain model.

### Journal Storage
The `Journal` and related protocols support simple-to-use Event Sourcing, including `JournalReader` for
streaming across all entries in the journal, and `StreamReader` for reaching individual "sub-streams"
belonging to entities/aggregates in your application. There is a growing number of implementations:

TODO: List implementations

### Object Storage
The `ObjectStore` is a simple object-relational mapped storage mechanism that can be run against a number of
persistence engines. These are the available implementations:

TODO: List implementations

### State Storage
The `StateStore` is a simple CQRS Key-CLOB/BLOB storage mechanism that can be run against a number of persistence engines.
Use it for both Command/Write Models and Query/Read Models. These are the available storage implementations:

   - In-memory binary: `InMemoryBinaryStateStoreActor`
   - In-memory text: `InMemoryTextStateStoreActor`

TODO: Add implementations


We welcome you to add support for your favorite database!

License (See LICENSE file for full license)
-------------------------------------------
Copyright © 2012-2020 VLINGO LABS. All rights reserved.

This Source Code Form is subject to the terms of the
Mozilla Public License, v. 2.0. If a copy of the MPL
was not distributed with this file, You can obtain
one at https://mozilla.org/MPL/2.0/.