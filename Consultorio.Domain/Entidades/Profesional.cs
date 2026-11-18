namespace Consultorio.Domain.Entidades;

public class Profesional
{
    public Profesional(int id, string matrícula, string nombre, bool activo = true)
    {
        Id = id;
        Matrícula = matrícula;
        Nombre = nombre;
        Activo = activo;
    }

    public int Id { get; private set; }
    public string Matrícula { get; private set; }
    public string Nombre { get; private set; }
    public bool Activo { get; private set; }

    public void Deshabilitar() => Activo = false;
}
