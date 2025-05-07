using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.AtualizacaoLancamento;

public class AtualizacaoLancamentoCommandhandler(IConsolidadoDiarioRepositorio consolidadoDiarioRepositorio,
    IConsolidadoDiarioCategoriaRepositorio consolidadoDiarioCategoriaRepositorio)
    : IRequestHandler<AtualizacaoLancamentoCommand, Resultado>
{
    private readonly IConsolidadoDiarioRepositorio _consolidadoDiarioRepositorio = consolidadoDiarioRepositorio;
    private readonly IConsolidadoDiarioCategoriaRepositorio _consolidadoDiarioCategoriaRepositorio = consolidadoDiarioCategoriaRepositorio;
    public async Task<Resultado> Handle(AtualizacaoLancamentoCommand request, CancellationToken cancellationToken)
    {
        var consolidadoTask = ProcessarAtualizacaoConsolidado(request.Evento, cancellationToken);
        var consolidadoCategoriaTask = ProcessarAtualizacaoConsolidadoCategoria(request.Evento, cancellationToken);

        await Task.WhenAll(consolidadoTask, consolidadoCategoriaTask);

        return Resultado.Ok();
    }

    private async Task ProcessarAtualizacaoConsolidado(LancamentoEvento evento, CancellationToken cancellationToken)
    {
        //var consolidado = await _consolidadoDbContext.ConsolidadoDiarios
        //.FirstOrDefaultAsync(x => x.Data == evento.Data, cancellationToken);

        var consolidado = await _consolidadoDiarioRepositorio
            .ObterConsolidadoDiarioPorDataAsync(evento.Data, cancellationToken);

        if (consolidado != null)
        {
            consolidado.TotalReceitas += evento.TipoId == (int)TipoLancamento.Entrada ? evento.Valor : 0;
            consolidado.TotalDespesas += evento.TipoId == (int)TipoLancamento.Saida ? evento.Valor : 0;
            consolidado.Saldo = consolidado.TotalReceitas - consolidado.TotalDespesas;
            consolidado.AtualizadoEm = DateTime.UtcNow;

            //_consolidadoDbContext.ConsolidadoDiarios.Update(consolidado);
            //await _consolidadoDbContext.SaveChangesAsync(cancellationToken);
            await _consolidadoDiarioRepositorio.AtualizarAsync(consolidado, cancellationToken);
        }
    }

    private async Task ProcessarAtualizacaoConsolidadoCategoria(LancamentoEvento evento, CancellationToken cancellationToken)
    {
        //var consolidadoCategoria = await _consolidadoCategoriaDbContext.ConsolidadoDiarioCategorias
        //    .FirstOrDefaultAsync(x => x.Data == evento.Data && x.CategoriaId == evento.CategoriaId, cancellationToken);
        var consolidadoCategoria = await _consolidadoDiarioCategoriaRepositorio
            .ObterConsolidadoPorCategoria(evento.CategoriaId, evento.Data, cancellationToken);

        if (consolidadoCategoria != null)
        {
            consolidadoCategoria.TotalReceitas += evento.TipoId == (int)TipoLancamento.Entrada ? evento.Valor : 0;
            consolidadoCategoria.TotalDespesas += evento.TipoId == (int)TipoLancamento.Saida ? evento.Valor : 0;
            consolidadoCategoria.Saldo = consolidadoCategoria.TotalReceitas - consolidadoCategoria.TotalDespesas;
            consolidadoCategoria.AtualizadoEm = DateTime.UtcNow;

            //_consolidadoCategoriaDbContext.ConsolidadoDiarioCategorias.Update(consolidadoCategoria);
            //await _consolidadoCategoriaDbContext.SaveChangesAsync(cancellationToken);

            await _consolidadoDiarioCategoriaRepositorio.AtualizarAsync(consolidadoCategoria, cancellationToken);
        }
    }
}
