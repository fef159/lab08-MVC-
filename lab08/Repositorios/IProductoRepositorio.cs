using lab08.Models;

namespace lab08.Repositorios;

public interface IProductoRepositorio
{
    Task<IEnumerable<Producto>> ListarAsync(string? busqueda);
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task InsertarAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);
    Task<IEnumerable<Categoria>> ListarCategoriasAsync();
}
