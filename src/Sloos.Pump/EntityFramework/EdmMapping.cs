// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Sloos.Pump.EntityFramework
{
    public class EdmMapping
    {
        private const string XmlNamespace = "http://schemas.microsoft.com/ado/2009/11/mapping/cs";

        public EdmMapping(EdmItemCollection conceptual, StoreItemCollection store, Stream storageMappingStream)
            : this(conceptual, store, XmlReader.Create(storageMappingStream))
        {
        }

        public EdmMapping(EdmItemCollection conceptual, StoreItemCollection store, XmlReader mappingReader)
        {
            // Pull mapping xml out
            var mappingDoc = new XmlDocument();
            mappingDoc.Load(mappingReader);

            var entitySets = conceptual.GetItems<EntityContainer>().Single().BaseEntitySets.OfType<System.Data.Entity.Core.Metadata.Edm.EntitySet>();
            var associationSets = conceptual.GetItems<EntityContainer>().Single().BaseEntitySets.OfType<System.Data.Entity.Core.Metadata.Edm.AssociationSet>();
            var tableSets = store.GetItems<EntityContainer>().Single().BaseEntitySets.OfType<System.Data.Entity.Core.Metadata.Edm.EntitySet>().ToArray();

            this.EntityMappings = BuildEntityMappings(mappingDoc, entitySets, tableSets);
            this.ManyToManyMappings = BuildManyToManyMappings(mappingDoc, associationSets, tableSets);
        }

        public Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>> EntityMappings { get; set; }

        public Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> ManyToManyMappings { get; set; }

        private static Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>> BuildManyToManyMappings(XmlDocument mappingDoc, IEnumerable<AssociationSet> associationSets, EntitySet[] tableSets)
        {
            // Build mapping for each association
            var mappings = new Dictionary<AssociationType, Tuple<EntitySet, Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>>>();
            var namespaceManager = new XmlNamespaceManager(mappingDoc.NameTable);
            namespaceManager.AddNamespace("ef", EdmMapping.XmlNamespace);

            foreach (var associationSet in associationSets.Where(a => !a.ElementType.AssociationEndMembers.Where(e => e.RelationshipMultiplicity != RelationshipMultiplicity.Many).Any()))
            {
                var setMapping = mappingDoc.SelectSingleNode($"//ef:AssociationSetMapping[@Name=\"{associationSet.Name}\"]", namespaceManager);
                var tableName = setMapping.Attributes["StoreEntitySet"].Value;
                var tableSet = tableSets.Single(s => s.Name == tableName);

                var endMappings = new Dictionary<RelationshipEndMember, Dictionary<EdmMember, string>>();
                foreach (var end in associationSet.AssociationSetEnds)
                {
                    var propertyToColumnMappings = new Dictionary<EdmMember, string>();
                    var endMapping = setMapping.SelectSingleNode($"./ef:EndProperty[@Name=\"{end.Name}\"]", namespaceManager);
                    foreach (XmlNode fk in endMapping.ChildNodes)
                    {
                        var propertyName = fk.Attributes["Name"].Value;
                        var property = end.EntitySet.ElementType.Properties[propertyName];
                        var columnName = fk.Attributes["ColumnName"].Value;
                        propertyToColumnMappings.Add(property, columnName);
                    }

                    endMappings.Add(end.CorrespondingAssociationEndMember, propertyToColumnMappings);
                }

                mappings.Add(associationSet.ElementType, Tuple.Create(tableSet, endMappings));
            }

            return mappings;
        }

        private static Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>> BuildEntityMappings(XmlDocument mappingDoc, IEnumerable<EntitySet> entitySets, IEnumerable<EntitySet> tableSets)
        {
            // Build mapping for each type
            var mappings = new Dictionary<EntityType, Tuple<EntitySet, Dictionary<EdmProperty, EdmProperty>>>();
            var namespaceManager = new XmlNamespaceManager(mappingDoc.NameTable);
            namespaceManager.AddNamespace("ef", EdmMapping.XmlNamespace);
            foreach (var entitySet in entitySets)
            {
                var setMapping = mappingDoc.SelectSingleNode(
                    $"//ef:EntitySetMapping[@Name=\"{entitySet.Name}\"]/ef:EntityTypeMapping/ef:MappingFragment", 
                    namespaceManager);

                var tableName = setMapping.Attributes["StoreEntitySet"].Value;
                var tableSet = tableSets.Single(s => s.Name == tableName);

                var propertyMappings = new Dictionary<EdmProperty, EdmProperty>();
                foreach (var prop in entitySet.ElementType.Properties)
                {
                    var propMapping = setMapping.SelectSingleNode($"./ef:ScalarProperty[@Name=\"{prop.Name}\"]", namespaceManager);
                    var columnName = propMapping.Attributes["ColumnName"].Value;
                    var columnProp = tableSet.ElementType.Properties[columnName];

                    propertyMappings.Add(prop, columnProp);
                }

                mappings.Add(entitySet.ElementType, Tuple.Create(tableSet, propertyMappings));
            }

            return mappings;
        }
    }
}
