using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.InclusaoLancamento;
using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Entidades;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using ConsolidadoDiario.Infrastructure.SqlServer;
using Moq;

namespace ConsolidadoDiario.Tests.Commands;

public class InclusaoLancamentoCommandHandlerTests
{
    private readonly InclusaoLancamentoCommandHandler _handler;
    private readonly Mock<IConsolidadoDiarioRepositorio> _consolidadoDiarioRepositorioMock;
    private readonly Mock<IConsolidadoDiarioCategoriaRepositorio> _consolidadoDiarioCategoriaRepositorioMock;
    public InclusaoLancamentoCommandHandlerTests()
    {
        _consolidadoDiarioRepositorioMock = new();
        _consolidadoDiarioCategoriaRepositorioMock = new();
        _handler = new InclusaoLancamentoCommandHandler(_consolidadoDiarioRepositorioMock.Object, _consolidadoDiarioCategoriaRepositorioMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoEventoEhPrimeiro_DeveProcessarInclusaoConsolidado()
    {
        // Arrange  
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow), 100, (int)TipoLancamento.Entrada, 1, TipoEvento.Criacao);
        var command = new InclusaoLancamentoCommand(evento);
        // Act  
        var resultado = await _handler.Handle(command, CancellationToken.None);
        // Assert  
        Assert.True(resultado.Sucesso);
        _consolidadoDiarioRepositorioMock.Verify(x => x.InserirAsync(It.IsAny<Domain.Entidades.ConsolidadoDiario>(), It.IsAny<CancellationToken>()), Times.Once);
        _consolidadoDiarioCategoriaRepositorioMock.Verify(x => x.InserirAsync(It.IsAny<ConsolidadoDiarioCategoria>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoEventoEhAtualizacao_DeveProcessarAtualizacaoConsolidado()
    {
        // Arrange  
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow), 100, (int)TipoLancamento.Entrada, 1, TipoEvento.Atualizacao);
        var command = new InclusaoLancamentoCommand(evento);
        var consolidadoDiario = new Domain.Entidades.ConsolidadoDiario
        {
            Data = evento.Data,
            TotalReceitas = 0,
            TotalDespesas = 0,
            Saldo = 0
        };
        var consolidadoDiarioCategoria = new ConsolidadoDiarioCategoria
        {
            Data = evento.Data,
            CategoriaId = evento.CategoriaId,
            TotalReceitas = 0,
            TotalDespesas = 0,
            Saldo = 0
        };

        _consolidadoDiarioRepositorioMock.Setup(x => x.ObterConsolidadoDiarioPorDataAsync(evento.Data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiario);

        _consolidadoDiarioCategoriaRepositorioMock.Setup(x => x.ObterConsolidadoPorCategoria(evento.CategoriaId, evento.Data, It.IsAny<CancellationToken>()))
            .ReturnsAsync(consolidadoDiarioCategoria);

        // Act  
        var resultado = await _handler.Handle(command, CancellationToken.None);
        // Assert  
        Assert.True(resultado.Sucesso);
        _consolidadoDiarioRepositorioMock.Verify(x => x.AtualizarAsync(It.IsAny<Domain.Entidades.ConsolidadoDiario>(), It.IsAny<CancellationToken>()), Times.Once);
        _consolidadoDiarioCategoriaRepositorioMock.Verify(x => x.AtualizarAsync(It.IsAny<ConsolidadoDiarioCategoria>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
