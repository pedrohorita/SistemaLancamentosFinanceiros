using ControleDeLancamentos.Application.Funcionalidades.Base;
using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Enums;
using ControleDeLancamentos.Domain.Interfaces;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Notificador;
using ControleDeLancamentos.Domain.Resultados;
using ControleDeLancamentos.Domain.Validadores;
using MediatR;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Atualizar;

public class AtualizarLancamentoCommandHandler(INotificador notificador, ILancamentoRepositorio lancamentoRepositorio,
    IProdutor produtor) 
    : BaseLancamentoHandler(notificador)
    , IRequestHandler<AtualizarLancamentoCommand, Resultado>
{
    private readonly ILancamentoRepositorio _lancamentoRepositorio = lancamentoRepositorio;
    private readonly IProdutor _produtor = produtor;
    public async Task<Resultado> Handle(AtualizarLancamentoCommand command, CancellationToken cancellationToken)
    {
        var lancamento = ObterLancamentoEntidade(command);
        lancamento.Id = command.Id;
        if (!await EntidadeEhValidaAsync(new LancamentoValidador(false), lancamento))
            return Erro();

        var lancamentoExistente = await _lancamentoRepositorio.ObterPorIdAsync(command.Id, cancellationToken);
        if (lancamentoExistente == null)
        {
            Notificar("Lançamento não encontrado.");
            return Erro();
        }

        lancamento.Data = lancamentoExistente.Data;
               
        await AtualizarLancamento(lancamento, lancamentoExistente, cancellationToken);

        return Ok();
    }

    private async Task AtualizarLancamento(Lancamento lancamento, Lancamento lancamentoExistente, CancellationToken cancellationToken)
    {
        if (DeveAtualizar(lancamento, lancamentoExistente))
        {
            await _lancamentoRepositorio.AtualizarAsync(lancamento, cancellationToken);
            var valor = lancamento.Valor - lancamentoExistente.Valor;
            var evento = ObterEvento(lancamento, valor);

            await _produtor.PublicarAsync(Constantes.RabbitMQ.QueueName, evento, cancellationToken);
        }
    }

    private static bool DeveAtualizar(Lancamento lancamento, Lancamento lancamentoExistente)
    {
        return lancamento.Descricao != lancamentoExistente.Descricao
            || lancamento.Valor != lancamentoExistente.Valor
            || lancamento.CategoriaId != lancamentoExistente.Categoria.Id
            || lancamento.TipoId != lancamentoExistente.Tipo.Id;
    }

    private static LancamentoEvento ObterEvento(Lancamento lancamento, decimal valor) =>
        new(lancamento.Id, DateOnly.FromDateTime(lancamento.Data), valor, lancamento.TipoId, lancamento.CategoriaId, TipoEvento.Atualizacao);
}
