namespace ControleDeLancamentos.Domain.Interfaces;

public interface IProdutor
{
    Task PublicarAsync<T>(string queueName, T message, CancellationToken cancellationToken);
}
