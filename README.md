# Sloos

Sloos is a tool to make it as easy as possible to bulk load CSV data
into SQL.  Sloos attempts to automatically understand the structure of
a CSV document.  Sloos tries to automatically determine:

 1. The delimiter of the CSV document, e.g. [',', ';', '\t', '|', ':']
 1. The presense of a header or not.
 1. The type of a column, e.g. string, bool, char, int, long, ulong, double, Guid, DateTime, TimeSpan.
 1. If the column value is optional or required, i.e. int? vs. int.

Sloos uses [Entity Framework Core][1] (aka EF Core) to interact with
SQL.  EF Core is used to create the database if it does not exist, or
create the tables if it does not exist.  Sloos uses [SqlBulkCopy][2]
to import the data into SQL.

Sloos targets .NET Core 3.1 LTS, EF Core 3, and SQL Server.  EF Core
supports may more databases, but I am only testing against SQL Server.

```sh
sloos --table "MyTable" \
      --source "c:\csv\my.csv" \
      --target "Server=localhost;Initial Catalog=MyDB;Trusted_Connection=true"
```

## Name

Sloos is a riff on the word `sluice`.  The term sluice conveys my
intent, which is a utility to control the flow of ~~water~~data.  I
think `sloos` will prove easier to find for any future searchers.

# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.


[1]: https://docs.microsoft.com/en-us/ef/core/.
[2]: https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlbulkcopy?view=netframework-4.8&viewFallbackFrom=netcore-3.1
