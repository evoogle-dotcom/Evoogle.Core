// Copyright (c) 2024-2025 Evoogle.com
// SPDX-License-Identifier: MIT
//
// This file is licensed under the MIT License.
// See the LICENSE file in the project root for more information.
using System.Text.Json;

using Evoogle.XUnit;

using FluentAssertions;

using Microsoft.Extensions.Logging;

namespace Evoogle.Json;

public class JsonConverterBaseReadTests(ITestOutputHelper output) : XUnitTests(output)
{
    private sealed record ProbeResult(int[] Items, int NullCount);
    private sealed record ObjectProbeResult
    (
        string? Known,
        int NullDispatchCount,
        int IgnoredNullCount,
        string[] UnknownNames
    );

    private sealed class ObjectProbeConverter() : JsonConverterBase<ObjectProbeResult>(null)
    {
        private sealed class ReadContext(ILogger logger, JsonSerializerOptions options)
            : IReadContext
        {
            public ILogger Logger { get; } = logger;
            public JsonSerializerOptions Options { get; } = options;
            public string? Known { get; set; }
            public int NullDispatchCount { get; set; }
            public int IgnoredNullCount { get; set; }
            public List<string> UnknownNames { get; } = [];

            public void OnReadOfUnknownProperty(string name) => this.UnknownNames.Add(name);
        }

        private static readonly JsonReaderHandlerTable<ReadContext> _handlers = new()
        {
            { "Known", ReadKnown },
            { "Nullable", ReadNullable, true },
            { "IgnoredNull", ReadIgnoredNull }
        };

        protected override IReadContext CreateReadContext
        (
            ILogger logger,
            JsonSerializerOptions options
        )
            => new ReadContext(logger, options);

        protected override IWriteContext CreateWriteContext
        (
            ILogger logger,
            JsonSerializerOptions options
        )
            => throw new NotSupportedException();

        protected override ObjectProbeResult CreateValue(IReadContext context)
        {
            var readContext = (ReadContext)context;
            return new ObjectProbeResult
            (
                readContext.Known,
                readContext.NullDispatchCount,
                readContext.IgnoredNullCount,
                [.. readContext.UnknownNames]
            );
        }

        protected override void ReadCore(ref Utf8JsonReader reader, IReadContext context)
            => ReadJsonObject(ref reader, (ReadContext)context, _handlers);

        protected override void WriteCore
        (
            Utf8JsonWriter writer,
            ObjectProbeResult value,
            IWriteContext context
        )
            => throw new NotSupportedException();

        private static void ReadKnown(ref Utf8JsonReader reader, ReadContext context)
            => context.Known = reader.GetString();

        private static void ReadNullable(ref Utf8JsonReader reader, ReadContext context)
            => context.NullDispatchCount++;

        private static void ReadIgnoredNull(ref Utf8JsonReader reader, ReadContext context)
            => context.IgnoredNullCount++;
    }

    private sealed class ProbeConverter(bool useLegacyOverload)
        : JsonConverterBase<ProbeResult>(null)
    {
        private sealed class ReadContext(ILogger logger, JsonSerializerOptions options)
            : IReadContext
        {
            public ILogger Logger { get; } = logger;
            public JsonSerializerOptions Options { get; } = options;
            public List<int> Items { get; } = [];
            public int NullCount { get; private set; }

            public void OnReadOfNullArrayItem(int index) => this.NullCount++;
        }

        protected override IReadContext CreateReadContext
        (
            ILogger logger,
            JsonSerializerOptions options
        )
            => new ReadContext(logger, options);

        protected override IWriteContext CreateWriteContext
        (
            ILogger logger,
            JsonSerializerOptions options
        )
            => throw new NotSupportedException();

        protected override ProbeResult CreateValue(IReadContext context)
        {
            var readContext = (ReadContext)context;
            return new ProbeResult([.. readContext.Items], readContext.NullCount);
        }

        protected override void ReadCore(ref Utf8JsonReader reader, IReadContext context)
        {
            var readContext = (ReadContext)context;
            if (useLegacyOverload)
            {
                ReadJsonArray(ref reader, readContext, static _ => ReadItem);
            }
            else
            {
                ReadJsonArray(ref reader, readContext, ReadItem);
            }
        }

        protected override void WriteCore
        (
            Utf8JsonWriter writer,
            ProbeResult value,
            IWriteContext context
        )
            => throw new NotSupportedException();

        private static void ReadItem(ref Utf8JsonReader reader, ReadContext context)
            => context.Items.Add(reader.GetInt32());
    }

    private sealed class ArrayReadTest : XUnitTest
    {
        public required string SourceJson { get; init; }
        public bool UseLegacyOverload { get; init; }
        public int[]? ExpectedItems { get; init; }
        public int ExpectedNullCount { get; init; }
        public bool ExpectsJsonException { get; init; }

        private ProbeResult? ActualResult { get; set; }
        private Exception? ActualException { get; set; }

