using System.ComponentModel.DataAnnotations;

namespace lab08.Models;

public class PedidoReporte
{
    public int PedidoID { get; set; }

    [DataType(DataType.Date)]
    public DateTime FechaPedido { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaRequerida { get; set; }

    public string Cliente { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public short Cantidad { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }
    public string Estado { get; set; } = string.Empty;
}
