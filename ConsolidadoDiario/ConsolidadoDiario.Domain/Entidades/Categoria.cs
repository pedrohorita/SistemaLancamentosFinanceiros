using System.ComponentModel.DataAnnotations.Schema;

namespace ConsolidadoDiario.Domain.Entidades;

[Table("Categorias")]
public class Categoria : Base.Enum
{
    public Categoria(int id, string nome, string descricao, DateTime dataCriacao, string usuarioCriacao, 
        DateTime? dataAtualizacao = null, string? usuarioAtualizacao = null)
        : base(id, nome, descricao, dataCriacao, usuarioCriacao, dataAtualizacao, usuarioAtualizacao) { }
}
