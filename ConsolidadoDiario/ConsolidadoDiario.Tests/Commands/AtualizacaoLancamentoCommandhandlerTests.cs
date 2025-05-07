using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.AtualizacaoLancamento;
using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Entidades;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using Moq;

namespace ConsolidadoDiario.Tests.Commands;

public class AtualizacaoLancamentoCommandhandlerTests
{
    private readonly Mock<IConsolidadoDiarioRepositorio> _consolidadoDiarioRepositorioMock;
    private readonly Mock<IConsolidadoDiarioCategoriaRepositorio> _consolidadoDiarioCategoriaRepositorioMock;
    private readonly AtualizacaoLancamentoCommandhandler _handler;
    public AtualizacaoLancamentoCommandhandlerTests()
    {
        _consolidadoDiarioRepositorioMock = new Mock<IConsolidadoDiarioRepositorio>();
        _consolidadoDiarioCategoriaRepositorioMock = new Mock<IConsolidadoDiarioCategoriaRepositorio>();
        _handler = new AtualizacaoLancamentoCommandhandler(_consolidadoDiarioRepositorioMock.Object,
            _consolidadoDiarioCategoriaRepositorioMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoEventoEhAtualizacao_DeveProcessarAtualizacaoConsolidado()
    {
        // Arrange  
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.UtcNow), 100, (int)TipoLancamento.Entrada, 1, TipoEvento.Atualizacao);
        var command = new AtualizacaoLancamentoCommand(evento);
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
    }
}
