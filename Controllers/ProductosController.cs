using Microsoft.AspNetCore.Mvc;
using Models;
using Interface;
using Repository;

namespace Controllers;


public class ProductosController : Controller
{
    private readonly IProductos datos;

    public ProductosController()
    {
        datos = new ProductosRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Productos> productos = datos.ListarProductos() ?? [];
        return View(productos);
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View();
    }

    /// <summary>
    /// Crea una Producto
    /// </summary>
    /// <param name="descripcion">descripcion del Producto a crear</param>
    /// <param name="precio">precio del Producto a crear</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost]
    public IActionResult AltaProducto(string descripcion, int precio)
    {
        if (precio <= 0)
        {
            TempData["ErrorMessage"] = " El precio no puede ser negativo...";
            return RedirectToAction("Index");
        }
        if (string.IsNullOrEmpty(descripcion))
        {
            TempData["ErrorMessage"] = " La descripcion no puede estar vacia...";
            RedirectToAction("Index");
        }
        if (descripcion.Length > 1000)
        {
            TempData["ErrorMessage"] = " La descripcion es muy larga...";
            RedirectToAction("Index");
        }

        Productos producto = new(descripcion, precio);
        if (producto != null)
        {
            datos.CrearProducto(producto);
            return RedirectToAction("Index");
        }
        else
        {
            TempData["ErrorMessage"] = " No se pudo Crear el Producto";
            return RedirectToAction("Index");
        }
    }

    
    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        Productos? producto = datos.ObtenerProductoPorId(id) ?? null;
        if (producto == null)
        {
            TempData["ErrorMessage"] = "Producto no encontrado";
            return RedirectToAction("Index");
        }
        return View(producto);
    }


    /// <summary>
    ///  Permite modificar un nombre de un Producto
    /// </summary>
    /// <param name="id">id del Producto a modificar</param>
    /// <param name="descripcion">nueva descripcion del Producto a devolver</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost]
    public IActionResult ModificarProducto(Productos producto)
    {
        if (producto.Descripcion.Length > 1000)
        {
            TempData["ErrorMessage"] = "Descripcion demaciado larga...";
            return RedirectToAction("Index");
        }
        datos.ActualizarProducto(producto.IdProducto, producto);
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Devuelve la lista de Productos
    /// </summary>
    /// <returns>ok</returns>
    [HttpGet]
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
    [HttpGet]
    public IActionResult GetProductoPorId(int id)
    {
        List<Productos> listaPorductos = datos.ListarProductos();
        Productos? producto = listaPorductos.Find(p => p.IdProducto == id) ?? null;
        if (producto == null) return BadRequest("Producto no encontrada");
        return Ok(producto);
    }

    [HttpGet]
    public IActionResult DeleteProducto(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "id inexistente...";
            return RedirectToAction("Index");
        }
        Productos? producto = datos.ObtenerProductoPorId(id) ?? null;
        if (producto == null)
        {
            TempData["ErrorMessage"] = "NO se pudo encontrar la Producto...";
            return RedirectToAction("Index");
        }
        return View(producto);
    }

    /// <summary>
    /// Elimina una Producto por su id
    /// </summary>
    /// <param name="id">id de la Producto a eliminar</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpGet]
    public IActionResult DeleteProductoPorId(int id)
    {
        datos.EliminarProducto(id);
        return RedirectToAction("Index");
    }
}