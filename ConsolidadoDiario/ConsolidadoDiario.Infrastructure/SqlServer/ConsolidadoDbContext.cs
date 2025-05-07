using ConsolidadoDiario.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Infrastructure.SqlServer;

[ExcludeFromCodeCoverage]
public class ConsolidadoDbContext(DbContextOptions<ConsolidadoDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Entidades.ConsolidadoDiario> ConsolidadoDiarios { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);    

        modelBuilder.Entity<Domain.Entidades.ConsolidadoDiario>().ToTable("ConsolidadoDiarios");

        modelBuilder.Entity<ConsolidadoDiarioCategoria>().ToTable("ConsolidadoDiarioCategorias");

        modelBuilder.Ignore<Categoria>();
    }
}
