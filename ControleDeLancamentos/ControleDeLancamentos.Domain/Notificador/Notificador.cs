using System.Collections.Concurrent;

namespace ControleDeLancamentos.Domain.Notificador;

public class Notificador : INotificador
{
    private readonly object _lock = new();
    private readonly ConcurrentQueue<Notificacao> _notificacoes = [];
    public void Notificar(string mensagem)
    {
        _notificacoes.Enqueue(new Notificacao(mensagem));
    }
    public void Notificar(Notificacao notificacao)
    {
        _notificacoes.Enqueue(notificacao);
    }
    public bool TemNotificacoes()
    {
        return !_notificacoes.IsEmpty;
    }

    public bool TemNotificacoes<T>()
    {
        return _notificacoes.Any(n => n is T);
    }
    public string[] ObterNotificacoes()
    {
        lock(_lock)
        {
            return [.._notificacoes.Select(n => n.Mensagem)];
        }
    }
}
