<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations.Schema</Namespace>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Table {
    public int ID { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime LastUpdated { get; set; }
    public DateTime TypeIsDateTime1{ get; set; }
    public DateTime TypeIsDateTime2{ get; set; }
    public TimeSpan Time { get; set; }
    public TimeSpan Time_Precision0 { get; set; }
    
    [Timestamp]
    public byte[] TimeStamp { get; set; }
}

public class Context : DbContext {
    static Context() {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
    }

    public Context(string connectionString)
            : base(connectionString)
    {
    }

    public DbSet<Table> Tables{ get; set; }
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Table>().Property(x => x.TypeIsDateTime1).HasColumnType("datetime");
        modelBuilder.Entity<Table>().Property(x => x.TypeIsDateTime2).HasColumnType("datetime2");
        modelBuilder.Entity<Table>().Property(x => x.Time_Precision0).HasPrecision(0);
    }
}

void Main()
{
    using (var context = new Context("Server=localhost;Database=DateTime;Trusted_Connection=True;")) {
            Console.WriteLine(context.Tables.Count());
    }
}


