using Models;

namespace Interface;

interface IPresupuestos
{
    void CrearPresupuesto(Presupuestos presupuesto);
    void AgregarProductoAPresupuesto(int idPresupuesto,int idProducto, int cantidad);
    List<Presupuestos> ListarPresupuestos();
    public Presupuestos ObtenerPresupuestoPorId(int id);
    void EliminarPresupuesto(int id);
}