using ControleDeLancamentos.Domain.Entidades;

namespace ControleDeLancamentos.Application.Modelos;

public class LancamentoPaginado
{
    public int TotalRegistros { get; set; } = 0;
    public int TotalPaginas { get; set; } = 0;
    public int PaginaAtual { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
    public IEnumerable<Lancamento> Lancamentos { get; set; } = [];
}
