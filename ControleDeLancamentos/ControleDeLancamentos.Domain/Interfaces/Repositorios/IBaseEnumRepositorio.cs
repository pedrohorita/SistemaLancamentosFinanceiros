namespace ControleDeLancamentos.Domain.Interfaces.Repositorios;

public interface IBaseEnumRepositorio<TEntity>
{
    Task<TEntity?> ObterPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> ObterTodosAsync(CancellationToken cancellationToken);
    Task<bool> ExisteAsync(int id, CancellationToken cancellationToken);
}
