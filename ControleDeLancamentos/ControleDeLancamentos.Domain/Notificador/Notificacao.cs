namespace ControleDeLancamentos.Domain.Notificador;

public class Notificacao(string mensagem)
{
    public string Mensagem { get; } = mensagem;
}
