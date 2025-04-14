Any changes to Mnemosyne context outside of the database should be made in a partial class. Pulling changes from the database will include overwriting the MnemosyneContext class in the root folder.

To re-scaffold the context and models from the database use this command:
dotnet ef dbcontext scaffold --output-dir Models --context-dir . "{ConnectionString}" Npgsql.EntityFrameworkCore.PostgreSQL --force --no-onconfiguring

`-force` makes it overwrite existing files
`-no-onconfiguring` prevents the generation of the OnConfiguring method in the context class, which causes the generator to include the connection string or connection string name. The connection string is configured outside of this class library.