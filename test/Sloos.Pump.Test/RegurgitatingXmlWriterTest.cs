// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Sloos.Pump.Test
{
    public class RegurgitatingXmlWriterTest
    {
        [Fact]
        public void RegurgitatingWriterShouldReadWrittenResults()
        {
            var testSubject = new RegurgitatingXmlWriter();
            testSubject.WriteStartDocument();
            testSubject.WriteStartElement("root");
            testSubject.WriteElementString("_1st", string.Empty, "one");
            testSubject.WriteElementString("_2nd", string.Empty, "two");
            testSubject.WriteEndElement();
            testSubject.WriteEndDocument();
            testSubject.Flush();
            testSubject.Close();

            var doc = XDocument.Load(
                testSubject.CreateReader());

            var nodes = doc.Document.Root.Nodes().Cast<XElement>().ToArray();
            nodes[0].Value.Should().Be("one");
            nodes[1].Value.Should().Be("two");
        }

        [Fact]
        public void RegurgitateShouldRoundTrip()
        {
            var xmlReader = RegurgitatingXmlWriter.Regurgitate(x =>
                {
                    x.WriteStartDocument();
                    x.WriteStartElement("root");
                    x.WriteElementString("_1st", string.Empty, "one");
                    x.WriteElementString("_2nd", string.Empty, "two");
                    x.WriteEndElement();
                    x.WriteEndDocument();
                });

            var doc = XDocument.Load(xmlReader);

            var nodes = doc.Document.Root.Nodes().Cast<XElement>().ToArray();
            nodes[0].Value.Should().Be("one");
            nodes[1].Value.Should().Be("two");
        }
    }
}
