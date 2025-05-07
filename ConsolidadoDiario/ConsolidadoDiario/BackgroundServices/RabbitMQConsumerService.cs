using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.ProcessarLancamento;
using ConsolidadoDiario.Application.Modelos;
using ConsolidadoDiario.Domain;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace ConsolidadoDiario.Api.BackgroundServices;

public class RabbitMQConsumerService : BackgroundService
{
    private readonly IServiceProvider _provider;
    private IConnection? _connection;
    private IChannel? _channel; 

    public RabbitMQConsumerService(IServiceProvider provider)
    {
        _provider = provider;
        try
        {
            InitializeRabbitMQ();
        }
        catch (BrokerUnreachableException ex)
        {
            Console.WriteLine($"RabbitMQ connection failed: {ex.Message}");
        }
    }

    private void InitializeRabbitMQ()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = Constantes.RabbitMQ.HostName,
                UserName = Constantes.RabbitMQ.UserName,
                Password = Constantes.RabbitMQ.Password,
                Port = Constantes.RabbitMQ.Port
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.QueueDeclareAsync(
                queue: Constantes.RabbitMQ.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null)
            .GetAwaiter();

            Console.WriteLine("RabbitMQ inicializado com sucesso. Conectado a {0}:{1}, Fila: {2}",
                Constantes.RabbitMQ.HostName, Constantes.RabbitMQ.Port, Constantes.RabbitMQ.QueueName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao inicializar o RabbitMQ: {0}", ex.Message);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        if (_channel == null)
        {
            Console.WriteLine("Canal RabbitMQ não inicializado.");
            return;
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea)
            => await ProcessarEvento(_channel, ea);

        await _channel.BasicConsumeAsync(queue: Constantes.RabbitMQ.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    public async Task ProcessarEvento(IChannel channel, BasicDeliverEventArgs ea)
    {
        try
        {
            Console.WriteLine($"Mensagem recebida: {ea.RoutingKey}");
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            if (JsonSerializer.Deserialize<LancamentoEvento>(message) is { } evento)
            {
                Console.WriteLine($"Mensagem deserializada: {evento}");
                using var scope = _provider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new ProcessarLancamentoCommand(evento));
                Console.WriteLine($"Mensagem processada: {evento}");

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            else
            {
                Console.WriteLine("Falha ao deserializar o LancamentoEvento.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
        }
    }

    public override void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter();

        _connection?.CloseAsync().GetAwaiter();

        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
