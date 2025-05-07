using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Resultados;
using MediatR;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.ObterPaginado;

public class ObterLancamentosPaginadosQueryHandler(ILancamentoRepositorio lancamentoRepositorio) 
    : IRequestHandler<ObterLancamentosPaginadosQuery, Resultado<LancamentoPaginado>>
{
    private readonly ILancamentoRepositorio _lancamentoRepositorio = lancamentoRepositorio;
    public async Task<Resultado<LancamentoPaginado>> Handle(ObterLancamentosPaginadosQuery request, CancellationToken cancellationToken)
    {
        if (cancellationToken == CancellationToken.None)
            cancellationToken = new CancellationTokenSource(1000).Token;


        var lancamentos = await _lancamentoRepositorio.ObterPaginadoAsync(request.Pagina, request.Quantidade, cancellationToken);
        if (!lancamentos.Any())
            return Resultado<LancamentoPaginado>.Erro(["Nenhum lançamento encontrado."]);

        var total = await _lancamentoRepositorio.ObterQuantidadeAsync(cancellationToken);
        var quantidadePaginas = (int)Math.Ceiling((double)total / request.Quantidade);

        var lancamentoPaginado = new LancamentoPaginado
        {
            Lancamentos = lancamentos,
            PaginaAtual = request.Pagina,
            TamanhoPagina = lancamentos.Count(),
            TotalPaginas = quantidadePaginas,
            TotalRegistros = total
        };

        return Resultado<LancamentoPaginado>.Ok(lancamentoPaginado);
    }
}
