using Models;
namespace Interface;

public interface IProductoRepository
{
    void CrearProducto(Productos producto);
    void ActualizarProducto(Productos producto);
    List<Productos> ListarProductos();
    Productos ObtenerProductoPorId(int id);
    public void EliminarProducto(int id);
}