using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Entidades;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.InclusaoLancamento;

public class InclusaoLancamentoCommandHandler(IConsolidadoDiarioRepositorio consolidadoDiarioRepositorio,
    IConsolidadoDiarioCategoriaRepositorio consolidadoDiarioCategoriaRepositorio)
    : IRequestHandler<InclusaoLancamentoCommand, Resultado>
{
    private readonly IConsolidadoDiarioRepositorio _consolidadoDiarioRepositorio = consolidadoDiarioRepositorio;
    private readonly IConsolidadoDiarioCategoriaRepositorio _consolidadoDiarioCategoriaRepositorio = consolidadoDiarioCategoriaRepositorio;
    public async Task<Resultado> Handle(InclusaoLancamentoCommand request, CancellationToken cancellationToken)
    {
        var consolidadoTask = ProcessarInclusaoConsolidado(request.Evento, cancellationToken);
        var consolidadoCategoriaTask = ProcessarInclusaoConsolidadoCategoria(request.Evento, cancellationToken);

        await Task.WhenAll(consolidadoTask, consolidadoCategoriaTask);

        return Resultado.Ok();
    }

    private async Task ProcessarInclusaoConsolidado(LancamentoEvento evento, CancellationToken cancellationToken)
    {
        //var consolidado = await _consolidadoDbContext.ConsolidadoDiarios
        //    .FirstOrDefaultAsync(x => x.Data == evento.Data, cancellationToken);

        var consolidado = await _consolidadoDiarioRepositorio
            .ObterConsolidadoDiarioPorDataAsync(evento.Data, cancellationToken);


        if (consolidado == null)
        {
            consolidado = ObterConsolidado(evento);
            //await _consolidadoDbContext.ConsolidadoDiarios.AddAsync(consolidado, cancellationToken);
            await _consolidadoDiarioRepositorio.InserirAsync(consolidado, cancellationToken);
        }
        else
        {
            consolidado.TotalReceitas += evento.TipoId == (int)TipoLancamento.Entrada ? evento.Valor : 0;
            consolidado.TotalDespesas += evento.TipoId == (int)TipoLancamento.Saida ? evento.Valor : 0;
            consolidado.Saldo = consolidado.TotalReceitas - consolidado.TotalDespesas;
            consolidado.AtualizadoEm = DateTime.UtcNow;
            //_consolidadoDbContext.ConsolidadoDiarios.Update(consolidado);

            await _consolidadoDiarioRepositorio.AtualizarAsync(consolidado, cancellationToken);
        }
        
    }

    private async Task ProcessarInclusaoConsolidadoCategoria(LancamentoEvento evento, CancellationToken cancellationToken)
    {
        //var consolidadoCategoria = await _consolidadoCategoriaDbContext.ConsolidadoDiarioCategorias
        //    .FirstOrDefaultAsync(x => x.Data == evento.Data && x.CategoriaId == evento.CategoriaId, cancellationToken);

        var consolidadoCategoria = await _consolidadoDiarioCategoriaRepositorio
            .ObterConsolidadoPorCategoria(evento.CategoriaId, evento.Data, cancellationToken);

        if (consolidadoCategoria == null)
        {
            consolidadoCategoria = ObterConsolidadoCategoria(evento);
            //await _consolidadoCategoriaDbContext.ConsolidadoDiarioCategorias.AddAsync(consolidadoCategoria, cancellationToken);
            await _consolidadoDiarioCategoriaRepositorio.InserirAsync(consolidadoCategoria, cancellationToken);
        }
        else
        {
            consolidadoCategoria.TotalReceitas += evento.TipoId == (int)TipoLancamento.Entrada ? evento.Valor : 0;
            consolidadoCategoria.TotalDespesas += evento.TipoId == (int)TipoLancamento.Saida ? evento.Valor : 0;
            consolidadoCategoria.Saldo = consolidadoCategoria.TotalReceitas - consolidadoCategoria.TotalDespesas;
            consolidadoCategoria.AtualizadoEm = DateTime.UtcNow;
            //_consolidadoCategoriaDbContext.ConsolidadoDiarioCategorias.Update(consolidadoCategoria);
            await _consolidadoDiarioCategoriaRepositorio.AtualizarAsync(consolidadoCategoria, cancellationToken);
        }
    }

    private static Domain.Entidades.ConsolidadoDiario ObterConsolidado(LancamentoEvento evento)
    {
        var consolidado = new Domain.Entidades.ConsolidadoDiario()
        {
            Data = evento.Data,
            TotalReceitas = evento.TipoId == (int)TipoLancamento.Entrada ? evento.Valor : 0,
            TotalDespesas = evento.TipoId == (int)TipoLancamento.Saida ? evento.Valor : 0,
            CriadoEm = DateTime.UtcNow
        };

        consolidado.Saldo = consolidado.TotalReceitas - consolidado.TotalDespesas;

        return consolidado;
    }

    private static ConsolidadoDiarioCategoria ObterConsolidadoCategoria(LancamentoEvento evento)
    {
        var consolidado = new ConsolidadoDiarioCategoria()
        {
            Data = evento.Data,
            CategoriaId = evento.CategoriaId,
            TotalReceitas = evento.TipoId == (int)TipoLancamento.Entrada ? evento.Valor : 0,
            TotalDespesas = evento.TipoId == (int)TipoLancamento.Saida ? evento.Valor : 0,
            CriadoEm = DateTime.UtcNow
        };
        consolidado.Saldo = consolidado.TotalReceitas - consolidado.TotalDespesas;
        return consolidado;
    }

}
