using Models;
namespace Interface;

public interface IPresupuestoRepository
{
    void CrearPresupuesto(Presupuestos presupuesto);
    void AgregarProductoAPresupuesto(int idPresupuesto,int idProducto, int cantidad);
    List<Presupuestos> ListarPresupuestos();
    public Presupuestos ObtenerPresupuestoPorId(int id);
    public List<PresupuestosDetalle> MostrarDetallePorId(int id);
    void EliminarPresupuesto(int id);
}