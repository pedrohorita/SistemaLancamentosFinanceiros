using ConsolidadoDiario.Api.Controllers;
using ConsolidadoDiario.Domain.Entidades;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ConsolidadoDiario.Tests.Controllers;

public class ConsolidadoControllerTests
{
    private readonly Mock<IConsolidadoDiarioRepositorio> _consolidadoDiarioRepositorioMock;
    private readonly Mock<IConsolidadoDiarioCategoriaRepositorio> _consolidadoDiarioCategoriaRepositorioMock;
    private readonly ConsolidadoController _controller;
    public ConsolidadoControllerTests()
    {
        _consolidadoDiarioRepositorioMock = new Mock<IConsolidadoDiarioRepositorio>();
        _consolidadoDiarioCategoriaRepositorioMock = new Mock<IConsolidadoDiarioCategoriaRepositorio>();
        _controller = new ConsolidadoController(_consolidadoDiarioRepositorioMock.Object, _consolidadoDiarioCategoriaRepositorioMock.Object);
    }

    [Fact]
    public async Task ObterConsolidadoDiarioPorDataAsync_ConsolidadoDiarioExistente_RetornaOk()
    {
        // Arrange
        var data = new DateOnly(2023, 10, 1);
        var consolidadoDiario = new Domain.Entidades.ConsolidadoDiario 
        {
            Id = 1,
            Data = data,  
        };
        _consolidadoDiarioRepositorioMock.Setup(x => x.ObterConsolidadoDiarioPorDataAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiario);
        // Act
        var result = await _controller.ObterConsolidadoDiarioPorDataAsync(data, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(consolidadoDiario, okResult.Value);
    }

    [Fact]
    public async Task ObterConsolidadoDiarioPorDataAsync_ConsolidadoDiarioNaoExistente_RetornaNoContent()
    {
        // Arrange
        var data = new DateOnly(2023, 10, 1);
        _consolidadoDiarioRepositorioMock.Setup(x => x.ObterConsolidadoDiarioPorDataAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entidades.ConsolidadoDiario)null);
        // Act
        var result = await _controller.ObterConsolidadoDiarioPorDataAsync(data, CancellationToken.None);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ObterConsolidadosDiariosAsync_ConsolidadosExistentes_RetornaOk()
    {
        // Arrange
        var dataInicio = new DateOnly(2023, 10, 1);
        var dataFim = new DateOnly(2023, 10, 31);
        var consolidadoDiarios = new List<Domain.Entidades.ConsolidadoDiario>
        {
            new() { Data = dataInicio },
            new() { Data = dataFim }
        };
        _consolidadoDiarioRepositorioMock.Setup(x => x.ObterConsolidadosDiariosAsync(dataInicio, dataFim, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiarios);
        // Act
        var result = await _controller.ObterConsolidadosDiariosAsync(dataInicio, dataFim, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(consolidadoDiarios, okResult.Value);
    }

    [Fact]
    public async Task ObterConsolidadosDiariosAsync_ConsolidadosNaoExistentes_RetornaNoContent()
    {
        // Arrange
        var dataInicio = new DateOnly(2023, 10, 1);
        var dataFim = new DateOnly(2023, 10, 31);
        _consolidadoDiarioRepositorioMock.Setup(x => x.ObterConsolidadosDiariosAsync(dataInicio, dataFim, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        // Act
        var result = await _controller.ObterConsolidadosDiariosAsync(dataInicio, dataFim, CancellationToken.None);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ObterConsolidadoDiarioCategoria_ConsolidadoDiarioCategoriaExistente_RetornaOk()
    {
        // Arrange
        var data = new DateOnly(2023, 10, 1);
        var consolidadoDiarioCategorias = new List<Domain.Entidades.ConsolidadoDiarioCategoria>
        {
            new() { Data = data }
        };
        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadoDiarioCategoriaAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiarioCategorias);
        // Act
        var result = await _controller.ObterConsolidadoDiarioCategoria(data, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(consolidadoDiarioCategorias, okResult.Value);
    }

    [Fact]
    public async Task ObterConsolidadoDiarioCategoria_ConsolidadoDiarioCategoriaNaoExistente_RetornaNoContent()
    {
        // Arrange
        var data = new DateOnly(2023, 10, 1);
        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadoDiarioCategoriaAsync(data, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Domain.Entidades.ConsolidadoDiarioCategoria>)null);
        // Act
        var result = await _controller.ObterConsolidadoDiarioCategoria(data, CancellationToken.None);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ObterConsolidadoPorCategoria_ConsolidadoDiarioCategoriaExistente_RetornaOk()
    {
        // Arrange
        var categoriaId = 1;
        var data = new DateOnly(2023, 10, 1);
        var consolidadoDiarioCategoria = new ConsolidadoDiarioCategoria 
        { 
            CategoriaId = categoriaId, 
            Categoria = new Categoria(categoriaId, "teste", "teste", DateTime.Now, "user"),
            Data = data 
        };
        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadoPorCategoria(categoriaId, data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiarioCategoria);
        // Act
        var result = await _controller.ObterConsolidadoPorCategoria(categoriaId, data, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(consolidadoDiarioCategoria, okResult.Value);
    }

    [Fact]
    public async Task ObterConsolidadoPorCategoria_ConsolidadoDiarioCategoriaNaoExistente_RetornaNoContent()
    {
        // Arrange
        var categoriaId = 1;
        var data = new DateOnly(2023, 10, 1);
        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadoPorCategoria(categoriaId, data, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entidades.ConsolidadoDiarioCategoria)null);
        // Act
        var result = await _controller.ObterConsolidadoPorCategoria(categoriaId, data, CancellationToken.None);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ObterConsolidadosDiariosPorCategoriaAsync_ConsolidadosDiariosExistentes_RetornaOk()
    {
        // Arrange
        var categoriaId = 1;
        var dataInicio = new DateOnly(2023, 10, 1);
        var dataFim = new DateOnly(2023, 10, 31);
        var consolidadoDiarioCategorias = new List<Domain.Entidades.ConsolidadoDiarioCategoria>
        {
            new() { CategoriaId = categoriaId, Data = dataInicio },
            new() { CategoriaId = categoriaId, Data = dataFim }
        };
        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadosDiariosPorCategoriaAsync(categoriaId, dataInicio, dataFim, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiarioCategorias);
        // Act
        var result = await _controller.ObterConsolidadosDiariosPorCategoria(categoriaId, dataInicio, dataFim, CancellationToken.None);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(consolidadoDiarioCategorias, okResult.Value);
    }

    [Fact]
    public async Task ObterConsolidadosDiariosPorCategoriaAsync_ConsolidadosDiariosNaoExistentes_RetornaNoContent()
    {
        // Arrange
        var categoriaId = 1;
        var dataInicio = new DateOnly(2023, 10, 1);
        var dataFim = new DateOnly(2023, 10, 31);
        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadosDiariosPorCategoriaAsync(categoriaId, dataInicio, dataFim, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Domain.Entidades.ConsolidadoDiarioCategoria>)null);
        // Act
        var result = await _controller.ObterConsolidadosDiariosPorCategoria(categoriaId, dataInicio, dataFim, CancellationToken.None);
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
