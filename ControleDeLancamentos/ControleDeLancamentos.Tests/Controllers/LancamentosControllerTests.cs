using ControleDeLancamentos.Api.Controllers;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Atualizar;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Excluir;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;
using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.ObterPaginado;
using ControleDeLancamentos.Application.Modelos;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Domain.Resultados;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ControleDeLancamentos.Tests.Controllers;

public class LancamentosControllerTests
{
    private readonly LancamentosController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILancamentoRepositorio> _lancamentoRepositorioMock;
    public LancamentosControllerTests()
    {
        _mediatorMock = new();
        _lancamentoRepositorioMock = new();
        _controller = new(_mediatorMock.Object, _lancamentoRepositorioMock.Object);
    }
    [Fact]
    public async Task ObterLancamentosAsync_QuandoLancamentosExistem_DeveRetornarOk()
    {
        // Arrange
        var lancamentos = new List<Lancamento>
        {
            new(Guid.NewGuid(), "Descricao", 100, DateTime.Now, 1, 1, "user")
        };
        var lancamentosPaginados = new LancamentoPaginado()
        {
            Lancamentos = lancamentos,
            TotalRegistros = 1,
            PaginaAtual = 1,
            TotalPaginas = 1,
            TamanhoPagina = 1
        };
        var query = new ObterLancamentosPaginadosQuery(1, 10);
        var resultado = Resultado<LancamentoPaginado>.Ok(lancamentosPaginados);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);
        // Act
        var result = await _controller.ObterLancamentosAsync(1, 10);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(lancamentosPaginados, okResult.Value);
    }
    [Fact]
    public async Task ObterLancamentosAsync_QuandoLancamentosNaoExistem_DeveRetornarNoContent()
    {
        // Arrange
        var query = new ObterLancamentosPaginadosQuery(1, 10);
        var resultado = Resultado<LancamentoPaginado>.Erro(["Nenhum lançamento encontrado."]);

        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);
        // Act
        var result = await _controller.ObterLancamentosAsync(1, 10);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ObterLancamentoPorIdAsync_QuandoLancamentoExistir_DeveRetornarOk()
    {
        // Arrange
        var lancamento = new Lancamento(Guid.NewGuid(), "Descricao", 100, DateTime.Now, 1, 1, "user");
        _lancamentoRepositorioMock.Setup(repo => repo.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lancamento);
        // Act
        var result = await _controller.ObterLancamentoPorIdAsync(lancamento.Id, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(lancamento, okResult.Value);
    }

    [Fact]
    public async Task ObterLancamentoPorIdAsync_QuandoLancamentoNaoExistir_DeveRetornarNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        _lancamentoRepositorioMock.Setup(repo => repo.ObterPorIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Lancamento)null);
        // Act
        var result = await _controller.ObterLancamentoPorIdAsync(id, CancellationToken.None);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task InserirLancamentoAsync_QuandoLancamentoInserido_DeveRetornarCreated()
    {
        // Arrange
        var comando = new InserirLancamentoCommand("Descricao", 100, 1, 1, "user");
        var resultado = Resultado<Guid>.Ok(Guid.NewGuid());
        _mediatorMock.Setup(m => m.Send(comando, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);
        // Act
        var result = await _controller.InserirLancamentoAsync(comando, CancellationToken.None);
        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
    }

    [Fact]
    public async Task InserirLancamentoAsync_QuandoLancamentoNaoInserido_DeveRetornarBadRequest()
    {
        // Arrange
        var comando = new InserirLancamentoCommand("Descricao", 100, 1, 1, "user");
        var resultado = Resultado<Guid>.Erro(["Erro ao inserir lançamento."]);
        _mediatorMock.Setup(m => m.Send(comando, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);
        // Act
        var result = await _controller.InserirLancamentoAsync(comando, CancellationToken.None);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task AtualizarLancamentoAsync_QuandoLancamentoAtualizado_DeveRetornarOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var comando = new AtualizarLancamentoCommand(id, "Descricao Atualizada", 150, 1, 1, "user");
        var resultado = Resultado.Ok();
        _mediatorMock.Setup(m => m.Send(comando, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);
        // Act
        var result = await _controller.AtualizarLancamentoAsync(id, comando, CancellationToken.None);
        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AtualizarLancamentoAsync_QuandoLancamentoNaoAtualizado_DeveRetornarBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var comando = new AtualizarLancamentoCommand(id, "Descricao Atualizada", 150, 1, 1, "user");
        var resultado = Resultado.Erro(["Erro ao atualizar lançamento."]);
        _mediatorMock.Setup(m => m.Send(comando, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        // Act
        var result = await _controller.AtualizarLancamentoAsync(id, comando, CancellationToken.None);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task ExcluirLancamentoAsync_QuandoLancamentoExcluido_DeveRetornarOk()
    {
        // Arrange
        var id = Guid.NewGuid();
        var resultado = Resultado.Ok();
        var comando = new ExcluirLancamentoCommand(id);

        _mediatorMock.Setup(m => m.Send(comando, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        // Act
        var result = await _controller.ExcluirLancamentoAsync(id, CancellationToken.None);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task ExcluirLancamentoAsync_QuandoLancamentoNaoExiste_DeveRetornarNoContent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var resultado = Resultado.Erro(["Erro ao excluir lançamento."]);
        var comando = new ExcluirLancamentoCommand(id);
        _mediatorMock.Setup(m => m.Send(comando, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultado);

        // Act
        var result = await _controller.ExcluirLancamentoAsync(id, CancellationToken.None);
        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }

}
