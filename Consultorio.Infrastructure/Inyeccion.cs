using Consultorio.Domain.Repositorios;
using Consultorio.Infrastructure.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace Consultorio.Infrastructure;

public static class Inyeccion
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services)
    {
        services.AddSingleton<IProfesionalRepositorio, MemoriaProfesionalRepositorio>();
        services.AddSingleton<ITurnoRepositorio, MemoriaTurnoRepositorio>();
        return services;
    }
}
