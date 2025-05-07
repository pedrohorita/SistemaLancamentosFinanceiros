using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Domain;

[ExcludeFromCodeCoverage]
public static class Constantes
{
    public static class SQLServer
    {
        public static readonly string ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new ArgumentException("CONNECTION_STRING");
    }

    public static class RabbitMQ
    {
        public static readonly string HostName = Environment.GetEnvironmentVariable("RABBIT_HOST_NAME") ?? throw new ArgumentException("RABBIT_HOST_NAME");
        public static readonly int Port = int.TryParse(Environment.GetEnvironmentVariable("RABBIT_PORT"), out var port) ? port : throw new ArgumentException("RABBIT_PORT");
        public static readonly string UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME") ?? throw new ArgumentException("RABBIT_USERNAME");
        public static readonly string Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD") ?? throw new ArgumentException("RABBIT_PASSWORD");
        public static readonly string QueueName = Environment.GetEnvironmentVariable("RABBIT_LANCAMENTO_QUEUE") ?? throw new ArgumentException("RABBIT_LANCAMENTO_QUEUE");
    }
}
