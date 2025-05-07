using ConsolidadoDiario.Api.BackgroundServices;
using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ProcessarLancamento;
using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain.Enums;
using ConsolidadoDiario.Domain.Interfaces.Consumer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ConsolidadoDiario.Tests.BackgroundServiceTests;
public class WorkerServiceTests
{
    private readonly RabbitMQConsumerService _service;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IServiceScopeFactory> serviceScopeFactoryMock;
    private readonly Mock<IConnectionFactory> _connectionFactoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IConnection> _connectionMock;
    private readonly Mock<IChannel> _channelMock;

    public WorkerServiceTests()
    {
        _serviceProviderMock = new();
        _serviceScopeMock = new();
        serviceScopeFactoryMock = new();
        _service = new(_serviceProviderMock.Object);

        _mediatorMock = new();
        _connectionMock = new();
        _channelMock = new();
        _connectionFactoryMock = new();


        _connectionFactoryMock.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_connectionMock.Object);
        _connectionMock.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_channelMock.Object);

        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);

        serviceScopeFactoryMock
            .Setup(sf => sf.CreateScope())
            .Returns(_serviceScopeMock.Object);

        _serviceScopeMock
            .Setup(s => s.ServiceProvider)
            .Returns(_serviceProviderMock.Object);

        _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IMediator)))
            .Returns(_mediatorMock.Object);

    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallStartConsuming_WhenCancellationNotRequested()
    {
        // Arrange
        var stoppingTokenSource = new CancellationTokenSource();


        // Act
        var executeTask = _service.StartAsync(stoppingTokenSource.Token);


        await Task.Delay(200);

        stoppingTokenSource.Cancel();
        await executeTask;

        // Assert
        _mediatorMock.Verify(c => c.Send(It.IsAny<ProcessarLancamentoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }


    [Fact]
    public async Task StartConsuming_QuandoEventoValido_DeveProcessarEvento()
    {
        // Arrange
        var lancamentoEvento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 100, 1, 1, TipoEvento.Criacao);
        var message = JsonSerializer.Serialize(lancamentoEvento);
        var body = Encoding.UTF8.GetBytes(message);

        var basicDeliverEventArgs = new BasicDeliverEventArgs(
            consumerTag: "test-consumer",
            deliveryTag: 1,
            redelivered: false,
            exchange: "test-exchange",
            routingKey: "test-routing-key",
            properties: null,
            body: new ReadOnlyMemory<byte>(body),
            cancellationToken: CancellationToken.None
        );


        var consumer = new AsyncEventingBasicConsumer(Mock.Of<IChannel>());
        consumer.ReceivedAsync += async (sender, args) =>
            await _service.ProcessarEvento(_channelMock.Object, basicDeliverEventArgs);

        // Act
        await consumer.HandleBasicDeliverAsync(
            basicDeliverEventArgs.ConsumerTag,
            basicDeliverEventArgs.DeliveryTag,
            basicDeliverEventArgs.Redelivered,
            basicDeliverEventArgs.Exchange,
            basicDeliverEventArgs.RoutingKey,
            basicDeliverEventArgs.BasicProperties,
            basicDeliverEventArgs.Body
        );

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ProcessarLancamentoCommand>(), default), Times.Once);
    }

    [Fact]
    public async Task StartConsuming_QuandoEventoInvalido_DeveTratarErro()
    {
        // Arrange
        var invalidMessage = "Invalid JSON";
        var body = Encoding.UTF8.GetBytes(invalidMessage);

        var basicDeliverEventArgs = new BasicDeliverEventArgs(
            consumerTag: "test-consumer",
            deliveryTag: 1,
            redelivered: false,
            exchange: "test-exchange",
            routingKey: "test-routing-key",
            properties: null,
            body: new ReadOnlyMemory<byte>(body),
            cancellationToken: CancellationToken.None
        );

        var consumer = new AsyncEventingBasicConsumer(Mock.Of<IChannel>());

        // Subscribe to the ReceivedAsync event
        consumer.ReceivedAsync += async (sender, args) =>
        {
            // Act
            var exception = await Record.ExceptionAsync(() => Task.CompletedTask);

            // Assert
            Assert.Null(exception);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ProcessarLancamentoCommand>(), default), Times.Never);
        };

        // Trigger the event
        await consumer.HandleBasicDeliverAsync(
            basicDeliverEventArgs.ConsumerTag,
            basicDeliverEventArgs.DeliveryTag,
            basicDeliverEventArgs.Redelivered,
            basicDeliverEventArgs.Exchange,
            basicDeliverEventArgs.RoutingKey,
            basicDeliverEventArgs.BasicProperties,
            basicDeliverEventArgs.Body
        );
    }


    [Fact]
    public async Task StartConsuming_QuandoParseJsonFalhar_DeveTratarErro()
    {
        // Arrange
        var invalidMessage = "Invalid JSON";
        var body = Encoding.UTF8.GetBytes(invalidMessage);
        var basicDeliverEventArgs = new BasicDeliverEventArgs(
            consumerTag: "test-consumer",
            deliveryTag: 1,
            redelivered: false,
            exchange: "test-exchange",
            routingKey: "test-routing-key",
            properties: null,
            body: new ReadOnlyMemory<byte>(body),
            cancellationToken: CancellationToken.None
        );
        // Act
        var exception = await Record.ExceptionAsync(() => _service.ProcessarEvento(_channelMock.Object, basicDeliverEventArgs));
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task StartConsuming_QuandoFalharProcessamento_DeveTratarErro()
    {
        // Arrange
        var lancamentoEvento = new LancamentoEvento(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now), 100, 1, 1, TipoEvento.Criacao);
        var message = JsonSerializer.Serialize(lancamentoEvento);
        var body = Encoding.UTF8.GetBytes(message);
        var basicDeliverEventArgs = new BasicDeliverEventArgs(
            consumerTag: "test-consumer",
            deliveryTag: 1,
            redelivered: false,
            exchange: "test-exchange",
            routingKey: "test-routing-key",
            properties: null,
            body: new ReadOnlyMemory<byte>(body),
            cancellationToken: CancellationToken.None
        );
        _mediatorMock.Setup(m => m.Send(It.IsAny<ProcessarLancamentoCommand>(), default))
            .ThrowsAsync(new Exception("Erro ao processar evento"));
        // Act
        var exception = await Record.ExceptionAsync(() => _service.ProcessarEvento(_channelMock.Object, basicDeliverEventArgs));
        // Assert
        Assert.Null(exception);
    }
}
