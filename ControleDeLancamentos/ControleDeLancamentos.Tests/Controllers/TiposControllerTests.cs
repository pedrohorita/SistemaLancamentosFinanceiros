using ControleDeLancamentos.Api.Controllers;
using ControleDeLancamentos.Domain.Entidades;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ControleDeLancamentos.Tests.Controllers;

public class TiposControllerTests
{
    private readonly Mock<ITipoRepositorio> _tipoRepositorioMock;
    private readonly TiposController _controller;
    public TiposControllerTests()
    {
        _tipoRepositorioMock = new();
        _controller = new(_tipoRepositorioMock.Object);
    }
    [Fact]
    public async Task ObterTiposAsync_QuandoTiposExistem_DeveRetornarOK()
    {
        // Arrange
        var tipos = new List<Tipo>
        {
            new(1, "Tipo1", "Tipo1", DateTime.Now, "user"),
            new(2, "Tipo2", "Tipo2", DateTime.Now, "user")
        };
        _tipoRepositorioMock.Setup(repo => repo.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tipos);
        // Act
        var result = await _controller.ObterTiposAsync();
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTipos = Assert.IsAssignableFrom<IEnumerable<Tipo>>(okResult.Value);
        Assert.Equal(tipos.Count, returnedTipos.Count());
    }
    [Fact]
    public async Task ObterTiposAsync_QuandoTiposNaoExistem_DeveRetornarNoContent()
    {
        // Arrange
        _tipoRepositorioMock.Setup(repo => repo.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        // Act
        var result = await _controller.ObterTiposAsync();
        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
