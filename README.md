# Ejercicio 2: Validaciones de Servicio, DI de Menús y Consola Segura

## Contexto

La solución `Consultorio` administra profesionales y turnos en memoria. El dominio ya está resuelto, pero faltan validaciones clave en la capa de aplicación, el registro en DI de servicios y menús, y la protección global de excepciones en la consola.

## Qué tenés que resolver

1. **Reglas de negocio en Aplicación** (`Consultorio.Application/Servicios`):
   - En `TurnoServicio.ReservarAsync`:
     - Verificá que el profesional exista (`Errores.Profesional.NoEncontrado`) y esté activo (`Errores.Profesional.Inactivo`).
     - Verificá que no exista superposición horaria llamando a `ExisteSuperposiciónAsync` (`Errores.Turno.Superpuesto`).
   - En `ProfesionalServicio.DeshabilitarAsync`:
     - Impedí deshabilitar al profesional si tiene turnos confirmados futuros usando `TieneTurnosConfirmadosFuturosAsync` (`Errores.Profesional.TieneTurnosPendientes`).

2. **Registro en DI** (`Consultorio.Console/Program.cs`):
   - Registrá `ProfesionalServicio`, `TurnoServicio`, `MenuProfesionales` y `MenuTurnos` en `builder.Services` para que las opciones 1 y 2 del menú funcionen sin errores.

3. **Manejo global de excepciones en Consola** (`Consultorio.Console/Program.cs`):
   - Envolvé el cuerpo del `while (true)` en un único `try/catch (Exception ex)` que registre el error con `logger.LogError(...)` y muestre un mensaje amigable al usuario sin cerrar la aplicación (verificable con la opción 3 del menú).

## Cómo verificar

```powershell
dotnet test
dotnet run --project Consultorio.Console
```
