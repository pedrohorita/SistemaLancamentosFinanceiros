using ConsolidadoDiario.Domain;
using ConsolidadoDiario.Domain.Interfaces.Respositorio;
using ConsolidadoDiario.Infrastructure.Repositorios;
using ConsolidadoDiario.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiario.Infrastructure;

[ExcludeFromCodeCoverage]
public static class Configuracao
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ConsolidadoDbContext>(options =>
            options.UseSqlServer(Constantes.SQLServer.ConnectionString));

        services.AddDbContext<ConsolidadoCategoriaDbContext>(options =>
            options.UseSqlServer(Constantes.SQLServer.ConnectionString));


        services.AddScoped<IConsolidadoDiarioRepositorio, ConsolidadoDiarioRepositorio>();
        services.AddScoped<IConsolidadoDiarioCategoriaRepositorio, ConsolidadoDiarioCategoriaRepositorio>();

        return services;
    }
}
