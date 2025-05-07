using ControleDeLancamentos.Domain;
using ControleDeLancamentos.Domain.Interfaces;
using ControleDeLancamentos.Domain.Interfaces.Repositorios;
using ControleDeLancamentos.Infrastructure.RabbitMQ;
using ControleDeLancamentos.Infrastructure.Repositorios;
using ControleDeLancamentos.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ControleDeLancamentos.Infrastructure;

[ExcludeFromCodeCoverage]
public static class Configuracao
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Constantes.SQLServer.ConnectionString));

        services.AddSingleton<IProdutor, RabbitMqService>();

        services.AddScoped<ILancamentoRepositorio, LancamentoRepositorio>();
        services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
        services.AddScoped<ITipoRepositorio, TipoRepositorio>();


        return services;
    }
}