        protected override void Arrange()
        {
        }

        protected override void Act()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ProbeConverter(this.UseLegacyOverload));

            try
            {
                this.ActualResult = JsonSerializer.Deserialize<ProbeResult>
                (
                    this.SourceJson,
                    options
                );
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }
        }

        protected override void Assert()
        {
            if (this.ExpectsJsonException)
            {
                this.ActualException.Should().BeOfType<JsonException>();
                return;
            }

            this.ActualException.Should().BeNull();
            this.ActualResult.Should().NotBeNull();
            this.ActualResult!.Items.Should().Equal(this.ExpectedItems);
            this.ActualResult.NullCount.Should().Be(this.ExpectedNullCount);
        }
    }

    private sealed class ObjectReadTest : XUnitTest
    {
        public required string SourceJson { get; init; }
        public string? ExpectedKnown { get; init; }
        public int ExpectedNullDispatchCount { get; init; }
        public string[] ExpectedUnknownNames { get; init; } = [];
        public bool ExpectsJsonException { get; init; }

        private ObjectProbeResult? ActualResult { get; set; }
        private Exception? ActualException { get; set; }

        protected override void Arrange()
        {
        }

        protected override void Act()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new ObjectProbeConverter());

            try
            {
                this.ActualResult = JsonSerializer.Deserialize<ObjectProbeResult>
                (
                    this.SourceJson,
                    options
                );
            }
            catch (Exception exception)
            {
                this.ActualException = exception;
            }
        }

        protected override void Assert()
        {
            if (this.ExpectsJsonException)
            {
                this.ActualException.Should().BeOfType<JsonException>();
                return;
            }

            this.ActualException.Should().BeNull();
            this.ActualResult.Should().NotBeNull();
            this.ActualResult!.Known.Should().Be(this.ExpectedKnown);
            this.ActualResult.NullDispatchCount.Should().Be(this.ExpectedNullDispatchCount);
            this.ActualResult.IgnoredNullCount.Should().Be(0);
            this.ActualResult.UnknownNames.Should().Equal(this.ExpectedUnknownNames);
        }
    }

    public static TheoryDataRow<IXUnitTest>[] ReadJsonArrayTheoryData =>
    [
        new ArrayReadTest
        {
            Name = "Direct handler skips null items",
            SourceJson = "[1,null,2]",
            ExpectedItems = [1, 2],
            ExpectedNullCount = 1
        },
        new ArrayReadTest
        {
            Name = "Legacy accessor skips null items",
            SourceJson = "[1,null,2]",
            UseLegacyOverload = true,
            ExpectedItems = [1, 2],
            ExpectedNullCount = 1
        },
        new ArrayReadTest
        {
            Name = "Direct handler reads an empty array",
            SourceJson = "[]",
            ExpectedItems = []
        },
        new ArrayReadTest
        {
            Name = "Direct handler rejects an object",
            SourceJson = "{}",
            ExpectsJsonException = true
        },
        new ArrayReadTest
        {
            Name = "Legacy accessor rejects an object",
            SourceJson = "{}",
            UseLegacyOverload = true,
            ExpectsJsonException = true
        }
    ];

    public static TheoryDataRow<IXUnitTest>[] ReadJsonObjectTheoryData =>
    [
        new ObjectReadTest
        {
            Name = "Known null dispatches while other null is skipped",
            SourceJson = """{"Known":"ok","Nullable":null,"IgnoredNull":null}""",
            ExpectedKnown = "ok",
            ExpectedNullDispatchCount = 1
        },
        new ObjectReadTest
        {
            Name = "Escaped known name matches cached UTF-8 name",
            SourceJson = """{"Kno\u0077n":"ok"}""",
            ExpectedKnown = "ok"
        },
        new ObjectReadTest
        {
            Name = "Unknown nested value is skipped",
            SourceJson = """{"Unknown":{"Nested":[1,2]},"Known":"ok"}""",
            ExpectedKnown = "ok",
            ExpectedUnknownNames = ["Unknown"]
        },
        new ObjectReadTest
        {
            Name = "Duplicate known property keeps last value",
            SourceJson = """{"Known":"first","Known":"last"}""",
            ExpectedKnown = "last"
        },
        new ObjectReadTest
        {
            Name = "Property dispatch remains case-sensitive",
            SourceJson = """{"known":"value"}""",
            ExpectedUnknownNames = ["known"]
        },
        new ObjectReadTest
        {
            Name = "Object helper rejects an array",
            SourceJson = "[]",
            ExpectsJsonException = true
        }
    ];

    [Theory]
    [MemberData(nameof(ReadJsonArrayTheoryData))]
    public void ReadJsonArray(IXUnitTest test) => test.Execute(this);

    [Theory]
    [MemberData(nameof(ReadJsonObjectTheoryData))]
    public void ReadJsonObject(IXUnitTest test) => test.Execute(this);
}
