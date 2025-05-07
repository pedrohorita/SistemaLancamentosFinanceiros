using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using ConsolidadoDiario.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Infrastructure.Repositorios;

[ExcludeFromCodeCoverage]
public class ConsolidadoDiarioRepositorio(ConsolidadoDbContext context) : IConsolidadoDiarioRepositorio
{
    private readonly ConsolidadoDbContext _context = context;

    public async Task AtualizarAsync(Domain.Entidades.ConsolidadoDiario consolidado, CancellationToken cancellationToken)
    {
        _context.ConsolidadoDiarios.Update(consolidado);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task InserirAsync(Domain.Entidades.ConsolidadoDiario consolidado, CancellationToken cancellationToken)
    {
        await _context.ConsolidadoDiarios.AddAsync(consolidado, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Domain.Entidades.ConsolidadoDiario> ObterConsolidadoDiarioPorDataAsync(DateOnly data, CancellationToken token)
    {
        var consolidado = await _context.ConsolidadoDiarios
            .Where(c => c.Data == data)
            .FirstOrDefaultAsync(token);

        return consolidado;
    }

    public async Task<IEnumerable<Domain.Entidades.ConsolidadoDiario>> ObterConsolidadosDiariosAsync(DateOnly dataInicio, DateOnly dataFim, CancellationToken token)
    {
        var consolidados = await _context.ConsolidadoDiarios
            .Where(c => c.Data >= dataInicio && c.Data <= dataFim)
            .ToListAsync(token);

        return consolidados;
    }

}
