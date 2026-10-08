using lab08.Models;

namespace lab08.Repositorios;

public interface IPedidoRepositorio
{
    Task<IEnumerable<PedidoReporte>> ReporteAsync(DateTime desde, DateTime hasta);
}
