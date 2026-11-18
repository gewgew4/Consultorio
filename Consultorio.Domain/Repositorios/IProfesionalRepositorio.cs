using Consultorio.Domain.Entidades;

namespace Consultorio.Domain.Repositorios;

public interface IProfesionalRepositorio
{
    Task<Profesional?> ObtenerPorIdAsync(int id);
    Task<IReadOnlyList<Profesional>> ListarAsync();
}
