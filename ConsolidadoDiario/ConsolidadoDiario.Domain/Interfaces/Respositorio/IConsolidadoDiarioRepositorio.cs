

namespace ConsolidadoDiario.Domain.Interfaces.Respositorio;

public interface IConsolidadoDiarioRepositorio
{
    Task AtualizarAsync(Entidades.ConsolidadoDiario consolidado, CancellationToken cancellationToken);
    Task InserirAsync(Entidades.ConsolidadoDiario consolidado, CancellationToken cancellationToken);
    Task<Entidades.ConsolidadoDiario> ObterConsolidadoDiarioPorDataAsync(DateOnly data, CancellationToken token);
    Task<IEnumerable<Entidades.ConsolidadoDiario>> ObterConsolidadosDiariosAsync(DateOnly dataInicio, DateOnly dataFim, CancellationToken token);
}
