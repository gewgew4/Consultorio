using Consultorio.Domain.Entidades;
using Consultorio.Domain.Enums;
using Consultorio.Domain.Repositorios;

namespace Consultorio.Infrastructure.Repositorios;

public sealed class MemoriaProfesionalRepositorio : IProfesionalRepositorio
{
    readonly List<Profesional> _profesionales =
    [
        new(1, "MN-10420", "Dra. Laura Gómez"),
        new(2, "MN-20815", "Dr. Martín Pereyra")
    ];

    public Task<Profesional?> ObtenerPorIdAsync(int id) =>
        Task.FromResult(_profesionales.FirstOrDefault(p => p.Id == id));

    public Task<IReadOnlyList<Profesional>> ListarAsync() =>
        Task.FromResult<IReadOnlyList<Profesional>>(_profesionales.ToList());
}

public sealed class MemoriaTurnoRepositorio : ITurnoRepositorio
{
    readonly List<Turno> _turnos = [];

    public Task AgregarAsync(Turno turno)
    {
        _turnos.Add(turno);
        return Task.CompletedTask;
    }

    public Task<bool> ExisteSuperposiciónAsync(int profesionalId, DateTime inicio, DateTime fin) =>
        Task.FromResult(_turnos.Any(t =>
            t.ProfesionalId == profesionalId &&
            t.Estado == EstadoTurno.Confirmado &&
            inicio < t.Fin &&
            fin > t.Inicio));

    public Task<bool> TieneTurnosConfirmadosFuturosAsync(int profesionalId, DateTime desde) =>
        Task.FromResult(_turnos.Any(t =>
            t.ProfesionalId == profesionalId &&
            t.Estado == EstadoTurno.Confirmado &&
            t.Inicio >= desde));

    public Task<IReadOnlyList<Turno>> ListarPorProfesionalAsync(int profesionalId) =>
        Task.FromResult<IReadOnlyList<Turno>>(_turnos
            .Where(t => t.ProfesionalId == profesionalId)
            .OrderBy(t => t.Inicio)
            .ToList());
}
