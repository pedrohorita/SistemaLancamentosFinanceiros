using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Infrastructure.Repositorios;

[ExcludeFromCodeCoverage]
public class TipoRepositorio(AppDbContext dbContext) : ITipoRepositorio
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<bool> ExisteAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tipos
            .AsNoTracking()
            .AnyAsync(t => t.Id.Equals(id), cancellationToken);
    }

    public async Task<Tipo?> ObterPorIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tipos
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id.Equals(id), cancellationToken);
    }

    public async Task<IEnumerable<Tipo>> ObterTodosAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Tipos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
