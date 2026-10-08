# Laboratorio 08 - Neptuno MVC con Dapper

Aplicación ASP.NET Core MVC que adapta la guía de la semana 08 a la base de datos existente `NeptunoDB`. Usa Dapper, `Microsoft.Data.SqlClient`, procedimientos almacenados, repositorios e inyección de dependencias.

## Preparación

1. Verificar que SQL Server esté iniciado y que exista `NeptunoDB` con las tablas `Productos`, `Proveedores`, `Categorias`, `Pedidos`, `DetallePedidos` y `Clientes`.
2. Ejecutar `lab08/Script-NeptunoDB.sql` en SQL Server para crear o actualizar los procedimientos almacenados del laboratorio.
3. Si SQL Server no usa autenticación integrada, ajustar solamente `ConnectionStrings:NeptunoDB` en `lab08/appsettings.json`.

## Ejecución

```powershell
dotnet restore .\lab08\lab08.csproj
dotnet run --project .\lab08\lab08.csproj
```

## Recorrido de una petición

Ejemplo: `GET /Productos?busqueda=café`.

1. La ruta convencional dirige la petición a `ProductosController.Index`.
2. El controlador recibe el texto de búsqueda, lo conserva en `ViewData["Busqueda"]` y llama a `IProductoRepositorio.ListarAsync`.
3. `ProductoRepositorio` abre una conexión a partir de `appsettings.json` y ejecuta `usp_Lab08_Producto_Listar` con Dapper (`QueryAsync`) y `CommandType.StoredProcedure`.
4. El procedimiento filtra los productos activos y une `Productos`, `Proveedores` y `Categorias`.
5. El controlador entrega la colección como modelo fuertemente tipado a `Views/Productos/Index.cshtml`.
6. La vista reutiliza `_ProductoRow.cshtml` para dibujar cada resultado. El modelo transporta la lista porque representa el contenido principal; `ViewData` conserva el filtro, y `TempData` se reserva para mensajes que deben sobrevivir a una redirección después de crear, editar o eliminar.

## Observaciones y conclusiones

- Los controladores no contienen SQL ni crean conexiones.
- Las operaciones de datos son asíncronas de punta a punta.
- La eliminación de productos es lógica (`Activo = 0`).
- Los formularios aplican DataAnnotations, validación del servidor, antiforgery y el patrón Post/Redirect/Get.
- El reporte usa un filtro GET para que el intervalo de fechas permanezca visible y pueda compartirse en la URL.
