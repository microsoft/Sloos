// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Sloos.Pump
{
    public sealed class RegurgitatingXmlWriter : XmlWriter
    {
        private readonly StringBuilder stringBuilder;
        private readonly XmlWriter xmlWriter;

        public RegurgitatingXmlWriter()
        {
            this.stringBuilder = new StringBuilder();
            this.xmlWriter = XmlWriter.Create(this.stringBuilder);
        }

        public override WriteState WriteState => this.xmlWriter.WriteState;

        public override void Flush()
        {
            this.xmlWriter.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return this.xmlWriter.LookupPrefix(ns);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            this.xmlWriter.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            this.xmlWriter.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            this.xmlWriter.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            this.xmlWriter.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            this.xmlWriter.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubId, string sysId, string subset)
        {
            this.xmlWriter.WriteDocType(name, pubId, sysId, subset);
        }

        public override void WriteEndAttribute()
        {
            this.xmlWriter.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            this.xmlWriter.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            this.xmlWriter.WriteEndElement();
        }

        public override void WriteEntityRef(string name)
        {
            this.xmlWriter.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            this.xmlWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            this.xmlWriter.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(string data)
        {
            this.xmlWriter.WriteRaw(data);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            this.xmlWriter.WriteRaw(buffer, index, count);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            this.xmlWriter.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument(bool standalone)
        {
            this.xmlWriter.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            this.xmlWriter.WriteStartDocument();
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            this.xmlWriter.WriteStartElement(prefix, localName, ns);
        }

        public override void WriteString(string text)
        {
            this.xmlWriter.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            this.xmlWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            this.xmlWriter.WriteWhitespace(ws);
        }

        public XmlReader CreateReader()
        {
            return XmlReader.Create(
                new StringReader(this.stringBuilder.ToString()));
        }

        public static XmlReader Regurgitate(Action<XmlWriter> action)
        {
            var regurge = new RegurgitatingXmlWriter();
            action(regurge);

            regurge.Flush();
            regurge.Close();

            return regurge.CreateReader();
        }
    }
}
