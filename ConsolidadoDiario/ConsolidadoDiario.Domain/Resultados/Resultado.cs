namespace ConsolidadoDiario.Domain.Resultados;

public class Resultado
{
    protected Resultado(bool sucesso, IEnumerable<string> erros)
    {
        Sucesso = sucesso;
        Erros = erros;
    }
    public bool Sucesso { get; private set; }
    public IEnumerable<string> Erros { get; private set; }

    public static Resultado Ok() => new(true, []);      

    public static Resultado Erro(IEnumerable<string> erros) => new(false, erros);

}
