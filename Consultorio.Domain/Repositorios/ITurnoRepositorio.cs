using Consultorio.Domain.Entidades;

namespace Consultorio.Domain.Repositorios;

public interface ITurnoRepositorio
{
    Task AgregarAsync(Turno turno);
    Task<bool> ExisteSuperposiciónAsync(int profesionalId, DateTime inicio, DateTime fin);
    Task<bool> TieneTurnosConfirmadosFuturosAsync(int profesionalId, DateTime desde);
    Task<IReadOnlyList<Turno>> ListarPorProfesionalAsync(int profesionalId);
}
