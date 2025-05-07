using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.InclusaoLancamento;

public record InclusaoLancamentoCommand(LancamentoEvento Evento) : IRequest<Resultado>;

