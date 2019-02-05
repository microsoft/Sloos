<Query Kind="Program">
  <NuGetReference Version="5.0.0">EntityFramework</NuGetReference>
  <Namespace>System.Data.Entity</Namespace>
</Query>

#define NONEST

public class Table {
    public int ID { get; set; }
    public byte byte_type { get; set; }
    public bool bool_type { get; set; }
//    public sbyte sbyte_type { get; set; }
    public char char_type { get; set; }
    public short short_type { get; set; }
    public ushort ushort_type { get; set; }
    public int int_type { get; set; }
    public uint uint_type { get; set; }
    public long long_type { get; set; }
    public ulong ulong_type { get; set; }
    public float float_type { get; set; }
    public double double_type { get; set; }
    public decimal decimal_type { get; set; }
    public Guid Guid_type { get; set; }
    public TimeSpan TimeSpan_type { get; set; }
    public DateTime DateTime_type { get; set; }
    public DateTimeOffset DateTimeOffset_type { get; set; }

    public byte? byte_nullable { get; set; }
    public bool? bool_nullable { get; set; }
//    public sbyte? sbyte_nullable { get; set; }
    public char? char_nullable { get; set; }
    public short? short_nullable { get; set; }
    public ushort? ushort_nullable { get; set; }
    public int? int_nullable { get; set; }
    public uint? uint_nullable { get; set; }
    public long? long_nullable { get; set; }
    public ulong? ulong_nullable { get; set; }
    public float? float_nullable { get; set; }
    public double? double_nullable { get; set; }
    public decimal? decimal_nullable { get; set; }
    public Guid? Guid_nullable { get; set; }
    public TimeSpan? TimeSpan_nullable { get; set; }
    public DateTime? DateTime_nullable { get; set; }
    public DateTimeOffset? DateTimeOffset_nullable { get; set; }

    public byte[] byte_array_type { get; set; }
    public string string_type { get; set; }
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
    using (var context = new Context("Server=localhost;Database=AllClrTypes;Trusted_Connection=True;")) {
            Console.WriteLine(context.Tables.Count());
    }
}


