using lab08.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace lab08.Controllers;

public class PedidosController : Controller
{
    private readonly IPedidoRepositorio _pedidoRepositorio;

    public PedidosController(IPedidoRepositorio pedidoRepositorio)
    {
        _pedidoRepositorio = pedidoRepositorio;
    }

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        var fechaHasta = (hasta ?? DateTime.Today).Date;
        var fechaDesde = (desde ?? fechaHasta.AddMonths(-3)).Date;

        if (fechaDesde > fechaHasta)
        {
            ModelState.AddModelError(string.Empty, "La fecha inicial no puede ser posterior a la fecha final.");
            (fechaDesde, fechaHasta) = (fechaHasta, fechaDesde);
        }

        ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");
        var reporte = await _pedidoRepositorio.ReporteAsync(fechaDesde, fechaHasta);
        return View(reporte);
    }
}
