using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;

namespace ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ProcessarLancamento
{
    public record ProcessarLancamentoCommand(LancamentoEvento evento) : IRequest<Resultado>
    {
    }
}
