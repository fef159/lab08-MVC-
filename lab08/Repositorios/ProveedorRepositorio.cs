using System.Data;
using Dapper;
using lab08.Models;
using Microsoft.Data.SqlClient;

namespace lab08.Repositorios;

public class ProveedorRepositorio : IProveedorRepositorio
{
    private readonly string _connectionString;

    public ProveedorRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NeptunoDB")
            ?? throw new InvalidOperationException("No se configuró la conexión NeptunoDB.");
    }

    public async Task<IEnumerable<Proveedor>> ListarAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Proveedor>(
            "usp_Lab08_Proveedor_Listar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Proveedor proveedor)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "usp_Lab08_Proveedor_Insertar",
            new
            {
                proveedor.CompaniaNombre,
                proveedor.NombreContacto,
                proveedor.CargoContacto,
                proveedor.Direccion,
                proveedor.Ciudad,
                proveedor.CodigoPostal,
                proveedor.Pais,
                proveedor.Telefono
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExisteCompaniaAsync(string companiaNombre)
    {
        await using var connection = new SqlConnection(_connectionString);
        var cantidad = await connection.ExecuteScalarAsync<int>(
            "usp_Lab08_Proveedor_ExisteCompania",
            new { CompaniaNombre = companiaNombre },
            commandType: CommandType.StoredProcedure);
        return cantidad > 0;
    }
}
