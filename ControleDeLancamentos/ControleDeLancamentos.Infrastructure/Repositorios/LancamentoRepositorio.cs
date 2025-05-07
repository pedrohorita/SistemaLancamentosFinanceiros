using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Infrastructure.Repositorios;

[ExcludeFromCodeCoverage]
public class LancamentoRepositorio(AppDbContext dbContext) : ILancamentoRepositorio
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IEnumerable<Lancamento>> ObterPaginadoAsync(int pagina, int quantidade, CancellationToken cancellationToken)
    {
        var lancamentos = await _dbContext.Lancamentos            
            .AsNoTracking()
            .OrderByDescending(l => l.Data)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)            
            .Include(l => l.Categoria)
            .Include(l => l.Tipo)
            .ToListAsync(cancellationToken);
        return lancamentos;
    }

    public async Task<Lancamento?> ObterPorIdAsync(Guid id, CancellationToken token)
    {
        var lancamento = await _dbContext.Lancamentos            
            .Include(l => l.Categoria)
            .Include(l => l.Tipo)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id, token);

        return lancamento;
    }

    public async Task<int> ObterQuantidadeAsync(CancellationToken cancellationToken)
    {
        var total = await _dbContext.Lancamentos.CountAsync(cancellationToken);
        return total;
    }

    public async Task<Lancamento> InserirAsync(Lancamento lancamento, CancellationToken cancellationToken)
    {
        await _dbContext.Lancamentos.AddAsync(lancamento, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return lancamento;
    }

    public Task AtualizarAsync(Lancamento lancamento, CancellationToken cancellationToken)
    {
        _dbContext.Lancamentos.Update(lancamento);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task ExcluirAsync(Lancamento lancamento, CancellationToken cancellationToken)
    {
        _dbContext.Lancamentos.Remove(lancamento);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
