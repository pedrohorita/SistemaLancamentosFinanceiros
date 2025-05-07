using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain.Resultados;
using MediatR;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.ObterPaginado;

public record ObterLancamentosPaginadosQuery(int Pagina, int Quantidade) : IRequest<Resultado<LancamentoPaginado>>;
