using ControleDeLancamentos.Domain.Enums;

namespace ControleDeLancamentos.Application.Modelos;

public record LancamentoEvento(Guid Id, DateOnly Data, decimal Valor, int TipoId, int CategoriaId, TipoEvento TipoEvento);
