using ConsolidadoDiario.Application.Funcionalidades.ConsolidadoDiario.InclusaoLancamento;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Application;

[ExcludeFromCodeCoverage]
public static class Configuracao
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<InclusaoLancamentoCommand>());
        return services;
    }
}
