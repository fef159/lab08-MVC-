using System.Data;
using Dapper;
using lab08.Models;
using Microsoft.Data.SqlClient;

namespace lab08.Repositorios;

public class PedidoRepositorio : IPedidoRepositorio
{
    private readonly string _connectionString;

    public PedidoRepositorio(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NeptunoDB")
            ?? throw new InvalidOperationException("No se configuró la conexión NeptunoDB.");
    }

    public async Task<IEnumerable<PedidoReporte>> ReporteAsync(DateTime desde, DateTime hasta)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<PedidoReporte>(
            "usp_Lab08_Pedido_Reporte",
            new { Desde = desde.Date, Hasta = hasta.Date },
            commandType: CommandType.StoredProcedure);
    }
}
