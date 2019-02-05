<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.ComponentModel.DataAnnotations</Namespace>
  <Namespace>System.ComponentModel.DataAnnotations.Schema</Namespace>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Person {
    public int Id { get; set; }
    public string Name { get; set; }
    
    [InverseProperty("CreatedBy")]
    public List<Post> PostsWritten { get; set; }
    
    [InverseProperty("UpdatedBy")]
    public List<Post> PostsUpdated { get; set; }
}

public class Post {
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int BlogId { get; set; }
    [ForeignKey("BlogId")] public Blog Blog { get; set; }
    
    public Person CreatedBy { get; set; }
    public Person UpdatedBy { get; set; }
}

public class Blog {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }
}

public class Context : DbContext {
    static Context() {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
    }

    public Context(string connectionString)
            : base(connectionString)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    
    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }
}

void Main()
{
    using (var context = new Context("Server=localhost;Database=InverseMapping;Trusted_Connection=True;")) {
            Console.WriteLine(context.Blogs.Count());
    }
}


