using ControleDeLancamentos.Domain.Resultados;
using MediatR;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Excluir;

public record ExcluirLancamentoCommand(Guid Id) : IRequest<Resultado>;
