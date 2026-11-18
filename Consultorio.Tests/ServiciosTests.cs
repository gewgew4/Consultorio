using Consultorio.Application.Servicios;
using Consultorio.Domain.Entidades;
using Consultorio.Domain.Enums;
using Consultorio.Domain.Errores;
using Consultorio.Domain.Repositorios;

namespace Consultorio.Tests;

public class ServiciosTests
{
    sealed class RepoProfesionalesFalso(params Profesional[] iniciales) : IProfesionalRepositorio
    {
        readonly List<Profesional> _items = [.. iniciales];
        public Task<Profesional?> ObtenerPorIdAsync(int id) => Task.FromResult(_items.FirstOrDefault(p => p.Id == id));
        public Task<IReadOnlyList<Profesional>> ListarAsync() => Task.FromResult<IReadOnlyList<Profesional>>(_items);
    }

    sealed class RepoTurnosFalso : ITurnoRepositorio
    {
        readonly List<Turno> _items = [];
        public Task AgregarAsync(Turno turno) { _items.Add(turno); return Task.CompletedTask; }
        public Task<bool> ExisteSuperposiciónAsync(int profesionalId, DateTime inicio, DateTime fin) =>
            Task.FromResult(_items.Any(t =>
                t.ProfesionalId == profesionalId &&
                t.Estado == EstadoTurno.Confirmado &&
                inicio < t.Fin &&
                fin > t.Inicio));
        public Task<bool> TieneTurnosConfirmadosFuturosAsync(int profesionalId, DateTime desde) =>
            Task.FromResult(_items.Any(t =>
                t.ProfesionalId == profesionalId &&
                t.Estado == EstadoTurno.Confirmado &&
                t.Inicio >= desde));
        public Task<IReadOnlyList<Turno>> ListarPorProfesionalAsync(int profesionalId) =>
            Task.FromResult<IReadOnlyList<Turno>>(_items.Where(t => t.ProfesionalId == profesionalId).ToList());
    }

    [Fact]
    public async Task ReservarAsync_ProfesionalInexistente_DevuelveProfesionalNoEncontrado()
    {
        var servicio = new TurnoServicio(new RepoTurnosFalso(), new RepoProfesionalesFalso());

        var res = await servicio.ReservarAsync(99, "35123456", "Ana Pérez", DateTime.Today.AddHours(10), 30);

        Assert.True(res.EsFallo);
        Assert.Equal(Errores.Profesional.NoEncontrado, res.Error);
    }

    [Fact]
    public async Task ReservarAsync_ProfesionalInactivo_DevuelveProfesionalInactivo()
    {
        var prof = new Profesional(1, "MN-1", "Dra. Gómez", activo: false);
        var servicio = new TurnoServicio(new RepoTurnosFalso(), new RepoProfesionalesFalso(prof));

        var res = await servicio.ReservarAsync(1, "35123456", "Ana Pérez", DateTime.Today.AddHours(10), 30);

        Assert.True(res.EsFallo);
        Assert.Equal(Errores.Profesional.Inactivo, res.Error);
    }

    [Fact]
    public async Task ReservarAsync_HorarioSuperpuesto_DevuelveTurnoSuperpuesto()
    {
        var prof = new Profesional(1, "MN-1", "Dra. Gómez");
        var servicio = new TurnoServicio(new RepoTurnosFalso(), new RepoProfesionalesFalso(prof));
        var inicio = new DateTime(2026, 11, 20, 10, 0, 0);

        var primero = await servicio.ReservarAsync(1, "35123456", "Ana Pérez", inicio, 60);
        var segundo = await servicio.ReservarAsync(1, "36987654", "Juan López", inicio.AddMinutes(30), 30);

        Assert.True(primero.EsÉxito);
        Assert.True(segundo.EsFallo);
        Assert.Equal(Errores.Turno.Superpuesto, segundo.Error);
    }

    [Fact]
    public async Task DeshabilitarAsync_ConTurnosPendientes_DevuelveTieneTurnosPendientes()
    {
        var prof = new Profesional(1, "MN-1", "Dra. Gómez");
        var repoTurnos = new RepoTurnosFalso();
        var repoProf = new RepoProfesionalesFalso(prof);
        var turnoServicio = new TurnoServicio(repoTurnos, repoProf);
        var profServicio = new ProfesionalServicio(repoProf, repoTurnos);

        var ahora = new DateTime(2026, 11, 20, 9, 0, 0);
        await turnoServicio.ReservarAsync(1, "35123456", "Ana Pérez", ahora.AddHours(2), 30);

        var res = await profServicio.DeshabilitarAsync(1, ahora);

        Assert.True(res.EsFallo);
        Assert.Equal(Errores.Profesional.TieneTurnosPendientes, res.Error);
        Assert.True(prof.Activo);
    }
}
