using Microsoft.AspNetCore.Mvc;
using Models;
using Interface;
using Repository;
using ViewModels;

namespace Controllers;

public class ProductosController : Controller
{
    private readonly IProductosRepository datos;

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

    [HttpPost]
    public IActionResult AltaProducto(ProductoViewModel PVM)
    {
        if (!ModelState.IsValid)
        {
            // Si falla: Devolvemos el ViewModel con los datos y errores a la Vista
            return View(PVM);

        }
        var producto = new Productos
        {
            Descripcion = PVM.Descripcion,
            Precio = PVM.Precio
        };

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
        ProductoViewModel PVM = new ProductoViewModel
        {
            IdProducto = producto.IdProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };
        if (PVM == null)
        {
            TempData["ErrorMessage"] = "Producto no encontrado";
            return RedirectToAction("Index");
        }
        return View(PVM);
    }

    [HttpPost]
    public IActionResult ModificarProducto(int id, ProductoViewModel PVM)
    {
        if (id != PVM.IdProducto)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            // Si falla: Devolvemos el ViewModel con los datos y errores a la Vista
            return View(PVM);

        }
        var producto = new Productos
        {
            IdProducto = PVM.IdProducto,
            Descripcion = PVM.Descripcion,
            Precio = PVM.Precio
        };
        datos.ActualizarProducto(producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetProductos()
    {
        List<Productos> listaProductos = datos.ListarProductos() ?? [];
        return Ok(listaProductos);
    }

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
        ProductoViewModel PVM = new ProductoViewModel
        {
            IdProducto = producto.IdProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };

        if (PVM == null)
        {
            TempData["ErrorMessage"] = "NO se pudo encontrar la Producto...";
            return RedirectToAction("Index");
        }
        return View(PVM);
    }

    [HttpGet]
    public IActionResult DeleteProductoPorId(int  id)
    {
        datos.EliminarProducto(id);
        return RedirectToAction("Index");
    }
}