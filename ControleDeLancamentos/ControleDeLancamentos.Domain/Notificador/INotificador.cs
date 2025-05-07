
namespace ControleDeLancamentos.Domain.Notificador;

public interface INotificador
{
    void Notificar(string mensagem);
    void Notificar(Notificacao notificacao);
    string[] ObterNotificacoes();
    bool TemNotificacoes();
    bool TemNotificacoes<T>();
}
