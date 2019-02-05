<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Table {
    public int ID { get; set; }
    public string Name { get; set; }
}

public class Context : DbContext {
    static Context() {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
    }

    public Context(string connectionString)
            : base(connectionString)
    {
    }

    public DbSet<Table> Tables { get; set; }
}

void Main()
{
    using (var context = new Context("Server=localhost;Database=Simple;Trusted_Connection=True;")) {
            Console.WriteLine(context.Tables.Count());
    }
}


