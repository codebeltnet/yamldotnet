using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.YamlDotNet.Assets;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Extensions.IO;
using Xunit;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Codebelt.Extensions.YamlDotNet
{
    public class YamlConverterFactoryTest : Test
    {
        public YamlConverterFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_Generic_WithPredicateAndWriter_ShouldSerializeBook()
        {
            var converter = YamlConverterFactory.Create<Book>(
                t => typeof(Book).IsAssignableFrom(t),
                (emitter, book, _) =>
                {
                    emitter.WriteStartObject();
                    emitter.WriteString("title", book.Title);
                    emitter.WriteString("summary", book.Summary);
                    emitter.WriteEndObject();
                });

            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var sut = new Book { Title = "TestTitle", Summary = "TestSummary" };

            var yaml = formatter.Serialize(sut).ToEncodedString();
            TestOutput.WriteLine(yaml);

            Assert.Contains("TestTitle", yaml);
            Assert.Contains("TestSummary", yaml);
        }

        [Fact]
        public void Create_Generic_WithPredicateAndReader_ShouldDeserializeBook()
        {
            var converter = YamlConverterFactory.Create<Book>(
                t => typeof(Book).IsAssignableFrom(t),
                reader: (parser, _, _) =>
                {
                    parser.Consume<MappingStart>();
                    string title = null, summary = null;
                    while (!parser.TryConsume<MappingEnd>(out _))
                    {
                        var key = parser.Consume<Scalar>().Value;
                        var value = parser.Consume<Scalar>().Value;
                        if (key.Equals("title", StringComparison.OrdinalIgnoreCase)) title = value;
                        else if (key.Equals("summary", StringComparison.OrdinalIgnoreCase)) summary = value;
                    }
                    return new Book { Title = title, Summary = summary };
                });

            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var yaml = "title: TestTitle\nsummary: TestSummary\n";

            var book = (Book)formatter.Deserialize(yaml.ToStream(), typeof(Book));

            Assert.Equal("TestTitle", book.Title);
            Assert.Equal("TestSummary", book.Summary);
        }

        [Fact]
        public void Create_NonGeneric_WithTypeAndWriterAndReader_ShouldSerializeAndDeserializeBook()
        {
            var converter = YamlConverterFactory.Create(
                typeof(Book),
                (emitter, obj, _) =>
                {
                    var book = (Book)obj;
                    emitter.WriteStartObject();
                    emitter.WriteString("title", book.Title);
                    emitter.WriteString("summary", book.Summary);
                    emitter.WriteEndObject();
                },
                (parser, _, _) =>
                {
                    parser.Consume<MappingStart>();
                    string title = null, summary = null;
                    while (!parser.TryConsume<MappingEnd>(out _))
                    {
                        var key = parser.Consume<Scalar>().Value;
                        var value = parser.Consume<Scalar>().Value;
                        if (key.Equals("title", StringComparison.OrdinalIgnoreCase)) title = value;
                        else if (key.Equals("summary", StringComparison.OrdinalIgnoreCase)) summary = value;
                    }
                    return new Book { Title = title, Summary = summary };
                });

            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var sut = new Book { Title = "TestTitle", Summary = "TestSummary" };

            var yaml = formatter.Serialize(sut).ToEncodedString();
            TestOutput.WriteLine(yaml);
            Assert.Contains("TestTitle", yaml);

            var book = (Book)formatter.Deserialize(yaml.ToStream(), typeof(Book));
            Assert.Equal("TestTitle", book.Title);
            Assert.Equal("TestSummary", book.Summary);
        }

        [Fact]
        public void Create_NonGeneric_WithPredicateAndWriter_ShouldSerializeBook()
        {
            var converter = YamlConverterFactory.Create(
                t => typeof(Book).IsAssignableFrom(t),
                (emitter, obj, _) =>
                {
                    var book = (Book)obj;
                    emitter.WriteStartObject();
                    emitter.WriteString("title", book.Title);
                    emitter.WriteEndObject();
                });

            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var sut = new Book { Title = "PredicateBook", Summary = "Summary" };

            var yaml = formatter.Serialize(sut).ToEncodedString();
            TestOutput.WriteLine(yaml);

            Assert.Contains("PredicateBook", yaml);
        }

        [Fact]
        public void Create_Generic_WithNullWriter_ShouldThrowNotImplementedException()
        {
            var converter = YamlConverterFactory.Create<Book>();
            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var sut = new Book { Title = "Test", Summary = "Summary" };

            var ex = Assert.ThrowsAny<Exception>(() => formatter.Serialize(sut));
            Assert.IsType<NotImplementedException>(ex.GetBaseException());
        }

        [Fact]
        public void Create_Generic_WithNullReader_ShouldThrowNotImplementedException()
        {
            var converter = YamlConverterFactory.Create<Book>(t => typeof(Book).IsAssignableFrom(t));
            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var yaml = "title: Test\nsummary: Summary\n";

            var ex = Assert.ThrowsAny<Exception>(() => formatter.Deserialize(yaml.ToStream(), typeof(Book)));
            Assert.IsType<NotImplementedException>(ex.GetBaseException());
        }

        [Fact]
        public void Create_NonGeneric_WithNullWriter_ShouldThrowNotImplementedException()
        {
            var converter = YamlConverterFactory.Create(typeof(Book));
            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var sut = new Book { Title = "Test", Summary = "Summary" };

            var ex = Assert.ThrowsAny<Exception>(() => formatter.Serialize(sut));
            Assert.IsType<NotImplementedException>(ex.GetBaseException());
        }

        [Fact]
        public void Create_NonGeneric_WithNullReader_ShouldThrowNotImplementedException()
        {
            var converter = YamlConverterFactory.Create(t => typeof(Book).IsAssignableFrom(t));
            var formatter = new YamlFormatter(o => o.Settings.Converters.Add(converter));
            var yaml = "title: Test\nsummary: Summary\n";

            var ex = Assert.ThrowsAny<Exception>(() => formatter.Deserialize(yaml.ToStream(), typeof(Book)));
            Assert.IsType<NotImplementedException>(ex.GetBaseException());
        }
    }
}
