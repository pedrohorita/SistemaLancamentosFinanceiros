using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Excluir;
using ControleDeLancamentos.Domain.Interfaces;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Notificador;
using Moq;

namespace ControleDeLancamentos.Tests.Comandos;

public class ExcluirLancamentoCommandHandlerTests : BaseTests
{
    private readonly Mock<ILancamentoRepositorio> _lancamentoRepositorioMock;
    private readonly Mock<IProdutor> _produtorMock;

    private readonly ExcluirLancamentoCommandHandler _handler;
    public ExcluirLancamentoCommandHandlerTests()
    {
        _lancamentoRepositorioMock = new();
        _produtorMock = new();
        _handler = new(_lancamentoRepositorioMock.Object, _produtorMock.Object);
    }
    [Fact]
    public async Task Handle_ExcluirLancamentoComando_ExcluiLancamento()
    {
        // Arrange
        var lancamentoId = Guid.NewGuid();
        var comando = new ExcluirLancamentoCommand(lancamentoId);
        var lancamento = new Domain.Entidades.Lancamento(lancamentoId, "Descricao", 100, DateTime.Now, 1, 1, "user");
        _lancamentoRepositorioMock.Setup(x => x.ObterPorIdAsync(lancamentoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamento);
        // Act
        var resultado = await _handler.Handle(comando, CancellationToken.None);
        // Assert
        Assert.True(resultado.Sucesso);
        _lancamentoRepositorioMock.Verify(x => x.ExcluirAsync(lancamento, It.IsAny<CancellationToken>()), Times.Once);
        _produtorMock.Verify(x => x.PublicarAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_LancamentoNaoEncontrado_DeveRetornarErro()
    {
        // Arrange
        var lancamentoId = Guid.NewGuid();
        var comando = new ExcluirLancamentoCommand(lancamentoId);
        _lancamentoRepositorioMock.Setup(x => x.ObterPorIdAsync(lancamentoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entidades.Lancamento)null);
        // Act
        var resultado = await _handler.Handle(comando, CancellationToken.None);
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("Lançamento não encontrado.", resultado.Erros);
    }
}
