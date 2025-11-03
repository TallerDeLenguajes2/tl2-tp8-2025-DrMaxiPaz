namespace MiWebAPI.Models;

class Presupuestos
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateOnly fechaCreacion;
    private List<PresupuestosDetalle> detalle;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public DateOnly FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    internal List<PresupuestosDetalle> Detalle { get => detalle; set => detalle = value; }

    public Presupuestos()
    {
    }
    
    public Presupuestos(int idPresupuesto, string nombreDestinatario, DateOnly fechaCreacion, List<PresupuestosDetalle> detalle)
    {
        this.IdPresupuesto = idPresupuesto;
        this.NombreDestinatario = nombreDestinatario;
        this.FechaCreacion = fechaCreacion;
        this.Detalle = detalle;
    }

    public double MontoPresupuesto()
    {
        double total = 0;
        foreach (var d in Detalle)
        {
            total += d.Producto.Precio;
        }
        return total;
    }

    public double MontoPresupuestoConIVA()
    {
        double total = 0;
        foreach (var d in Detalle)
        {
            total += d.Producto.Precio;
        }
        return total * 1.21;
    }
    public double CantidadProductos()
    {
        return Detalle.Sum(p => p.Cantidad);
    }
    
}