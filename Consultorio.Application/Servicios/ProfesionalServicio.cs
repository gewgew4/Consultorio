using Consultorio.Domain.Comun;
using Consultorio.Domain.Entidades;
using Consultorio.Domain.Errores;
using Consultorio.Domain.Repositorios;

namespace Consultorio.Application.Servicios;

public sealed class ProfesionalServicio(
    IProfesionalRepositorio profesionales,
    ITurnoRepositorio turnos)
{
    public async Task<Result> DeshabilitarAsync(int profesionalId, DateTime ahora)
    {
        var profesional = await profesionales.ObtenerPorIdAsync(profesionalId);
        if (profesional is null)
            return Result.Fallo(Errores.Profesional.NoEncontrado);

        if (await turnos.TieneTurnosConfirmadosFuturosAsync(profesionalId, ahora))
            return Result.Fallo(Errores.Profesional.TieneTurnosPendientes);

        profesional.Deshabilitar();
        return Result.Éxito();
    }

    public Task<IReadOnlyList<Profesional>> ListarAsync() => profesionales.ListarAsync();
}
