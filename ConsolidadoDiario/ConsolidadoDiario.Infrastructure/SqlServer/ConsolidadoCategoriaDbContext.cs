using ConsolidadoDiario.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Infrastructure.SqlServer;

[ExcludeFromCodeCoverage]
public class ConsolidadoCategoriaDbContext(DbContextOptions<ConsolidadoCategoriaDbContext> options) : DbContext(options)
{
    public DbSet<ConsolidadoDiarioCategoria> ConsolidadoDiarioCategorias { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
}




