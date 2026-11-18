using Consultorio.Application.Servicios;

namespace Consultorio.Console.Menus;

public sealed class MenuTurnos(TurnoServicio servicio)
{
    public async Task EjecutarAsync()
    {
        var inicio = DateTime.Now.AddHours(2);
        var res = await servicio.ReservarAsync(1, "35123456", "Ana Pérez", inicio, 30);
        System.Console.WriteLine(res.EsÉxito
            ? $"Turno confirmado: {res.Valor.Inicio:HH:mm}-{res.Valor.Fin:HH:mm}"
            : $"Error [{res.Error.Código}]: {res.Error.Mensaje}");
    }
}
