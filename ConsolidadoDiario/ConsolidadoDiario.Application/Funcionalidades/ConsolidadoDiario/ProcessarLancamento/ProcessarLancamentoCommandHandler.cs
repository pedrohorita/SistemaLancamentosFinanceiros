using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.AtualizacaoLancamento;
using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ExclusaoLancamento;
using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.InclusaoLancamento;
using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ProcessarLancamento;

public class ProcessarLancamentoCommandHandler(IMediator mediator) : IRequestHandler<ProcessarLancamentoCommand, Resultado>
{
    private readonly IMediator _mediator = mediator;

    private readonly IDictionary<TipoEvento, Func<LancamentoEvento, IRequest<Resultado>>> _commandoMapeamento =
        new Dictionary<TipoEvento, Func<LancamentoEvento, IRequest<Resultado>>>
        {
            { TipoEvento.Criacao, (evento) => new InclusaoLancamentoCommand(evento) },
            { TipoEvento.Exclusao, (evento) => new ExclusaoLancamentoCommand(evento) },
            { TipoEvento.Atualizacao, (evento) => new AtualizacaoLancamentoCommand(evento) }
        };
    public async Task<Resultado> Handle(ProcessarLancamentoCommand request, CancellationToken cancellationToken)
    {
        var funcaoComando = _commandoMapeamento[request.evento.TipoEvento];
        var comando = funcaoComando(request.evento);

        try
        {
            Console.WriteLine($"Processando evento: {request.evento.TipoEvento} - {request.evento.Id}");
            return await _mediator.Send(comando, cancellationToken);         

        }
        catch(Exception ex)
        {
            // Logar o erro
            Console.WriteLine($"Erro ao processar o evento: {ex.Message}");
            return Resultado.Erro(["Erro ao processar o evento"]);
        }
    }
}
