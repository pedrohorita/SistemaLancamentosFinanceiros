using ConsolidadoDiario.Domain.Entidades;

namespace ConsolidadoDiario.Domain.Interfaces.Respositorio;

public interface IConsolidadoDiarioCategoriaRepositorio
{
    Task AtualizarAsync(ConsolidadoDiarioCategoria consolidadoCategoria, CancellationToken cancellationToken);
    Task InserirAsync(ConsolidadoDiarioCategoria consolidadoCategoria, CancellationToken cancellationToken);
    Task<IEnumerable<ConsolidadoDiarioCategoria>> ObterConsolidadoDiarioCategoriaAsync(DateOnly data, CancellationToken token);
    Task<ConsolidadoDiarioCategoria> ObterConsolidadoPorCategoria(int categoriaId, DateOnly data, CancellationToken token);
    Task<IEnumerable<ConsolidadoDiarioCategoria>> ObterConsolidadosDiariosPorCategoriaAsync(int categoriaId, DateOnly dataInicio, DateOnly dataFim, CancellationToken token);
}
