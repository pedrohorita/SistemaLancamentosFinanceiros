using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeLancamentos.Domain.Entidades;

[Table("Tipos")]
public class Tipo(int id, string nome, string descricao, DateTime dataCriacao, string usuarioCriacao,
    DateTime? dataAtualizacao = null, string? usuarioAtualizacao = null) 
    : Enum(id, nome, descricao, dataCriacao, usuarioCriacao, dataAtualizacao, usuarioAtualizacao)
{
}
