using ControleDeLancamentos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Infrastructure.SqlServer;

[ExcludeFromCodeCoverage]
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Lancamento> Lancamentos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Tipo> Tipos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Lancamento>(entity =>
        {
            entity.ToTable("Lancamentos");
        });

        modelBuilder.Entity<Categoria>().ToTable("Categorias").HasData(
            new(1, "Vendas", "Lançamentos relacionados a venda", new DateTime(2025, 04, 29),"admin", null, null) ,
            new(2, "Emprestimos", "Lançamentos relacionados a empréstimos", new DateTime(2025, 04, 29), "admin", null, null) ,
            new(3, "Despesas", "Lançamentos relacionados a despesas", new DateTime(2025, 04, 29), "admin", null, null) ,
            new(4, "Investimentos", "Lançamentos relacionados a investimentos", new DateTime(2025, 04, 29), "admin", null, null) ,
            new(5, "Outros", "Lançamentos relacionados a outros", new DateTime(2025, 04, 29), "admin", null, null) 
        );

        modelBuilder.Entity<Tipo>().ToTable("Tipos").HasData(
            new(1, "Entrada", "Lançamentos de entrada", new DateTime(2025, 04, 29), "admin", null, null),
            new(2, "Saída", "Lançamentos de saída", new DateTime(2025, 04, 29), "admin", null, null)
        );
    }
}
