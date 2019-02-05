<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Table {
    public int ID { get; set; }
    
    public string Default { get; set; }
    [Required] public string Required { get; set; }
    [MaxLength(128)] public string MaxLength128 { get; set; }
    [MinLength(100), MaxLength(128)] public string MinLength100MaxLength128 { get; set; }
    
    public string NotUnicode { get; set; }
    public string FixedLength { get; set; }
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
        
        modelBuilder.Entity<Table>().Property(x => x.NotUnicode).IsUnicode(false);
        modelBuilder.Entity<Table>().Property(x => x.FixedLength).HasMaxLength(100).IsFixedLength();
    }
}

void Main()
{
    using (var context = new Context("Server=localhost;Database=Strings;Trusted_Connection=True;")) {
            Console.WriteLine(context.Tables.Count());
    }
}


