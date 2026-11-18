using Consultorio.Application.Servicios;

namespace Consultorio.Console.Menus;

public sealed class MenuProfesionales(ProfesionalServicio servicio)
{
    public async Task EjecutarAsync()
    {
        var lista = await servicio.ListarAsync();
        foreach (var p in lista)
            System.Console.WriteLine($"[{p.Id}] {p.Nombre} ({p.Matrícula}) - {(p.Activo ? "Activo" : "Inactivo")}");

        System.Console.Write("ID de profesional a deshabilitar (Enter para volver): ");
        var entrada = System.Console.ReadLine()?.Trim();
        if (!int.TryParse(entrada, out var id))
            return;

        var res = await servicio.DeshabilitarAsync(id, DateTime.Now);
        System.Console.WriteLine(res.EsÉxito
            ? "Profesional deshabilitado."
            : $"Error [{res.Error.Código}]: {res.Error.Mensaje}");
    }
}
