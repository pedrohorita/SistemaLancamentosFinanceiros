using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;
using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Notificador;
using Moq;

namespace ControleDeLancamentos.Tests.Comandos;

public class InserirLancamentoCommandHandlerTests : BaseTests
{
    private readonly Mock<ILancamentoRepositorio> _lancamentoRepositorioMock;
    private readonly Mock<IProdutor> _produtorMock;
    private readonly INotificador _notificador;
    private readonly InserirLancamentoCommandHandler _handler;
    public InserirLancamentoCommandHandlerTests() : base()
    {
        _lancamentoRepositorioMock = new();
        _produtorMock = new();
        _notificador = new Domain.Notificador.Notificador();
        _handler = new InserirLancamentoCommandHandler(_notificador, _lancamentoRepositorioMock.Object, _produtorMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoRequisicaoEhValida_DeveInserirLancamento()
    {
        // Arrange
        var command = new InserirLancamentoCommand("Venda de Produto", 100.00m, 1, 1, "user");
        var lancamento = new Lancamento(command.Descricao, command.Valor, command.CategoriaId, command.TipoId, command.Usuario);

        _lancamentoRepositorioMock
            .Setup(x => x.InserirAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamento);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.Sucesso);
        Assert.True(result.Dados != Guid.Empty);

        _lancamentoRepositorioMock.Verify(x => x.InserirAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Once);
        _produtorMock.Verify(x => x.PublicarAsync(It.IsAny<string>(), It.IsAny<LancamentoEvento>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoRequisicaoEhInvalida_DeveRetornarErro()
    {
        // Arrange
        var command = new InserirLancamentoCommand("", 0, 0, 0, "user");
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result.Sucesso);
        Assert.Contains("A descrição é obrigatória.", result.Erros);
        Assert.Contains("O valor é obrigatório.", result.Erros);
        Assert.Contains("O valor deve ser maior que zero.", result.Erros);
        Assert.Contains("A categoria é obrigatória.", result.Erros);
        Assert.Contains("O tipo é obrigatório.", result.Erros);

        _lancamentoRepositorioMock.Verify(x => x.InserirAsync(It.IsAny<Lancamento>(), It.IsAny<CancellationToken>()), Times.Never);
        _produtorMock.Verify(x => x.PublicarAsync(It.IsAny<string>(), It.IsAny<LancamentoEvento>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
