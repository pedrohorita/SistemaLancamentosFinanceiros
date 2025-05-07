using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Enums;
using ControleDeLancamentos.Domain.Interfaces;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Resultados;
using MediatR;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Excluir;

public class ExcluirLancamentoCommandHandler(ILancamentoRepositorio lancamentoRepositorio, IProdutor produtor) 
    : IRequestHandler<ExcluirLancamentoCommand, Resultado>
{
    private readonly ILancamentoRepositorio _lancamentoRepositorio = lancamentoRepositorio;
    private readonly IProdutor _produtor = produtor;
    public async Task<Resultado> Handle(ExcluirLancamentoCommand request, CancellationToken cancellationToken)
    {
        var lancamento = await _lancamentoRepositorio.ObterPorIdAsync(request.Id, cancellationToken);
        if (lancamento == null)
            return Resultado.Erro(["Lançamento não encontrado."]);

        await _lancamentoRepositorio.ExcluirAsync(lancamento, cancellationToken);
        
        var evento = ObterEvento(lancamento);

        await _produtor.PublicarAsync(Constantes.RabbitMQ.QueueName, evento, cancellationToken);


        return Resultado.Ok();
    }

    private static LancamentoEvento ObterEvento(Lancamento lancamento) =>
        new(lancamento.Id, DateOnly.FromDateTime(lancamento.Data), lancamento.Valor, lancamento.TipoId, lancamento.CategoriaId, TipoEvento.Exclusao);
}
