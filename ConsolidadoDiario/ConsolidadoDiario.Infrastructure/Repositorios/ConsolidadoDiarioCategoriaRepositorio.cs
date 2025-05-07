using ConsolidadoDiario.Domain.Entidades;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using ConsolidadoDiario.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Infrastructure.Repositorios;

[ExcludeFromCodeCoverage]
public class ConsolidadoDiarioCategoriaRepositorio(ConsolidadoCategoriaDbContext context) : IConsolidadoDiarioCategoriaRepositorio
{
    private readonly ConsolidadoCategoriaDbContext _context = context;

    public async Task AtualizarAsync(ConsolidadoDiarioCategoria consolidadoCategoria, CancellationToken cancellationToken)
    {
        _context.ConsolidadoDiarioCategorias.Update(consolidadoCategoria);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task InserirAsync(ConsolidadoDiarioCategoria consolidadoCategoria, CancellationToken cancellationToken)
    {
        await _context.ConsolidadoDiarioCategorias.AddAsync(consolidadoCategoria, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ConsolidadoDiarioCategoria>> ObterConsolidadoDiarioCategoriaAsync(DateOnly data, CancellationToken token)
    {
        var consolidado = await _context.ConsolidadoDiarioCategorias
         .Where(c => c.Data == data)
         .Include(c => c.Categoria)
         .ToListAsync(token);
        return consolidado;
    }
    public async Task<ConsolidadoDiarioCategoria> ObterConsolidadoPorCategoria(int categoriaId, DateOnly data, CancellationToken token)
    {
        var consolidado = await _context.ConsolidadoDiarioCategorias
            .Where(c => c.CategoriaId == categoriaId && c.Data == data)
            .Include(c => c.Categoria)
            .FirstOrDefaultAsync(token);

        return consolidado;
    }

    public async Task<IEnumerable<ConsolidadoDiarioCategoria>> ObterConsolidadosDiariosPorCategoriaAsync(int categoriaId, DateOnly dataInicio, DateOnly dataFim, CancellationToken token)
    {
        var consolidados = await _context.ConsolidadoDiarioCategorias
            .Where(c => c.CategoriaId == categoriaId && c.Data >= dataInicio && c.Data <= dataFim)
            .Include(c => c.Categoria)
            .ToListAsync(token);

        return consolidados;
    }

}
