using Microsoft.AspNetCore.Mvc;
using MiWebAPI.Models;
using MiWebAPI.Interface;
using MiWebAPI.Repository;

namespace MiWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PresupuestosController : ControllerBase
{

    private readonly IPresupuestos datos;

    public PresupuestosController()
    {
        datos = new PresupuestosRepository();
    }

    /// <summary>
    /// Crea una Presupuestos
    /// </summary>
    /// <param name="nombreDestinatario">nombre del Presupuesto</param>
    /// <param name="fechaCreacion">fecha de creacion del Presupuesto</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost("/api/Presupuesto")]
    public IActionResult CrearPresupuesto(string nombreDestinatario, DateOnly fechaCreacion)
    {
        if (string.IsNullOrEmpty(nombreDestinatario)) return BadRequest("El nombre no puede estar vacia...");
        if (nombreDestinatario.Length > 1000) return BadRequest(" El nombre es muy largo...");
        if (fechaCreacion > DateOnly.FromDateTime(DateTime.Today)) return BadRequest(" Fecha incorrecta...");

        Presupuestos presupuesto = new(1, nombreDestinatario, fechaCreacion, []);
        if (presupuesto != null)
        {
            datos.CrearPresupuesto(presupuesto);
            return Ok(" Presupuesto Creado correctamente...");
        }
        else
        {
            return BadRequest(" No se pudo Crear el Presupuesto...");
        }
    }

    /// <summary>
    /// Agrega un producto a un presupuesto
    /// </summary>
    /// <param name="idPresupuesto">id de la Presupuesto</param>
    /// <param name="idProducto">id del producto a agregar</param>
    /// <param name="cantidad">cantiadad de productos</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost("/api/Presupuesto/{idPresupuesto}")]
    public IActionResult AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        if (idPresupuesto < 0 || idProducto < 0) return NotFound(" El id no puede ser negativo...");
        if (cantidad < 0) return NotFound(" La cantidad no puede ser negativa...");
        datos.AgregarProductoAPresupuesto(idPresupuesto, idProducto, cantidad);
        return Ok(" Producto agregado correctamente a presupuesto...");
    }


    /// <summary>
    /// Devuelve la lista de Presupuestos
    /// </summary>
    /// <returns>ok</returns>
    [HttpGet("/api/Presupuestos")]
    public IActionResult GetPresupuestos()
    {
        List<Presupuestos> listaPresupuestos = datos.ListarPresupuestos() ?? [];
        return Ok(listaPresupuestos);
    }

    /// <summary>
    /// Devuelve una Presupuestos por su id
    /// </summary>
    /// <param name="id">id del Presupuesto buscado</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpGet("/api/Presupuestos/{id}")]
    public IActionResult GetPresupuestosPorId(int id)
    {
        Presupuestos presupuestos = (datos.ListarPresupuestos() ?? null).Find(p => p.IdPresupuesto == id);
        if (presupuestos == null) return BadRequest("Presupuesto no encontrada");
        return Ok(presupuestos);
    }


    /// <summary>
    /// Elimina una Presupuestos por su id
    /// </summary>
    /// <param name="id">id de la Presupuestos a eliminar</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpDelete("/api/Presupuestos")]
    public IActionResult DeletePresupuestosPorId(int id)
    {
        Presupuestos presupuestos = datos.ObtenerPresupuestoPorId(id) ?? null;
        if (presupuestos == null) return NotFound("NO se pudo encontrar el Presupuesto...");
        datos.EliminarPresupuesto(id);
        return NoContent();
    }
}