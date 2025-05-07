using ControleDeLancamentos.Domain.Notificador;
using ControleDeLancamentos.Domain.Resultados;
using FluentValidation;
using FluentValidation.Results;

namespace ControleDeLancamentos.Application.Funcionalidades.Base;

public abstract class BaseHandler(INotificador notificador)
{
    private readonly INotificador _notificador = notificador;
    protected async Task<bool> EntidadeEhValidaAsync<T>(AbstractValidator<T> validador, T entidade)
    {
        var resultado = await validador.ValidateAsync(entidade);
        if (resultado.IsValid)
            return true;

        Notificar(resultado);
        return false;
    }
    protected IEnumerable<string> ObterNotificacoes() => _notificador.ObterNotificacoes();
    protected void Notificar(string mensagem) => _notificador.Notificar(mensagem);
    protected void Notificar(Notificacao notificacao) => _notificador.Notificar(notificacao);
    protected void Notificar(IEnumerable<string> mensagens)
    {
        foreach (var mensagem in mensagens)
            Notificar(mensagem);
    }
    protected void Notificar(IEnumerable<Notificacao> notificacoes)
    {
        foreach (var notificacao in notificacoes)
            Notificar(notificacao);
    }

    protected void Notificar(ValidationResult resultado)
    {
        foreach (var erro in resultado.Errors)
            Notificar(erro.ErrorMessage);
    }
    protected static Resultado Ok() => Resultado.Ok();
    protected Resultado Erro() => Resultado.Erro(ObterNotificacoes());

    protected Resultado Erro(string message)
    {
        Notificar(message);
        return Resultado.Erro(ObterNotificacoes());
    }
    protected Resultado Erro(IEnumerable<string> erros)
    {
        Notificar(erros);
        return Resultado.Erro(ObterNotificacoes());
    }


    protected static Resultado<T> Ok<T>(T data) => Resultado<T>.Ok(data);
    protected Resultado<T> Erro<T>() => Resultado<T>.Erro(ObterNotificacoes());
    protected Resultado<T> Erro<T>(string message)
    {
        Notificar(message);
        return Resultado<T>.Erro(ObterNotificacoes());
    }
    protected Resultado<T> Erro<T>(IEnumerable<string> erros)
    {
        Notificar(erros);
        return Resultado<T>.Erro(ObterNotificacoes());
    }

}
