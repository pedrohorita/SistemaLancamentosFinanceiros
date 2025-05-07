using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.AtualizacaoLancamento;
using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ExclusaoLancamento;
using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.InclusaoLancamento;
using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ProcessarLancamento;
using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Resultados;
using MediatR;
using Moq;

namespace ConsolidadoDiario.Tests.Commands;

public class ProcessarLancamentoCommandHandlerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProcessarLancamentoCommandHandler _handler;
    public ProcessarLancamentoCommandHandlerTests()
    {
        _mediatorMock = new();
        _handler = new ProcessarLancamentoCommandHandler(_mediatorMock.Object);
    }
    [Fact]
    public async Task Handle_QuandoLancamentoTipoCriacao_DeveProcessarLancamento()
    {
        // Arrange
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 100, 1, 1, TipoEvento.Criacao);
        var command = new ProcessarLancamentoCommand(evento);

        _mediatorMock.Setup(m => m.Send(It.IsAny<InclusaoLancamentoCommand>(), CancellationToken.None))
            .ReturnsAsync(Resultado.Ok());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.Sucesso);
        Assert.Empty(result.Erros);

        _mediatorMock.Verify(m => m.Send(It.IsAny<InclusaoLancamentoCommand>(), CancellationToken.None), Times.Once);
    }
    
    [Fact]
    public async Task Handle_QuandoLancamentoEhExclusao_DeveProcessarLancamento()
    {
        // Arrange
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 100, 1, 1, TipoEvento.Exclusao);
        var command = new ProcessarLancamentoCommand(evento);
        _mediatorMock.Setup(m => m.Send(It.IsAny<ExclusaoLancamentoCommand>(), CancellationToken.None))
            .ReturnsAsync(Resultado.Ok());
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.Sucesso);
        Assert.Empty(result.Erros);

        _mediatorMock.Verify(m => m.Send(It.IsAny<ExclusaoLancamentoCommand>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_QuandoLancamentoEhAtualizacao_DeveProcessarLancamento()
    {
        // Arrange
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 100, 1, 1, TipoEvento.Atualizacao);
        var command = new ProcessarLancamentoCommand(evento);
        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizacaoLancamentoCommand>(), CancellationToken.None))
            .ReturnsAsync(Resultado.Ok());
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.Sucesso);
        Assert.Empty(result.Erros);

        _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizacaoLancamentoCommand>(), CancellationToken.None), Times.Once);
    }


    [Fact]
    public async Task Handle_QuandoProcessamentoLancaExcecao_DeveRetornarOk()
    {
        // Arrange
        var evento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 100, 1, 1, TipoEvento.Criacao);
        var command = new ProcessarLancamentoCommand(evento);
        _mediatorMock.Setup(m => m.Send(It.IsAny<InclusaoLancamentoCommand>(), CancellationToken.None)).Throws(new Exception());
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.False(result.Sucesso);
        Assert.NotEmpty(result.Erros);
        _mediatorMock.Verify(m => m.Send(It.IsAny<InclusaoLancamentoCommand>(), CancellationToken.None), Times.Once);
    }

}
