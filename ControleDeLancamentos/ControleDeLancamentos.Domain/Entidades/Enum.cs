using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeLancamentos.Domain.Entidades;

public abstract class Enum(int id, string nome, string descricao, DateTime dataCriacao, string usuarioCriacao, DateTime? dataAtualizacao, string? usuarioAtualizacao)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } = id;

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = nome;

    [MaxLength(255)]
    public string Descricao { get; set; } = descricao;

    [Required]
    public DateTime DataCriacao { get; set; } = dataCriacao;

    public DateTime? DataAtualizacao { get; set; } = dataAtualizacao;

    [Required]
    [MaxLength(100)]
    public string UsuarioCriacao { get; set; } = usuarioCriacao;

    [MaxLength(100)]
    public string? UsuarioAtualizacao { get; set; } = usuarioAtualizacao;
}
