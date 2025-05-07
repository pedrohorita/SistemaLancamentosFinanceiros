namespace ControleDeLancamentos.Domain.Resultados;

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

public class Resultado<T> : Resultado
{
    protected Resultado(bool sucesso, T? dados, IEnumerable<string> erros) : base(sucesso, erros)
    {
        Dados = dados;
    }

    public T? Dados { get; private set; }

    public static Resultado<T> Ok(T dado) => new(true, dado, []);

    public static new Resultado<T> Erro(IEnumerable<string> erros) => new(false, default, erros);
}
