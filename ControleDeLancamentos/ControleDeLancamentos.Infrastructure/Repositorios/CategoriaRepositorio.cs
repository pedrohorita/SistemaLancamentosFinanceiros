using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Infrastructure.Repositorios;

[ExcludeFromCodeCoverage]
public class CategoriaRepositorio(AppDbContext dbContext) : ICategoriaRepositorio
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<bool> ExisteAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Categoria?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Categoria>> ObterTodosAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Categorias
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
