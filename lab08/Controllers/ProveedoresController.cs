using lab08.Models;
using lab08.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace lab08.Controllers;

public class ProveedoresController : Controller
{
    private readonly IProveedorRepositorio _proveedorRepositorio;

    public ProveedoresController(IProveedorRepositorio proveedorRepositorio)
    {
        _proveedorRepositorio = proveedorRepositorio;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _proveedorRepositorio.ListarAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Proveedor proveedor)
    {
        if (!string.IsNullOrWhiteSpace(proveedor.CompaniaNombre)
            && await _proveedorRepositorio.ExisteCompaniaAsync(proveedor.CompaniaNombre))
        {
            ModelState.AddModelError(nameof(Proveedor.CompaniaNombre), "Ya existe un proveedor con ese nombre de compañía.");
        }

        if (!ModelState.IsValid)
        {
            return View(proveedor);
        }

        await _proveedorRepositorio.InsertarAsync(proveedor);
        TempData["Mensaje"] = "Proveedor registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
