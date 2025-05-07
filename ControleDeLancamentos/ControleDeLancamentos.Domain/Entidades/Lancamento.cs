using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeLancamentos.Domain.Entidades;

[Table("Lancamentos")]
public class Lancamento 
{
    [Key] 
    public Guid Id { get; set; }

    [Required] 
    [MaxLength(255)] 
    public string Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")] 
    public decimal Valor { get; set; }

    [Required]
    public DateTime Data { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public Categoria Categoria { get; set; }

    [Required]
    public int TipoId { get; set; } 

    [ForeignKey("TipoId")]
    public Tipo Tipo { get; set; }
    [Required]
    [MaxLength(100)]
    public string Usuario { get; set; }

    public Lancamento(string descricao, decimal valor,int categoriaId, int tipoId, string usuario)
    {
        Id = Guid.NewGuid();
        Descricao = descricao;
        Valor = valor;
        Data = DateTime.UtcNow;
        CategoriaId = categoriaId;
        TipoId = tipoId;
        Usuario = usuario;

        Categoria = null!;
        Tipo = null!;
    }

    public Lancamento(Guid id, string descricao, decimal valor, DateTime data, int categoriaId, int tipoId, string usuario)
    {
        Id = id;
        Descricao = descricao;
        Valor = valor;
        Data = data;
        CategoriaId = categoriaId;
        TipoId = tipoId;
        Usuario = usuario;

        Categoria = null!;
        Tipo = null!;
    }
}
