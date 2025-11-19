using Models;
namespace Interface;

interface IProductosRepository
{
    void CrearProducto(Productos producto);
    void ActualizarProducto(Productos producto);
    List<Productos> ListarProductos();
    Productos ObtenerProductoPorId(int id);
    public void EliminarProducto(int id);
}