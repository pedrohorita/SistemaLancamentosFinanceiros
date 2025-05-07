using ConsolidadoDiario.Domain.Entidades.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ConsolidadoDiario.Domain.Entidades;

[Table("ConsolidadoDiarioCategorias")]
public class ConsolidadoDiarioCategoria : ConsolidadoBase
{
    [Required]
    
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public virtual Categoria Categoria { get; set; } 
}