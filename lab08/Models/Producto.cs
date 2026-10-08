using System.ComponentModel.DataAnnotations;

namespace lab08.Models;

public class Producto
{
    public int ProductoID { get; set; }

    [Display(Name = "Producto")]
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres.")]
    public string NombreProducto { get; set; } = string.Empty;

    [Display(Name = "Proveedor")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un proveedor.")]
    public int? ProveedorID { get; set; }

    [Display(Name = "Categoría")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
    public int? CategoriaID { get; set; }

    [Display(Name = "Cantidad por unidad")]
    [StringLength(60, ErrorMessage = "La cantidad por unidad no puede superar los 60 caracteres.")]
    public string? CantidadPorUnidad { get; set; }

    [Display(Name = "Precio unitario")]
    [DataType(DataType.Currency)]
    [Range(0, 9999999.99, ErrorMessage = "El precio debe ser mayor o igual que cero.")]
    public decimal PrecioUnidad { get; set; }

    [Display(Name = "Existencias")]
    [Range(0, short.MaxValue, ErrorMessage = "Las existencias deben ser mayores o iguales que cero.")]
    public short UnidadesEnExistencia { get; set; }

    [Display(Name = "Unidades en pedido")]
    [Range(0, short.MaxValue, ErrorMessage = "Las unidades en pedido deben ser mayores o iguales que cero.")]
    public short UnidadesEnPedido { get; set; }

    [Display(Name = "Nivel de reorden")]
    [Range(0, short.MaxValue, ErrorMessage = "El nivel de reorden debe ser mayor o igual que cero.")]
    public short NivelDeReorden { get; set; }

    [Display(Name = "Descontinuado")]
    public bool Descontinuado { get; set; }

    public string? NombreProveedor { get; set; }
    public string? NombreCategoria { get; set; }
}
