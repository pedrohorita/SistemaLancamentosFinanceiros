using ControleDeLancamentos.Domain;
using ControleDeLancamentos.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ControleDeLancamentos.Infrastructure.RabbitMQ;
public class RabbitMqService() : IProdutor
{
    private readonly string _hostName = Constantes.RabbitMQ.HostName;
    private readonly int _port = Constantes.RabbitMQ.Port;
    private readonly string _userName = Constantes.RabbitMQ.UserName;
    private readonly string _password = Constantes.RabbitMQ.Password;

    private async Task<IConnection> CriarConexaoAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password,
            Port = _port,
        };

        return await factory.CreateConnectionAsync(cancellationToken);
    }

    public async Task PublicarAsync<T>(string queueName, T message, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Criando conexão com o RabbitMQ...");
            using var connection = await CriarConexaoAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            Console.WriteLine($"Conexão criada com sucesso. Publicando mensagem na fila {queueName}...");
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, 
                autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            Console.WriteLine($"Mensagem serializada: {jsonMessage}");
            var properties = new BasicProperties()
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            Console.WriteLine($"Publicando mensagem na fila {queueName}...");

            await channel.BasicPublishAsync(exchange: string.Empty,
                            routingKey: queueName,
                            mandatory: true,
                            basicProperties: properties,
                            body: body,
                            cancellationToken: cancellationToken);

            Console.WriteLine($"Mensagem publicada na fila {queueName} com sucesso.");
        }
        catch (Exception)
        {
            Console.WriteLine("Não foi possível publicar o evento");
        }
    }
}

