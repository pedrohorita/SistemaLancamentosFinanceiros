namespace ControleDeLancamentos.Tests;

public class BaseTests
{
    protected BaseTests()
    {
        Environment.SetEnvironmentVariable("RABBIT_HOST_NAME", "localhost");
        Environment.SetEnvironmentVariable("RABBIT_PORT", "5672");
        Environment.SetEnvironmentVariable("RABBIT_USERNAME", "guest");
        Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "guest");
        Environment.SetEnvironmentVariable("RABBIT_LANCAMENTO_QUEUE", "lancamentos");
    }
}
