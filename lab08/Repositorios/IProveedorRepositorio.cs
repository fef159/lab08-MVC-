using lab08.Models;

namespace lab08.Repositorios;

public interface IProveedorRepositorio
{
    Task<IEnumerable<Proveedor>> ListarAsync();
    Task InsertarAsync(Proveedor proveedor);
    Task<bool> ExisteCompaniaAsync(string companiaNombre);
}
