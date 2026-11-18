using Consultorio.Domain.Comun;
using Consultorio.Domain.Entidades;
using Consultorio.Domain.Errores;
using Consultorio.Domain.Repositorios;

namespace Consultorio.Application.Servicios;

public sealed class TurnoServicio(
    ITurnoRepositorio turnos,
    IProfesionalRepositorio profesionales)
{
    public async Task<Result<Turno>> ReservarAsync(
        int profesionalId,
        string pacienteDni,
        string pacienteNombre,
        DateTime inicio,
        int duraciónMinutos)
    {
        var profesional = await profesionales.ObtenerPorIdAsync(profesionalId);
        if (profesional is null)
            return Result.Fallo<Turno>(Errores.Profesional.NoEncontrado);

        if (!profesional.Activo)
            return Result.Fallo<Turno>(Errores.Profesional.Inactivo);

        var resultado = Turno.Crear(profesionalId, pacienteDni, pacienteNombre, inicio, duraciónMinutos);
        if (resultado.EsFallo)
            return resultado;

        var turno = resultado.Valor;
        if (await turnos.ExisteSuperposiciónAsync(profesionalId, turno.Inicio, turno.Fin))
            return Result.Fallo<Turno>(Errores.Turno.Superpuesto);

        await turnos.AgregarAsync(turno);
        return resultado;
    }

    public Task<IReadOnlyList<Turno>> ListarPorProfesionalAsync(int profesionalId) =>
        turnos.ListarPorProfesionalAsync(profesionalId);
}
