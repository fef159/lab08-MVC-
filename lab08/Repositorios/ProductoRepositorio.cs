using System.Data;
using Dapper;
using lab08.Models;
using Microsoft.Data.SqlClient;

namespace lab08.Repositorios;

public class ProductoRepositorio : IProductoRepositorio
{
    private readonly string _connectionString;

    public ProductoRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NeptunoDB")
            ?? throw new InvalidOperationException("No se configuró la conexión NeptunoDB.");
    }

    public async Task<IEnumerable<Producto>> ListarAsync(string? busqueda)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Producto>(
            "usp_Lab08_Producto_Listar",
            new { Busqueda = busqueda },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Producto>(
            "usp_Lab08_Producto_ObtenerPorId",
            new { ProductoID = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Producto producto)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Lab08_Producto_Insertar",
            Parametros(producto),
            commandType: CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(Producto producto)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Lab08_Producto_Actualizar",
            new
            {
                producto.ProductoID,
                producto.NombreProducto,
                producto.ProveedorID,
                producto.CategoriaID,
                producto.CantidadPorUnidad,
                producto.PrecioUnidad,
                producto.UnidadesEnExistencia,
                producto.UnidadesEnPedido,
                producto.NivelDeReorden,
                producto.Descontinuado
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Lab08_Producto_Eliminar",
            new { ProductoID = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Categoria>> ListarCategoriasAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Categoria>(
            "usp_Lab08_Categoria_ListarActivas",
            commandType: CommandType.StoredProcedure);
    }

    private static object Parametros(Producto producto) => new
    {
        producto.NombreProducto,
        producto.ProveedorID,
        producto.CategoriaID,
        producto.CantidadPorUnidad,
        producto.PrecioUnidad,
        producto.UnidadesEnExistencia,
        producto.UnidadesEnPedido,
        producto.NivelDeReorden,
        producto.Descontinuado
    };
}
