using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.ObterPaginado;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using Moq;

namespace ControleDeLancamentos.Tests.Comandos;

public class ObterLancamentosPaginadosQueryHandlerTests
{
    private readonly Mock<ILancamentoRepositorio> _lancamentoRepositorioMock;
    private readonly ObterLancamentosPaginadosQueryHandler _handler;
    public ObterLancamentosPaginadosQueryHandlerTests()
    {
        _lancamentoRepositorioMock = new();
        _handler = new ObterLancamentosPaginadosQueryHandler(_lancamentoRepositorioMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoLancamentosExistem_DeveRetornarLancamentoPaginado()
    {
        // Arrange
        var lancamentos = new List<Lancamento>
        {
            new(Guid.NewGuid(), "Descricao", 100, DateTime.Now, 1, 1, "user")
        };
        var query = new ObterLancamentosPaginadosQuery(1, 10);

        _lancamentoRepositorioMock.Setup(repo => repo.ObterPaginadoAsync(query.Pagina, query.Quantidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamentos);

        _lancamentoRepositorioMock.Setup(repo => repo.ObterQuantidadeAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamentos.Count);

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.True(resultado.Sucesso);
        Assert.NotNull(resultado.Dados);
        Assert.Equal(lancamentos.Count, resultado.Dados.Lancamentos.Count());
    }

    [Fact]
    public async Task Handle_QuandoLancamentosNaoExistem_DeveRetornarErro()
    {
        // Arrange
        var query = new ObterLancamentosPaginadosQuery(1, 10);
        _lancamentoRepositorioMock.Setup(repo => repo.ObterPaginadoAsync(query.Pagina, query.Quantidade, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Null(resultado.Dados);
        Assert.Contains("Nenhum lançamento encontrado.", resultado.Erros);
    }
}
