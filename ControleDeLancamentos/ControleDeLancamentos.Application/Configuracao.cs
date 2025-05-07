using ControleDeLancamentos.Application.Funcionalidades.Lancamentos.Inserir;
using ControleDeLancamentos.Domain.Notificador;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Application;

[ExcludeFromCodeCoverage]
public static class Configuracao
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INotificador, Notificador>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<InserirLancamentoCommand>());
        return services;
    }
}
