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
        // TODO (Punto 1): Validar que el profesional exista (Profesional.NoEncontrado) y esté activo (Profesional.Inactivo).
        // TODO (Punto 1): Validar que no exista superposición horaria con ExisteSuperposiciónAsync (Turno.Superpuesto).
        var resultado = Turno.Crear(profesionalId, pacienteDni, pacienteNombre, inicio, duraciónMinutos);
        if (resultado.EsFallo)
            return resultado;

        await turnos.AgregarAsync(resultado.Valor);
        return resultado;
    }

    public Task<IReadOnlyList<Turno>> ListarPorProfesionalAsync(int profesionalId) =>
        turnos.ListarPorProfesionalAsync(profesionalId);
}
