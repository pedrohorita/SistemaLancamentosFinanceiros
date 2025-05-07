using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Infrastructure.SqlServer.DesignTime;

[ExcludeFromCodeCoverage]
public class ConsolidadoDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConsolidadoDbContext>
{
    public ConsolidadoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ConsolidadoDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                               ?? "Server=localhost,14333;Database=ControleDeLancamentos;User Id=sa;Password=X7@pL9qW#zT2;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);

        return new ConsolidadoDbContext(optionsBuilder.Options);
    }
}
