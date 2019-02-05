<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations.Schema</Namespace>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Tag {
    public int ID { get; set; }
}

public class Post {
    public int ID { get; set; }
    public ICollection<Tag> Tags { get; set; }
}

public class Context : DbContext {
    static Context() {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
    }

    public Context(string connectionString)
            : base(connectionString)
    {
    }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}

void Main()
{
    using (var context = new Context("Server=localhost;Database=OneToMany;Trusted_Connection=True;")) {
            Console.WriteLine(context.Tags.Count());
    }
}


