using Consultorio.Domain.Comun;

namespace Consultorio.Domain.Errores;

public static class Errores
{
    public static class Profesional
    {
        public static readonly Error NoEncontrado = new("Profesional.NoEncontrado", "No se encontró el profesional.");
        public static readonly Error Inactivo = new("Profesional.Inactivo", "El profesional está deshabilitado.");
        public static readonly Error TieneTurnosPendientes = new("Profesional.TieneTurnosPendientes", "No se puede deshabilitar un profesional con turnos confirmados pendientes.");
    }

    public static class Turno
    {
        public static readonly Error PacienteRequerido = new("Turno.PacienteRequerido", "El DNI y nombre del paciente son obligatorios.");
        public static readonly Error DuraciónInválida = new("Turno.DuraciónInválida", "La duración del turno debe ser de 30 o 60 minutos.");
        public static readonly Error Superpuesto = new("Turno.Superpuesto", "El profesional ya tiene un turno confirmado en ese horario.");
    }
}
