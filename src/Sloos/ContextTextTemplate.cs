using System;
using System.Text;

// NOTE(chrboum): I cannot make any of the replacement T4 engines work, so I
// am doing it myself.
namespace Sloos
{
    public class ContextTextTemplate
    {
        public static string Template(string ns, string rowName, string tableName, string keyName, Column[] columns)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@$"using System;");
            sb.AppendLine(@$"using System.Linq;");
            sb.AppendLine(@$"using System.ComponentModel.DataAnnotations;");
            sb.AppendLine(@$"using System.Runtime.Serialization;");
            sb.AppendLine(@$"using Microsoft.EntityFrameworkCore;");
            sb.AppendLine(@$"");
            sb.AppendLine(@$"namespace {ns}");
            sb.AppendLine(@$"{{");
            sb.AppendLine(@$"	public class Context : DbContext, IEntityPump");
            sb.AppendLine(@$"	{{");
            sb.AppendLine(@$"		public Context(DbContextOptions options)");
            sb.AppendLine(@$"			: base(options)");
            sb.AppendLine(@$"		{{");
            sb.AppendLine(@$"		}}");
            sb.AppendLine(@$"");
            sb.AppendLine(@$"		public DbSet<{rowName}> {tableName} {{ get; set; }}");
            sb.AppendLine(@$"");
            sb.AppendLine(@$"		/// <summary>");
            sb.AppendLine(@$"		///     Attempt to create the database and table if either or both do not exist.");
            sb.AppendLine(@$"		/// </summary>");
            sb.AppendLine(@$"		public void CreateIfNotExists()");
            sb.AppendLine(@$"		{{");
            sb.AppendLine(@$"			// EnsureCreated creates the DB if it does not exist, and creates table(s) if *no*");
            sb.AppendLine(@$"			// tables exist.  If a new table is being created, but other tables already exist");
            sb.AppendLine(@$"			// EnsureCreated is a no-op hence the need to call CreateTables() in some cases.");
            sb.AppendLine(@$"			this.Database.EnsureCreated();");
            sb.AppendLine(@$"			var creator = (Microsoft.EntityFrameworkCore.Storage.RelationalDatabaseCreator)Microsoft.EntityFrameworkCore.Infrastructure.AccessorExtensions.GetService<Microsoft.EntityFrameworkCore.Storage.IDatabaseCreator>(this.Database);");
            sb.AppendLine(@$"");
            sb.AppendLine(@$"			try");
            sb.AppendLine(@$"			{{");
            sb.AppendLine(@$"				this.{tableName}.Any();");
            sb.AppendLine(@$"			}}");
            sb.AppendLine(@$"			catch (System.Data.SqlClient.SqlException e)");
            sb.AppendLine(@$"			{{");
            sb.AppendLine(@$"				if (e.Number != 208)");
            sb.AppendLine(@$"				{{");
            sb.AppendLine(@$"					throw;");
            sb.AppendLine(@$"				}}");
            sb.AppendLine(@$"");
            sb.AppendLine(@$"				creator.CreateTables();");
            sb.AppendLine(@$"			}}");
            sb.AppendLine(@$"		}}");
            sb.AppendLine(@$"	}}");
            sb.AppendLine(@$"");

            sb.AppendLine(@$"	[DataContract]");
            sb.AppendLine(@$"	public class {rowName}");
            sb.AppendLine(@$"	{{");
            sb.AppendLine(@$"		[Key]");
            sb.AppendLine(@$"		public long {keyName} {{ get; set; }}");
            sb.AppendLine(@$"");
            for (int i = 0; i < columns.Length; i++)
            {
                var c = columns[i];
                var required = c.TypeName.EndsWith("?") ? string.Empty : "[Required]";

                sb.AppendLine(@$"		[DataMember(Order = {i})] {required} public {c.TypeName} {c.Name} {{ get; set; }}");
            }
            sb.AppendLine(@$"	}}");
            sb.AppendLine(@$"}}");

            return sb.ToString();
        }
    }
}
