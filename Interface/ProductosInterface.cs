using Models;
namespace Interface;

interface IProductos
{
    void CrearProducto(Productos producto);
    void ActualizarProducto(int id, Productos producto);
    List<Productos> ListarProductos();
    Productos ObtenerProductoPorId(int id);

    public void EliminarProducto(int id);
}