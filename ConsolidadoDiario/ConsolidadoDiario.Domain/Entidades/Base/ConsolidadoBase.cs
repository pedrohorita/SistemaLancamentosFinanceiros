using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsolidadoDiario.Domain.Entidades.Base;

public abstract class ConsolidadoBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateOnly Data { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalReceitas { get; set; } = 0.00m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDespesas { get; set; } = 0.00m;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Saldo { get; set; }

    [Required]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}
