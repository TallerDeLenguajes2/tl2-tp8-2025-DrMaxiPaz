using Microsoft.AspNetCore.Mvc;
using MiWebAPI.Models;
using MiWebAPI.Interface;
using MiWebAPI.Repository;

namespace MiWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductosController : ControllerBase
{

    private readonly IProductos datos;

    public ProductosController()
    {
        datos = new ProductosRepository();
    }

    /// <summary>
    /// Crea una Producto
    /// </summary>
    /// <param name="descripcion">descripcion del Producto a crear</param>
    /// <param name="precio">precio del Producto a crear</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost("/api/Producto")]
    public IActionResult CrearProducto(string descripcion, int precio)
    {
        if (precio < 0) return BadRequest(" El precio no puede ser negativo...");
        if (string.IsNullOrEmpty(descripcion)) return BadRequest(" La descripcion no puede estar vacia...");
        if (descripcion.Length > 1000) return BadRequest(" La descripcion es muy larga...");
        Productos producto = new(0, descripcion, precio);
        if (producto != null)
        {
            datos.CrearProducto(producto);
            return Ok(" Producto Creado correctamente...");
        }
        else
        {
            return BadRequest(" No se pudo Crear el Producto");
        }
    }

    /// <summary>
    ///  Permite modificar un nombre de un Producto
    /// </summary>
    /// <param name="id">id del Producto a modificar</param>
    /// <param name="descripcion">nueva descripcion del Producto a devolver</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPut("/api/Producto/{id}")]
    public IActionResult ModificarProducto(int id, string descripcion)
    {
        if (descripcion.Length > 1000) return BadRequest("descripcion muy larga...");
        Productos producto = datos.ObtenerProductoPorId(id) ?? null;
        if (producto == null) return BadRequest("Producto no encontrado");
        producto.Descripcion = descripcion;
        datos.ActualizarProducto(id, producto);
        return Ok(producto);
    }

    /// <summary>
    /// Devuelve la lista de Productos
    /// </summary>
    /// <returns>ok</returns>
    [HttpGet("/api/Productos")]
    public IActionResult GetProductos()
    {
        List<Productos> listaProductos = datos.ListarProductos() ?? [];
        return Ok(listaProductos);
    }

    /// <summary>
    /// Devuelve una Producto por su id
    /// </summary>
    /// <param name="id">id del Producto a devolver</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpGet("/api/Producto/{id}")]
    public IActionResult GetProductoPorId(int id)
    {
        Productos producto = (datos.ListarProductos() ?? null).Find(p => p.IdProducto == id);
        if (producto == null) return BadRequest("Producto no encontrada");
        return Ok(producto);
    }


    /// <summary>
    /// Elimina una Producto por su id
    /// </summary>
    /// <param name="id">id de la Producto a eliminar</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpDelete("/api/Producto")]
    public IActionResult DeleteProductoPorId(int id)
    {
        Productos producto = datos.ObtenerProductoPorId(id) ?? null;
        if (producto == null) return BadRequest("NO se pudo encontrar la Producto");
        datos.EliminarProducto(id);
        return Ok("Producto borrado correctamente");
    }
}