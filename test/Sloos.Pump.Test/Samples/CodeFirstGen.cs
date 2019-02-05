// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.IO;
using System.Text;
using Sloos.Common.Test;

namespace Sloos.Pump.Test.Samples
{
    public static class CodeFirstGen
    {
        public static class AllClrTypes
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Tables' EntityType='DefaultNamespace.Table' />");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Table'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='byte_type' Type='Byte' Nullable='false' />");
                sb.AppendLine("    <Property Name='bool_type' Type='Boolean' Nullable='false' />");
                sb.AppendLine("    <Property Name='short_type' Type='Int16' Nullable='false' />");
                sb.AppendLine("    <Property Name='int_type' Type='Int32' Nullable='false' />");
                sb.AppendLine("    <Property Name='long_type' Type='Int64' Nullable='false' />");
                sb.AppendLine("    <Property Name='float_type' Type='Single' Nullable='false' />");
                sb.AppendLine("    <Property Name='double_type' Type='Double' Nullable='false' />");
                sb.AppendLine("    <Property Name='decimal_type' Type='Decimal' Nullable='false' Precision='18' Scale='2' />");
                sb.AppendLine("    <Property Name='Guid_type' Type='Guid' Nullable='false' />");
                sb.AppendLine("    <Property Name='TimeSpan_type' Type='Time' Nullable='false' />");
                sb.AppendLine("    <Property Name='DateTime_type' Type='DateTime' Nullable='false' />");
                sb.AppendLine("    <Property Name='DateTimeOffset_type' Type='DateTimeOffset' Nullable='false' />");
                sb.AppendLine("    <Property Name='byte_nullable' Type='Byte' />");
                sb.AppendLine("    <Property Name='bool_nullable' Type='Boolean' />");
                sb.AppendLine("    <Property Name='short_nullable' Type='Int16' />");
                sb.AppendLine("    <Property Name='int_nullable' Type='Int32' />");
                sb.AppendLine("    <Property Name='long_nullable' Type='Int64' />");
                sb.AppendLine("    <Property Name='float_nullable' Type='Single' />");
                sb.AppendLine("    <Property Name='double_nullable' Type='Double' />");
                sb.AppendLine("    <Property Name='decimal_nullable' Type='Decimal' Precision='18' Scale='2' />");
                sb.AppendLine("    <Property Name='Guid_nullable' Type='Guid' />");
                sb.AppendLine("    <Property Name='TimeSpan_nullable' Type='Time' />");
                sb.AppendLine("    <Property Name='DateTime_nullable' Type='DateTime' />");
                sb.AppendLine("    <Property Name='DateTimeOffset_nullable' Type='DateTimeOffset' />");
                sb.AppendLine("    <Property Name='byte_array_type' Type='Binary' MaxLength='Max' FixedLength='false' />");
                sb.AppendLine("    <Property Name='string_type' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Tables'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Table'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tables'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("          <ScalarProperty Name='byte_type' ColumnName='byte_type' />");
                sb.AppendLine("          <ScalarProperty Name='bool_type' ColumnName='bool_type' />");
                sb.AppendLine("          <ScalarProperty Name='short_type' ColumnName='short_type' />");
                sb.AppendLine("          <ScalarProperty Name='int_type' ColumnName='int_type' />");
                sb.AppendLine("          <ScalarProperty Name='long_type' ColumnName='long_type' />");
                sb.AppendLine("          <ScalarProperty Name='float_type' ColumnName='float_type' />");
                sb.AppendLine("          <ScalarProperty Name='double_type' ColumnName='double_type' />");
                sb.AppendLine("          <ScalarProperty Name='decimal_type' ColumnName='decimal_type' />");
                sb.AppendLine("          <ScalarProperty Name='Guid_type' ColumnName='Guid_type' />");
                sb.AppendLine("          <ScalarProperty Name='TimeSpan_type' ColumnName='TimeSpan_type' />");
                sb.AppendLine("          <ScalarProperty Name='DateTime_type' ColumnName='DateTime_type' />");
                sb.AppendLine("          <ScalarProperty Name='DateTimeOffset_type' ColumnName='DateTimeOffset_type' />");
                sb.AppendLine("          <ScalarProperty Name='byte_nullable' ColumnName='byte_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='bool_nullable' ColumnName='bool_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='short_nullable' ColumnName='short_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='int_nullable' ColumnName='int_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='long_nullable' ColumnName='long_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='float_nullable' ColumnName='float_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='double_nullable' ColumnName='double_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='decimal_nullable' ColumnName='decimal_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='Guid_nullable' ColumnName='Guid_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='TimeSpan_nullable' ColumnName='TimeSpan_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='DateTime_nullable' ColumnName='DateTime_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='DateTimeOffset_nullable' ColumnName='DateTimeOffset_nullable' />");
                sb.AppendLine("          <ScalarProperty Name='byte_array_type' ColumnName='byte_array_type' />");
                sb.AppendLine("          <ScalarProperty Name='string_type' ColumnName='string_type' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Tables' EntityType='dbo.Tables' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Tables'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='byte_type' Type='tinyint' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='bool_type' Type='bit' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='short_type' Type='smallint' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='int_type' Type='int' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='long_type' Type='bigint' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='float_type' Type='real' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='double_type' Type='float' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='decimal_type' Type='decimal' Nullable='false' Scale='2' />");
                sb.AppendLine(@"    <Property Name='Guid_type' Type='uniqueidentifier' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='TimeSpan_type' Type='time' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='DateTime_type' Type='datetime' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='DateTimeOffset_type' Type='datetimeoffset' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='byte_nullable' Type='tinyint' />");
                sb.AppendLine(@"    <Property Name='bool_nullable' Type='bit' />");
                sb.AppendLine(@"    <Property Name='short_nullable' Type='smallint' />");
                sb.AppendLine(@"    <Property Name='int_nullable' Type='int' />");
                sb.AppendLine(@"    <Property Name='long_nullable' Type='bigint' />");
                sb.AppendLine(@"    <Property Name='float_nullable' Type='real' />");
                sb.AppendLine(@"    <Property Name='double_nullable' Type='float' />");
                sb.AppendLine(@"    <Property Name='decimal_nullable' Type='decimal' Scale='2' />");
                sb.AppendLine(@"    <Property Name='Guid_nullable' Type='uniqueidentifier' />");
                sb.AppendLine(@"    <Property Name='TimeSpan_nullable' Type='time' />");
                sb.AppendLine(@"    <Property Name='DateTime_nullable' Type='datetime' />");
                sb.AppendLine(@"    <Property Name='DateTimeOffset_nullable' Type='datetimeoffset' />");
                sb.AppendLine(@"    <Property Name='byte_array_type' Type='varbinary(max)' />");
                sb.AppendLine(@"    <Property Name='string_type' Type='nvarchar(max)' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class CompositeKey
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Tables' EntityType='DefaultNamespace.Table' />");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Table'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("      <PropertyRef Name='Name' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' />");
                sb.AppendLine("    <Property Name='Name' Type='String' Nullable='false' MaxLength='128' Unicode='true' FixedLength='false' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Tables'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Table'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tables'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("          <ScalarProperty Name='Name' ColumnName='Name' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Tables' EntityType='dbo.Tables' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Tables'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"      <PropertyRef Name='Name' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='Name' Type='nvarchar' Nullable='false' MaxLength='128' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class DateTime
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Tables' EntityType='DefaultNamespace.Table' />");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Table'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='LastUpdated' Type='DateTime' Nullable='false' />");
                sb.AppendLine("    <Property Name='TypeIsDateTime1' Type='DateTime' Nullable='false' />");
                sb.AppendLine("    <Property Name='TypeIsDateTime2' Type='DateTime' Nullable='false' />");
                sb.AppendLine("    <Property Name='Time' Type='Time' Nullable='false' />");
                sb.AppendLine("    <Property Name='Time_Precision0' Type='Time' Nullable='false' />");
                sb.AppendLine("    <Property Name='TimeStamp' Type='Binary' Nullable='false' MaxLength='8' FixedLength='true' annotation:StoreGeneratedPattern='Computed' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Tables'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Table'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tables'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("          <ScalarProperty Name='LastUpdated' ColumnName='LastUpdated' />");
                sb.AppendLine("          <ScalarProperty Name='TypeIsDateTime1' ColumnName='TypeIsDateTime1' />");
                sb.AppendLine("          <ScalarProperty Name='TypeIsDateTime2' ColumnName='TypeIsDateTime2' />");
                sb.AppendLine("          <ScalarProperty Name='Time' ColumnName='Time' />");
                sb.AppendLine("          <ScalarProperty Name='Time_Precision0' ColumnName='Time_Precision0' />");
                sb.AppendLine("          <ScalarProperty Name='TimeStamp' ColumnName='TimeStamp' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Tables' EntityType='dbo.Tables' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Tables'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='LastUpdated' Type='datetime' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='TypeIsDateTime1' Type='datetime' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='TypeIsDateTime2' Type='datetime2' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='Time' Type='time' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='Time_Precision0' Type='time' Nullable='false' Precision='0' />");
                sb.AppendLine(@"    <Property Name='TimeStamp' Type='timestamp' Nullable='false' StoreGeneratedPattern='Computed' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class GuidIdentity
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Tables' EntityType='DefaultNamespace.Table' />");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Table'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Guid' Nullable='false' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Tables'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Table'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tables'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Tables' EntityType='dbo.Tables' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Tables'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='uniqueidentifier' Nullable='false' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class InverseMapping
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Blogs' EntityType='DefaultNamespace.Blog' />");
                sb.AppendLine("    <EntitySet Name='People' EntityType='DefaultNamespace.Person' />");
                sb.AppendLine("    <EntitySet Name='Posts' EntityType='DefaultNamespace.Post' />");
                sb.AppendLine("    <AssociationSet Name='FK_dbo_Posts_dbo_Blogs_BlogId' Association='DefaultNamespace.FK_dbo_Posts_dbo_Blogs_BlogId'>");
                sb.AppendLine("      <End Role='Blogs' EntitySet='Blogs' />");
                sb.AppendLine("      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine("    </AssociationSet>");
                sb.AppendLine("    <AssociationSet Name='FK_dbo_Posts_dbo_People_CreatedBy_Id' Association='DefaultNamespace.FK_dbo_Posts_dbo_People_CreatedBy_Id'>");
                sb.AppendLine("      <End Role='People' EntitySet='People' />");
                sb.AppendLine("      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine("    </AssociationSet>");
                sb.AppendLine("    <AssociationSet Name='FK_dbo_Posts_dbo_People_UpdatedBy_Id' Association='DefaultNamespace.FK_dbo_Posts_dbo_People_UpdatedBy_Id'>");
                sb.AppendLine("      <End Role='People' EntitySet='People' />");
                sb.AppendLine("      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine("    </AssociationSet>");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Blog'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='Id' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='Id' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Name' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <NavigationProperty Name='Posts' Relationship='DefaultNamespace.FK_dbo_Posts_dbo_Blogs_BlogId' FromRole='Blogs' ToRole='Posts' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <EntityType Name='Person'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='Id' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='Id' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Name' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <NavigationProperty Name='Posts' Relationship='DefaultNamespace.FK_dbo_Posts_dbo_People_CreatedBy_Id' FromRole='People' ToRole='Posts' />");
                sb.AppendLine("    <NavigationProperty Name='Posts1' Relationship='DefaultNamespace.FK_dbo_Posts_dbo_People_UpdatedBy_Id' FromRole='People' ToRole='Posts' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <EntityType Name='Post'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='Id' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='Id' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Title' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <Property Name='BlogId' Type='Int32' Nullable='false' />");
                sb.AppendLine("    <Property Name='CreatedBy_Id' Type='Int32' />");
                sb.AppendLine("    <Property Name='UpdatedBy_Id' Type='Int32' />");
                sb.AppendLine("    <NavigationProperty Name='Blog' Relationship='DefaultNamespace.FK_dbo_Posts_dbo_Blogs_BlogId' FromRole='Posts' ToRole='Blogs' />");
                sb.AppendLine("    <NavigationProperty Name='Person' Relationship='DefaultNamespace.FK_dbo_Posts_dbo_People_CreatedBy_Id' FromRole='Posts' ToRole='People' />");
                sb.AppendLine("    <NavigationProperty Name='Person1' Relationship='DefaultNamespace.FK_dbo_Posts_dbo_People_UpdatedBy_Id' FromRole='Posts' ToRole='People' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <Association Name='FK_dbo_Posts_dbo_Blogs_BlogId'>");
                sb.AppendLine("    <End Role='Blogs' Type='DefaultNamespace.Blog' Multiplicity='1'>");
                sb.AppendLine("      <OnDelete Action='Cascade' />");
                sb.AppendLine("    </End>");
                sb.AppendLine("    <End Role='Posts' Type='DefaultNamespace.Post' Multiplicity='*' />");
                sb.AppendLine("    <ReferentialConstraint>");
                sb.AppendLine("      <Principal Role='Blogs'>");
                sb.AppendLine("        <PropertyRef Name='Id' />");
                sb.AppendLine("      </Principal>");
                sb.AppendLine("      <Dependent Role='Posts'>");
                sb.AppendLine("        <PropertyRef Name='BlogId' />");
                sb.AppendLine("      </Dependent>");
                sb.AppendLine("    </ReferentialConstraint>");
                sb.AppendLine("  </Association>");
                sb.AppendLine("  <Association Name='FK_dbo_Posts_dbo_People_CreatedBy_Id'>");
                sb.AppendLine("    <End Role='People' Type='DefaultNamespace.Person' Multiplicity='0..1' />");
                sb.AppendLine("    <End Role='Posts' Type='DefaultNamespace.Post' Multiplicity='*' />");
                sb.AppendLine("    <ReferentialConstraint>");
                sb.AppendLine("      <Principal Role='People'>");
                sb.AppendLine("        <PropertyRef Name='Id' />");
                sb.AppendLine("      </Principal>");
                sb.AppendLine("      <Dependent Role='Posts'>");
                sb.AppendLine("        <PropertyRef Name='CreatedBy_Id' />");
                sb.AppendLine("      </Dependent>");
                sb.AppendLine("    </ReferentialConstraint>");
                sb.AppendLine("  </Association>");
                sb.AppendLine("  <Association Name='FK_dbo_Posts_dbo_People_UpdatedBy_Id'>");
                sb.AppendLine("    <End Role='People' Type='DefaultNamespace.Person' Multiplicity='0..1' />");
                sb.AppendLine("    <End Role='Posts' Type='DefaultNamespace.Post' Multiplicity='*' />");
                sb.AppendLine("    <ReferentialConstraint>");
                sb.AppendLine("      <Principal Role='People'>");
                sb.AppendLine("        <PropertyRef Name='Id' />");
                sb.AppendLine("      </Principal>");
                sb.AppendLine("      <Dependent Role='Posts'>");
                sb.AppendLine("        <PropertyRef Name='UpdatedBy_Id' />");
                sb.AppendLine("      </Dependent>");
                sb.AppendLine("    </ReferentialConstraint>");
                sb.AppendLine("  </Association>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Blogs'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Blog'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Blogs'>");
                sb.AppendLine("          <ScalarProperty Name='Id' ColumnName='Id' />");
                sb.AppendLine("          <ScalarProperty Name='Name' ColumnName='Name' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("    <EntitySetMapping Name='People'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Person'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='People'>");
                sb.AppendLine("          <ScalarProperty Name='Id' ColumnName='Id' />");
                sb.AppendLine("          <ScalarProperty Name='Name' ColumnName='Name' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("    <EntitySetMapping Name='Posts'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Post'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Posts'>");
                sb.AppendLine("          <ScalarProperty Name='Id' ColumnName='Id' />");
                sb.AppendLine("          <ScalarProperty Name='Title' ColumnName='Title' />");
                sb.AppendLine("          <ScalarProperty Name='BlogId' ColumnName='BlogId' />");
                sb.AppendLine("          <ScalarProperty Name='CreatedBy_Id' ColumnName='CreatedBy_Id' />");
                sb.AppendLine("          <ScalarProperty Name='UpdatedBy_Id' ColumnName='UpdatedBy_Id' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Blogs' EntityType='dbo.Blogs' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <EntitySet Name='People' EntityType='dbo.People' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <EntitySet Name='Posts' EntityType='dbo.Posts' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <AssociationSet Name='FK_dbo_Posts_dbo_Blogs_BlogId' Association='dbo.FK_dbo_Posts_dbo_Blogs_BlogId'>");
                sb.AppendLine(@"      <End Role='Blogs' EntitySet='Blogs' />");
                sb.AppendLine(@"      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine(@"    </AssociationSet>");
                sb.AppendLine(@"    <AssociationSet Name='FK_dbo_Posts_dbo_People_CreatedBy_Id' Association='dbo.FK_dbo_Posts_dbo_People_CreatedBy_Id'>");
                sb.AppendLine(@"      <End Role='People' EntitySet='People' />");
                sb.AppendLine(@"      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine(@"    </AssociationSet>");
                sb.AppendLine(@"    <AssociationSet Name='FK_dbo_Posts_dbo_People_UpdatedBy_Id' Association='dbo.FK_dbo_Posts_dbo_People_UpdatedBy_Id'>");
                sb.AppendLine(@"      <End Role='People' EntitySet='People' />");
                sb.AppendLine(@"      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine(@"    </AssociationSet>");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Blogs'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='Id' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='Id' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Name' Type='nvarchar(max)' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <EntityType Name='People'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='Id' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='Id' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Name' Type='nvarchar(max)' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <EntityType Name='Posts'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='Id' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='Id' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Title' Type='nvarchar(max)' />");
                sb.AppendLine(@"    <Property Name='BlogId' Type='int' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='CreatedBy_Id' Type='int' />");
                sb.AppendLine(@"    <Property Name='UpdatedBy_Id' Type='int' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <Association Name='FK_dbo_Posts_dbo_Blogs_BlogId'>");
                sb.AppendLine(@"    <End Role='Blogs' Type='dbo.Blogs' Multiplicity='1'>");
                sb.AppendLine(@"      <OnDelete Action='Cascade' />");
                sb.AppendLine(@"    </End>");
                sb.AppendLine(@"    <End Role='Posts' Type='dbo.Posts' Multiplicity='*' />");
                sb.AppendLine(@"    <ReferentialConstraint>");
                sb.AppendLine(@"      <Principal Role='Blogs'>");
                sb.AppendLine(@"        <PropertyRef Name='Id' />");
                sb.AppendLine(@"      </Principal>");
                sb.AppendLine(@"      <Dependent Role='Posts'>");
                sb.AppendLine(@"        <PropertyRef Name='BlogId' />");
                sb.AppendLine(@"      </Dependent>");
                sb.AppendLine(@"    </ReferentialConstraint>");
                sb.AppendLine(@"  </Association>");
                sb.AppendLine(@"  <Association Name='FK_dbo_Posts_dbo_People_CreatedBy_Id'>");
                sb.AppendLine(@"    <End Role='People' Type='dbo.People' Multiplicity='0..1' />");
                sb.AppendLine(@"    <End Role='Posts' Type='dbo.Posts' Multiplicity='*' />");
                sb.AppendLine(@"    <ReferentialConstraint>");
                sb.AppendLine(@"      <Principal Role='People'>");
                sb.AppendLine(@"        <PropertyRef Name='Id' />");
                sb.AppendLine(@"      </Principal>");
                sb.AppendLine(@"      <Dependent Role='Posts'>");
                sb.AppendLine(@"        <PropertyRef Name='CreatedBy_Id' />");
                sb.AppendLine(@"      </Dependent>");
                sb.AppendLine(@"    </ReferentialConstraint>");
                sb.AppendLine(@"  </Association>");
                sb.AppendLine(@"  <Association Name='FK_dbo_Posts_dbo_People_UpdatedBy_Id'>");
                sb.AppendLine(@"    <End Role='People' Type='dbo.People' Multiplicity='0..1' />");
                sb.AppendLine(@"    <End Role='Posts' Type='dbo.Posts' Multiplicity='*' />");
                sb.AppendLine(@"    <ReferentialConstraint>");
                sb.AppendLine(@"      <Principal Role='People'>");
                sb.AppendLine(@"        <PropertyRef Name='Id' />");
                sb.AppendLine(@"      </Principal>");
                sb.AppendLine(@"      <Dependent Role='Posts'>");
                sb.AppendLine(@"        <PropertyRef Name='UpdatedBy_Id' />");
                sb.AppendLine(@"      </Dependent>");
                sb.AppendLine(@"    </ReferentialConstraint>");
                sb.AppendLine(@"  </Association>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class ManyToMany
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Posts' EntityType='DefaultNamespace.Post' />");
                sb.AppendLine("    <EntitySet Name='Tags' EntityType='DefaultNamespace.Tag' />");
                sb.AppendLine("    <AssociationSet Name='PostTags' Association='DefaultNamespace.PostTags'>");
                sb.AppendLine("      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine("      <End Role='Tags' EntitySet='Tags' />");
                sb.AppendLine("    </AssociationSet>");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Post'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='PostId' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='PostId' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <NavigationProperty Name='Tags' Relationship='DefaultNamespace.PostTags' FromRole='Posts' ToRole='Tags' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <EntityType Name='Tag'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='TagId' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='TagId' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Name' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <NavigationProperty Name='Posts' Relationship='DefaultNamespace.PostTags' FromRole='Tags' ToRole='Posts' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <Association Name='PostTags'>");
                sb.AppendLine("    <End Role='Posts' Type='DefaultNamespace.Post' Multiplicity='*' />");
                sb.AppendLine("    <End Role='Tags' Type='DefaultNamespace.Tag' Multiplicity='*' />");
                sb.AppendLine("  </Association>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Posts'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Post'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Posts'>");
                sb.AppendLine("          <ScalarProperty Name='PostId' ColumnName='PostId' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("    <EntitySetMapping Name='Tags'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Tag'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tags'>");
                sb.AppendLine("          <ScalarProperty Name='TagId' ColumnName='TagId' />");
                sb.AppendLine("          <ScalarProperty Name='Name' ColumnName='Name' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("    <AssociationSetMapping Name='PostTags' TypeName='DefaultNamespace.PostTags' StoreEntitySet='PostTags'>");
                sb.AppendLine("      <EndProperty Name='Posts'>");
                sb.AppendLine("        <ScalarProperty Name='PostId' ColumnName='Post_PostId' />");
                sb.AppendLine("      </EndProperty>");
                sb.AppendLine("      <EndProperty Name='Tags'>");
                sb.AppendLine("        <ScalarProperty Name='TagId' ColumnName='Tag_TagId' />");
                sb.AppendLine("      </EndProperty>");
                sb.AppendLine("    </AssociationSetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Posts' EntityType='dbo.Posts' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <EntitySet Name='PostTags' EntityType='dbo.PostTags' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <EntitySet Name='Tags' EntityType='dbo.Tags' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <AssociationSet Name='FK_dbo_PostTags_dbo_Posts_Post_PostId' Association='dbo.FK_dbo_PostTags_dbo_Posts_Post_PostId'>");
                sb.AppendLine(@"      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine(@"      <End Role='PostTags' EntitySet='PostTags' />");
                sb.AppendLine(@"    </AssociationSet>");
                sb.AppendLine(@"    <AssociationSet Name='FK_dbo_PostTags_dbo_Tags_Tag_TagId' Association='dbo.FK_dbo_PostTags_dbo_Tags_Tag_TagId'>");
                sb.AppendLine(@"      <End Role='Tags' EntitySet='Tags' />");
                sb.AppendLine(@"      <End Role='PostTags' EntitySet='PostTags' />");
                sb.AppendLine(@"    </AssociationSet>");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Posts'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='PostId' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='PostId' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <EntityType Name='PostTags'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='Post_PostId' />");
                sb.AppendLine(@"      <PropertyRef Name='Tag_TagId' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='Post_PostId' Type='int' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='Tag_TagId' Type='int' Nullable='false' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <EntityType Name='Tags'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='TagId' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='TagId' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Name' Type='nvarchar(max)' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <Association Name='FK_dbo_PostTags_dbo_Posts_Post_PostId'>");
                sb.AppendLine(@"    <End Role='Posts' Type='dbo.Posts' Multiplicity='1'>");
                sb.AppendLine(@"      <OnDelete Action='Cascade' />");
                sb.AppendLine(@"    </End>");
                sb.AppendLine(@"    <End Role='PostTags' Type='dbo.PostTags' Multiplicity='*' />");
                sb.AppendLine(@"    <ReferentialConstraint>");
                sb.AppendLine(@"      <Principal Role='Posts'>");
                sb.AppendLine(@"        <PropertyRef Name='PostId' />");
                sb.AppendLine(@"      </Principal>");
                sb.AppendLine(@"      <Dependent Role='PostTags'>");
                sb.AppendLine(@"        <PropertyRef Name='Post_PostId' />");
                sb.AppendLine(@"      </Dependent>");
                sb.AppendLine(@"    </ReferentialConstraint>");
                sb.AppendLine(@"  </Association>");
                sb.AppendLine(@"  <Association Name='FK_dbo_PostTags_dbo_Tags_Tag_TagId'>");
                sb.AppendLine(@"    <End Role='Tags' Type='dbo.Tags' Multiplicity='1'>");
                sb.AppendLine(@"      <OnDelete Action='Cascade' />");
                sb.AppendLine(@"    </End>");
                sb.AppendLine(@"    <End Role='PostTags' Type='dbo.PostTags' Multiplicity='*' />");
                sb.AppendLine(@"    <ReferentialConstraint>");
                sb.AppendLine(@"      <Principal Role='Tags'>");
                sb.AppendLine(@"        <PropertyRef Name='TagId' />");
                sb.AppendLine(@"      </Principal>");
                sb.AppendLine(@"      <Dependent Role='PostTags'>");
                sb.AppendLine(@"        <PropertyRef Name='Tag_TagId' />");
                sb.AppendLine(@"      </Dependent>");
                sb.AppendLine(@"    </ReferentialConstraint>");
                sb.AppendLine(@"  </Association>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class OneToMany
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Posts' EntityType='DefaultNamespace.Post' />");
                sb.AppendLine("    <EntitySet Name='Tags' EntityType='DefaultNamespace.Tag' />");
                sb.AppendLine("    <AssociationSet Name='FK_dbo_Tags_dbo_Posts_Post_ID' Association='DefaultNamespace.FK_dbo_Tags_dbo_Posts_Post_ID'>");
                sb.AppendLine("      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine("      <End Role='Tags' EntitySet='Tags' />");
                sb.AppendLine("    </AssociationSet>");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Post'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <NavigationProperty Name='Tags' Relationship='DefaultNamespace.FK_dbo_Tags_dbo_Posts_Post_ID' FromRole='Posts' ToRole='Tags' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <EntityType Name='Tag'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Post_ID' Type='Int32' />");
                sb.AppendLine("    <NavigationProperty Name='Post' Relationship='DefaultNamespace.FK_dbo_Tags_dbo_Posts_Post_ID' FromRole='Tags' ToRole='Posts' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("  <Association Name='FK_dbo_Tags_dbo_Posts_Post_ID'>");
                sb.AppendLine("    <End Role='Posts' Type='DefaultNamespace.Post' Multiplicity='0..1' />");
                sb.AppendLine("    <End Role='Tags' Type='DefaultNamespace.Tag' Multiplicity='*' />");
                sb.AppendLine("    <ReferentialConstraint>");
                sb.AppendLine("      <Principal Role='Posts'>");
                sb.AppendLine("        <PropertyRef Name='ID' />");
                sb.AppendLine("      </Principal>");
                sb.AppendLine("      <Dependent Role='Tags'>");
                sb.AppendLine("        <PropertyRef Name='Post_ID' />");
                sb.AppendLine("      </Dependent>");
                sb.AppendLine("    </ReferentialConstraint>");
                sb.AppendLine("  </Association>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Posts'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Post'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Posts'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("    <EntitySetMapping Name='Tags'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Tag'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tags'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("          <ScalarProperty Name='Post_ID' ColumnName='Post_ID' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Posts' EntityType='dbo.Posts' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <EntitySet Name='Tags' EntityType='dbo.Tags' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"    <AssociationSet Name='FK_dbo_Tags_dbo_Posts_Post_ID' Association='dbo.FK_dbo_Tags_dbo_Posts_Post_ID'>");
                sb.AppendLine(@"      <End Role='Posts' EntitySet='Posts' />");
                sb.AppendLine(@"      <End Role='Tags' EntitySet='Tags' />");
                sb.AppendLine(@"    </AssociationSet>");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Posts'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <EntityType Name='Tags'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Post_ID' Type='int' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"  <Association Name='FK_dbo_Tags_dbo_Posts_Post_ID'>");
                sb.AppendLine(@"    <End Role='Posts' Type='dbo.Posts' Multiplicity='0..1' />");
                sb.AppendLine(@"    <End Role='Tags' Type='dbo.Tags' Multiplicity='*' />");
                sb.AppendLine(@"    <ReferentialConstraint>");
                sb.AppendLine(@"      <Principal Role='Posts'>");
                sb.AppendLine(@"        <PropertyRef Name='ID' />");
                sb.AppendLine(@"      </Principal>");
                sb.AppendLine(@"      <Dependent Role='Tags'>");
                sb.AppendLine(@"        <PropertyRef Name='Post_ID' />");
                sb.AppendLine(@"      </Dependent>");
                sb.AppendLine(@"    </ReferentialConstraint>");
                sb.AppendLine(@"  </Association>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class Simple
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Tables' EntityType='DefaultNamespace.Table' />");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Table'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Name' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Tables'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Table'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tables'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("          <ScalarProperty Name='Name' ColumnName='Name' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Tables' EntityType='dbo.Tables' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Tables'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Name' Type='nvarchar(max)' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }

        public static class Strings
        {
            public static Stream Csdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Schema Namespace='DefaultNamespace' Alias='Self' p1:UseStrongSpatialTypes='false' xmlns:annotation='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns:p1='http://schemas.microsoft.com/ado/2009/02/edm/annotation' xmlns='http://schemas.microsoft.com/ado/2009/11/edm'>");
                sb.AppendLine("  <EntityContainer Name='MyContext' annotation:LazyLoadingEnabled='true'>");
                sb.AppendLine("    <EntitySet Name='Tables' EntityType='DefaultNamespace.Table' />");
                sb.AppendLine("  </EntityContainer>");
                sb.AppendLine("  <EntityType Name='Table'>");
                sb.AppendLine("    <Key>");
                sb.AppendLine("      <PropertyRef Name='ID' />");
                sb.AppendLine("    </Key>");
                sb.AppendLine("    <Property Name='ID' Type='Int32' Nullable='false' annotation:StoreGeneratedPattern='Identity' />");
                sb.AppendLine("    <Property Name='Default' Type='String' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <Property Name='Required' Type='String' Nullable='false' MaxLength='Max' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <Property Name='MaxLength128' Type='String' MaxLength='128' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <Property Name='MinLength100MaxLength128' Type='String' MaxLength='128' Unicode='true' FixedLength='false' />");
                sb.AppendLine("    <Property Name='NotUnicode' Type='String' MaxLength='Max' Unicode='false' FixedLength='false' />");
                sb.AppendLine("    <Property Name='FixedLength' Type='String' MaxLength='100' Unicode='true' FixedLength='true' />");
                sb.AppendLine("  </EntityType>");
                sb.AppendLine("</Schema>");
                return sb.ToString().ToStream();
            }

            public static Stream Msdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine("<Mapping Space='C-S' xmlns='http://schemas.microsoft.com/ado/2009/11/mapping/cs'>");
                sb.AppendLine("  <EntityContainerMapping StorageEntityContainer='dboContainer' CdmEntityContainer='MyContext'>");
                sb.AppendLine("    <EntitySetMapping Name='Tables'>");
                sb.AppendLine("      <EntityTypeMapping TypeName='DefaultNamespace.Table'>");
                sb.AppendLine("        <MappingFragment StoreEntitySet='Tables'>");
                sb.AppendLine("          <ScalarProperty Name='ID' ColumnName='ID' />");
                sb.AppendLine("          <ScalarProperty Name='Default' ColumnName='Default' />");
                sb.AppendLine("          <ScalarProperty Name='Required' ColumnName='Required' />");
                sb.AppendLine("          <ScalarProperty Name='MaxLength128' ColumnName='MaxLength128' />");
                sb.AppendLine("          <ScalarProperty Name='MinLength100MaxLength128' ColumnName='MinLength100MaxLength128' />");
                sb.AppendLine("          <ScalarProperty Name='NotUnicode' ColumnName='NotUnicode' />");
                sb.AppendLine("          <ScalarProperty Name='FixedLength' ColumnName='FixedLength' />");
                sb.AppendLine("        </MappingFragment>");
                sb.AppendLine("      </EntityTypeMapping>");
                sb.AppendLine("    </EntitySetMapping>");
                sb.AppendLine("  </EntityContainerMapping>");
                sb.AppendLine("</Mapping>");
                return sb.ToString().ToStream();
            }

            public static Stream Ssdl()
            {
                var sb = new StringBuilder();
                sb.AppendLine(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.AppendLine(@"<Schema Namespace='dbo' Alias='Self' Provider='System.Data.SqlClient' ProviderManifestToken='2008' xmlns:store='http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator' xmlns='http://schemas.microsoft.com/ado/2009/11/edm/ssdl'>");
                sb.AppendLine(@"  <EntityContainer Name='dboContainer'>");
                sb.AppendLine(@"    <EntitySet Name='Tables' EntityType='dbo.Tables' store:Type='Tables' Schema='dbo' />");
                sb.AppendLine(@"  </EntityContainer>");
                sb.AppendLine(@"  <EntityType Name='Tables'>");
                sb.AppendLine(@"    <Key>");
                sb.AppendLine(@"      <PropertyRef Name='ID' />");
                sb.AppendLine(@"    </Key>");
                sb.AppendLine(@"    <Property Name='ID' Type='int' Nullable='false' StoreGeneratedPattern='Identity' />");
                sb.AppendLine(@"    <Property Name='Default' Type='nvarchar(max)' />");
                sb.AppendLine(@"    <Property Name='Required' Type='nvarchar(max)' Nullable='false' />");
                sb.AppendLine(@"    <Property Name='MaxLength128' Type='nvarchar' MaxLength='128' />");
                sb.AppendLine(@"    <Property Name='MinLength100MaxLength128' Type='nvarchar' MaxLength='128' />");
                sb.AppendLine(@"    <Property Name='NotUnicode' Type='varchar(max)' />");
                sb.AppendLine(@"    <Property Name='FixedLength' Type='nchar' MaxLength='100' />");
                sb.AppendLine(@"  </EntityType>");
                sb.AppendLine(@"</Schema>");

                return sb.ToString().ToStream();
            }
        }
    }
}
