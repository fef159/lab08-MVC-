using System.ComponentModel.DataAnnotations;

namespace lab08.Models;

public class Proveedor
{
    public int ProveedorID { get; set; }

    [Display(Name = "Compañía")]
    [Required(ErrorMessage = "El nombre de la compañía es obligatorio.")]
    [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres.")]
    public string CompaniaNombre { get; set; } = string.Empty;

    [Display(Name = "Contacto")]
    [StringLength(80)]
    public string? NombreContacto { get; set; }

    [Display(Name = "Cargo")]
    [StringLength(80)]
    public string? CargoContacto { get; set; }

    [Display(Name = "Dirección")]
    [StringLength(160)]
    public string? Direccion { get; set; }

    [StringLength(60)]
    public string? Ciudad { get; set; }

    [Display(Name = "Código postal")]
    [StringLength(20)]
    public string? CodigoPostal { get; set; }

    [StringLength(60)]
    public string? Pais { get; set; }

    [Display(Name = "Teléfono")]
    [StringLength(48)]
    public string? Telefono { get; set; }
}
