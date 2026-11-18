using System.Globalization;
using System.Text;
using Consultorio.Application.Servicios;
using Consultorio.Console.Menus;
using Consultorio.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Consultorio.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        System.Console.OutputEncoding = Encoding.UTF8;
        var cultura = new CultureInfo("es-AR");
        CultureInfo.CurrentCulture = cultura;
        CultureInfo.CurrentUICulture = cultura;

        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        });

        builder.Logging.ClearProviders();
        builder.Services.AddSerilog((_, cfg) => cfg.ReadFrom.Configuration(builder.Configuration));

        builder.Services.AgregarInfraestructura();

        // TODO (Punto 2): Registrar ProfesionalServicio, TurnoServicio, MenuProfesionales y MenuTurnos en DI.

        using var host = builder.Build();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        // TODO (Punto 3): Proteger el loop con un único try/catch que registre el error con logger
        // y muestre un mensaje amigable sin cerrar la aplicación.
        while (true)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("=== CONSULTORIO ===");
            System.Console.WriteLine("1. Profesionales");
            System.Console.WriteLine("2. Turnos");
            System.Console.WriteLine("3. Simular falla técnica");
            System.Console.WriteLine("0. Salir");
            System.Console.Write("Opción: ");
            var opcion = System.Console.ReadLine()?.Trim();

            if (opcion == "0")
                return;

            using var alcance = host.Services.CreateScope();
            var proveedor = alcance.ServiceProvider;

            switch (opcion)
            {
                case "1":
                    await proveedor.GetRequiredService<MenuProfesionales>().EjecutarAsync();
                    break;
                case "2":
                    await proveedor.GetRequiredService<MenuTurnos>().EjecutarAsync();
                    break;
                case "3":
                    throw new InvalidOperationException("Falla técnica simulada.");
                default:
                    System.Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }
}
