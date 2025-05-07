using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Atualizar;
using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Notificador;
using Moq;

namespace ControleDeLancamentos.Tests.Comandos;

public class AtualizarLancamentoCommandHandlerTests : BaseTests
{
    private readonly Mock<ILancamentoRepositorio> _lancamentoRepositorioMock;
    private readonly Mock<IProdutor> _produtorMock;
    private readonly INotificador _notificador;
    private readonly AtualizarLancamentoCommandHandler _handler;
    public AtualizarLancamentoCommandHandlerTests()
    {
        _lancamentoRepositorioMock = new Mock<ILancamentoRepositorio>();
        _produtorMock = new();
        _notificador = new Domain.Notificador.Notificador();
        _handler = new AtualizarLancamentoCommandHandler(_notificador, _lancamentoRepositorioMock.Object, _produtorMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoNaoEncontrado_DeveNotificarErro()
    {
        // Arrange
        var command = new AtualizarLancamentoCommand(Guid.NewGuid(), "Descricao", 100, 1, 1, "user");
        _lancamentoRepositorioMock.Setup(repo => repo.ObterPorIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Lancamento)null);


        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("Lançamento não encontrado.", resultado.Erros);

        _lancamentoRepositorioMock.Verify(repo => repo.AtualizarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Never);
        _produtorMock.Verify(x => x.PublicarAsync(It.IsAny<string>(), It.IsAny<LancamentoEvento>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoExistir_DeveAtualizarLancamento()
    {
        // Arrange
        var lancamentoExistente = new Lancamento(Guid.NewGuid(), "Descricao", 100, DateTime.Now, 1, 1, "user");
        _lancamentoRepositorioMock.Setup(repo => repo.ObterPorIdAsync(lancamentoExistente.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamentoExistente);
        _lancamentoRepositorioMock.Setup(repo => repo.AtualizarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var command = new AtualizarLancamentoCommand(lancamentoExistente.Id, "Nova Descricao", 200, 2, 2, "user");
        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(resultado.Sucesso);

        _lancamentoRepositorioMock.Verify(x => x.AtualizarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtorMock.Verify(x => x.PublicarAsync(It.IsAny<string>(), It.IsAny<LancamentoEvento>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoNaoForAtualizado_NaoDeveRealizarAtualizacao()
    {
        // Arrange
        var lancamentoExistente = new Lancamento(Guid.NewGuid(), "Descricao", 100, DateTime.Now, 1, 1, "user")
        {
            Categoria = new(1, "Categoria", "Descricao", DateTime.Now, "user"),
            Tipo = new(1, "Tipo", "Descricao", DateTime.Now, "user")
        };
        _lancamentoRepositorioMock.Setup(repo => repo.ObterPorIdAsync(lancamentoExistente.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamentoExistente);

        var command = new AtualizarLancamentoCommand(lancamentoExistente.Id, "Descricao", 100, 1, 1, "user");
        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(resultado.Sucesso);

        _lancamentoRepositorioMock.Verify(x => x.AtualizarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Never);
        _produtorMock.Verify(x => x.PublicarAsync(It.IsAny<string>(), It.IsAny<LancamentoEvento>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoNaoEhValido_DeveRetornarErro()
    {
        // Arrange

        var command = new AtualizarLancamentoCommand(Guid.NewGuid(), "Descricao", 0, 1, 1, "user");
        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);
        // Assert   
        Assert.False(resultado.Sucesso);
        Assert.Contains("O valor é obrigatório.", resultado.Erros);
        Assert.Contains("O valor deve ser maior que zero.", resultado.Erros);

        _lancamentoRepositorioMock.Verify(x => x.AtualizarAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
