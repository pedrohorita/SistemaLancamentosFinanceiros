using ControleDeLancamentos.Api.Controllers;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ControleDeLancamentos.Tests.Controllers;

public class CategoriasControllerTests
{
    private readonly Mock<ICategoriaRepositorio> _categoriaRepositorioMock;
    private readonly CategoriasController _controller;
    public CategoriasControllerTests()
    {
        _categoriaRepositorioMock = new();
        _controller = new(_categoriaRepositorioMock.Object);
    }
    [Fact]
    public async Task ObterCategoriasAsync_QuandoCategoriasExistem_DeveRetornarOK()
    {
        // Arrange
        var categorias = new List<Categoria>
        {
            new(1, "Categoria1", "Categoria1", DateTime.Now, "user"),
            new(2, "Categoria2", "Categoria2", DateTime.Now, "user")
        };
        _categoriaRepositorioMock.Setup(repo => repo.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categorias);
        // Act
        var result = await _controller.ObterCategoriasAsync();
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategorias = Assert.IsAssignableFrom<IEnumerable<Categoria>>(okResult.Value);
        Assert.Equal(categorias.Count, returnedCategorias.Count());
    }
    [Fact]
    public async Task ObterCategoriasAsync_QuandoCategoriasNaoExistem_DeveRetornarNoContent()
    {
        // Arrange
        _categoriaRepositorioMock.Setup(repo => repo.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        // Act
        var result = await _controller.ObterCategoriasAsync();
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
