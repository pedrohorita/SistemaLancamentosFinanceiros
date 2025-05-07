using ConsolidadoDiario.Domain.Entidades.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsolidadoDiario.Domain.Entidades;

[Table("ConsolidadoDiarios")]
public class ConsolidadoDiario : ConsolidadoBase
{
}
