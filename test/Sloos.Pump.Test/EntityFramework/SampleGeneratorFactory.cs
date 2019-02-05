// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Xml;
using Sloos.Pump.EntityFramework;

namespace Sloos.Pump.Test.EntityFramework
{
    public class SampleGeneratorFactory : IGeneratorFactory
    {
        public SampleGeneratorFactory(Stream csdlStream, Stream ssdlStream, Stream msdl)
        {
            var csdlReader = XmlReader.Create(csdlStream);
            this.ConceptualItemCollection = new EdmItemCollection(new[] { csdlReader });

            var ssdlReader = XmlReader.Create(ssdlStream);
            this.StoreItemCollection = new StoreItemCollection(new[] { ssdlReader });

            this.EdmMapping = new EdmMapping(
                this.ConceptualItemCollection,
                this.StoreItemCollection,
                msdl);
        }

        public StoreItemCollection StoreItemCollection { get; }

        public EdmItemCollection ConceptualItemCollection { get; }

        public EdmMapping EdmMapping { get; }
    }
}
