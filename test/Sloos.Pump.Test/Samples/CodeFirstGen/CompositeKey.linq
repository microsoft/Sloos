<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations.Schema</Namespace>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Table {
    [Key]
    [Column(Order=1)]
    public int ID { get; set; }
    
    [Key]
    [Column(Order=2)]
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
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}

void Main()
{
    using (var context = new Context("Server=localhost;Database=CompositeKey;Trusted_Connection=True;")) {
            Console.WriteLine(context.Tables.Count());
    }
}


