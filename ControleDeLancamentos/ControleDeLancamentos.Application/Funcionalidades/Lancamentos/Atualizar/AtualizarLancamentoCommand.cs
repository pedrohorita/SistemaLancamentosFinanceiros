using ControleDeLancamentos.Application.Funcionalidades.Base;
using ControleDeLancamentos.Domain.Resultados;
using MediatR;
using System.Text.Json.Serialization;

namespace ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Atualizar;

public record AtualizarLancamentoCommand(Guid Id, string Descricao, decimal Valor, int TipoId, int CategoriaId, string Usuario) 
    : BaseLancamentoCommand(Descricao, Valor, TipoId, CategoriaId, Usuario), IRequest<Resultado>
{
    [JsonIgnore]
    public Guid Id { get; set; } = Id;
}
