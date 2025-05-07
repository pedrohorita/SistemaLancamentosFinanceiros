using ControleDeLancamentos.Domain.Entidades;

namespace ControleDeLancamentos.Domain.Interfaces.Repositorios;

public interface ILancamentoRepositorio 
{
    Task<IEnumerable<Lancamento>> ObterPaginadoAsync(int pagina, int quantidade, CancellationToken cancellationToken);
    Task<Lancamento?> ObterPorIdAsync(Guid id, CancellationToken token);
    Task<int> ObterQuantidadeAsync(CancellationToken cancellationToken);
    Task<Lancamento> InserirAsync(Lancamento lancamento, CancellationToken cancellationToken);    
    Task AtualizarAsync(Lancamento lancamento, CancellationToken cancellationToken);
    Task ExcluirAsync(Lancamento lancamento, CancellationToken cancellationToken);
}
