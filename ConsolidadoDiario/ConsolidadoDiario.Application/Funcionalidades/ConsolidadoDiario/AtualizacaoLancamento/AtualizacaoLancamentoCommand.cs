using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.AtualizacaoLancamento;

public record AtualizacaoLancamentoCommand(LancamentoEvento Evento) : IRequest<Resultado>;
