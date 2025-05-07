using ControleDeLancamentos.Application.Funcionalidades.Base;
using ControleDeLancamentos.Domain.Resultados;
using MediatR;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;

public record InserirLancamentoCommand(string Descricao, decimal Valor, int TipoId, int CategoriaId, string Usuario) 
    : BaseLancamentoCommand(Descricao, Valor, TipoId, CategoriaId, Usuario)
    , IRequest<Resultado<Guid>>;
