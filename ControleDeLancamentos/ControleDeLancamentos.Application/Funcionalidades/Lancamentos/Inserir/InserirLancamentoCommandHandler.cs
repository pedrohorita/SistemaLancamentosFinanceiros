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

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;

public class InserirLancamentoCommandHandler(INotificador notificador, 
    ILancamentoRepositorio lancamentoRepositorio, IProdutor produtor) 
    : BaseLancamentoHandler(notificador)
    , IRequestHandler<InserirLancamentoCommand, Resultado<Guid>>
{
    private readonly ILancamentoRepositorio _lancamentoRepositorio = lancamentoRepositorio;
    private readonly IProdutor _produtor = produtor;
    public async Task<Resultado<Guid>> Handle(InserirLancamentoCommand command, CancellationToken cancellationToken)
    {
        var lancamento = ObterLancamentoEntidade(command);
        if (!await EntidadeEhValidaAsync(new LancamentoValidador(true), lancamento))
            return Erro<Guid>();

        Console.WriteLine($"Inserindo lançamento: {lancamento.Id} - {lancamento.Data} - {lancamento.Valor} - {lancamento.TipoId} - {lancamento.CategoriaId}");
        lancamento = await _lancamentoRepositorio.InserirAsync(lancamento, cancellationToken);
        var evento = ObterEvento(lancamento);

        Console.WriteLine($"Publicando evento: {evento.Id} - {evento.Data} - {evento.Valor} - {evento.TipoId} - {evento.CategoriaId}");
        await _produtor.PublicarAsync(Constantes.RabbitMQ.QueueName, evento, cancellationToken);            

        return Ok(lancamento.Id);
    }

    private static LancamentoEvento ObterEvento(Lancamento lancamento) =>
        new(lancamento.Id, DateOnly.FromDateTime(lancamento.Data), lancamento.Valor, lancamento.TipoId, lancamento.CategoriaId, TipoEvento.Criacao);
}
