using Consultorio.Domain.Comun;
using Consultorio.Domain.Enums;

namespace Consultorio.Domain.Entidades;

public class Turno
{
    private Turno() { }

    private Turno(
        int profesionalId,
        string pacienteDni,
        string pacienteNombre,
        DateTime inicio,
        DateTime fin)
    {
        Id = Guid.NewGuid();
        ProfesionalId = profesionalId;
        PacienteDni = pacienteDni;
        PacienteNombre = pacienteNombre;
        Inicio = inicio;
        Fin = fin;
        Estado = EstadoTurno.Confirmado;
    }

    public Guid Id { get; private set; }
    public int ProfesionalId { get; private set; }
    public string PacienteDni { get; private set; } = string.Empty;
    public string PacienteNombre { get; private set; } = string.Empty;
    public DateTime Inicio { get; private set; }
    public DateTime Fin { get; private set; }
    public EstadoTurno Estado { get; private set; }

    public static Result<Turno> Crear(
        int profesionalId,
        string pacienteDni,
        string pacienteNombre,
        DateTime inicio,
        int duraciónMinutos)
    {
        if (string.IsNullOrWhiteSpace(pacienteDni) || string.IsNullOrWhiteSpace(pacienteNombre))
            return Result.Fallo<Turno>(Errores.Errores.Turno.PacienteRequerido);

        if (duraciónMinutos is not (30 or 60))
            return Result.Fallo<Turno>(Errores.Errores.Turno.DuraciónInválida);

        return Result.Éxito(new Turno(
            profesionalId,
            pacienteDni.Trim(),
            pacienteNombre.Trim(),
            inicio,
            inicio.AddMinutes(duraciónMinutos)));
    }

    public void Cancelar() => Estado = EstadoTurno.Cancelado;
}
