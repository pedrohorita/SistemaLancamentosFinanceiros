using ControleDeLancamentos.Domain.Notificador;

namespace ControleDeLancamentos.Tests.Notificador;

public class NotificadorTests
{
    private readonly Domain.Notificador.Notificador _notificador;

    public NotificadorTests()
    {
        _notificador = new();
    }

    [Fact]
    public void AdicionarErro_QuandoChamado_DeveAdicionarErroNaLista()
    {
        // Arrange
        var mensagem = "Erro de teste";
        // Act
        _notificador.Notificar(mensagem);
        // Assert
        Assert.True(_notificador.TemNotificacoes());
        Assert.Contains(mensagem, _notificador.ObterNotificacoes());
    }

    [Fact]
    public void ObterNotificacoes_QuandoChamado_DeveRetornarListaDeNotificacoes()
    {
        // Arrange
        var mensagem1 = "Erro de teste 1";
        var mensagem2 = "Erro de teste 2";
        _notificador.Notificar(mensagem1);
        _notificador.Notificar(mensagem2);
        // Act
        var notificacoes = _notificador.ObterNotificacoes();
        // Assert
        Assert.Equal(2, notificacoes.Length);
        Assert.Contains(mensagem1, notificacoes);
        Assert.Contains(mensagem2, notificacoes);
    }

    [Fact]
    public void AdicionarNotificacao_QuandoChamado_DeveAdicionarErroNaLista()
    {
        // Arrange
        var mensagem = "Erro de teste";
        var notificacao = new Notificacao(mensagem);
        // Act
        _notificador.Notificar(notificacao);
        // Assert
        Assert.True(_notificador.TemNotificacoes<Notificacao>());
        Assert.Contains(mensagem, _notificador.ObterNotificacoes());
    }
}
