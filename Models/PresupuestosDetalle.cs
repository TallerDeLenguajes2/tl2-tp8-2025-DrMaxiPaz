namespace Models;

public class PresupuestosDetalle
{
    private Productos producto;
    private int cantidad;

    public PresupuestosDetalle()
    {
    }

    public PresupuestosDetalle(Productos producto, int cantidad)
    {
        this.Producto = producto;
        this.Cantidad = cantidad;
    }

    public Productos Producto { get => producto; set => producto = value; }
    public int Cantidad { get => cantidad; set => cantidad = value; }
}