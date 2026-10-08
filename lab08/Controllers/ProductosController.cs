using lab08.Models;
using lab08.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lab08.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoRepositorio _productoRepositorio;
    private readonly IProveedorRepositorio _proveedorRepositorio;

    public ProductosController(IProductoRepositorio productoRepositorio, IProveedorRepositorio proveedorRepositorio)
    {
        _productoRepositorio = productoRepositorio;
        _proveedorRepositorio = proveedorRepositorio;
    }

    public async Task<IActionResult> Index(string? busqueda)
    {
        ViewData["Busqueda"] = busqueda;
        var productos = await _productoRepositorio.ListarAsync(busqueda);
        return View(productos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var producto = await _productoRepositorio.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : View(producto);
    }

    public async Task<IActionResult> Create()
    {
        await CargarListasAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(producto.ProveedorID, producto.CategoriaID);
            return View(producto);
        }

        await _productoRepositorio.InsertarAsync(producto);
        TempData["Mensaje"] = "Producto registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _productoRepositorio.ObtenerPorIdAsync(id);
        if (producto is null)
        {
            return NotFound();
        }

        await CargarListasAsync(producto.ProveedorID, producto.CategoriaID);
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.ProductoID)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await CargarListasAsync(producto.ProveedorID, producto.CategoriaID);
            return View(producto);
        }

        await _productoRepositorio.ActualizarAsync(producto);
        TempData["Mensaje"] = "Producto actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoRepositorio.ObtenerPorIdAsync(id);
        return producto is null ? NotFound() : View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productoRepositorio.EliminarAsync(id);
        TempData["Mensaje"] = "Producto eliminado lógicamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarListasAsync(int? proveedorId = null, int? categoriaId = null)
    {
        var proveedores = await _proveedorRepositorio.ListarAsync();
        var categorias = await _productoRepositorio.ListarCategoriasAsync();
        ViewData["Proveedores"] = new SelectList(proveedores, "ProveedorID", "CompaniaNombre", proveedorId);
        ViewData["Categorias"] = new SelectList(categorias, "CategoriaID", "NombreCategoria", categoriaId);
    }
}
