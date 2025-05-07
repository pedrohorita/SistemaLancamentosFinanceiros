namespace ControleDeLancamentos.Application.Funcionalidades.Base;

public record BaseLancamentoCommand(string Descricao, decimal Valor, int TipoId, int CategoriaId, string Usuario);
