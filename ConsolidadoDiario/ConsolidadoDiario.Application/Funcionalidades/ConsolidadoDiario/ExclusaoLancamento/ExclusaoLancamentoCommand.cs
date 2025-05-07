using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ExclusaoLancamento;

public record ExclusaoLancamentoCommand(LancamentoEvento Evento) : IRequest<Resultado>;