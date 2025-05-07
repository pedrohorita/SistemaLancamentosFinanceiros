using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Infrastructure.SqlServer;

[ExcludeFromCodeCoverage]
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Use a string de conexão do ambiente de desenvolvimento
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? "Server=localhost,14333;Database=ControleDeLancamentos;User Id=sa;Password=X7@pL9qW#zT2;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
