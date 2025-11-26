using Microsoft.AspNetCore.Mvc;
using Models;
using Interface;
using Repository;
using ViewModels;

namespace Controllers;

public class ProductosController : Controller
{
    private readonly IProductoRepository datos;
    private IAuthenticationService _authService;

    public ProductosController(IProductoRepository prodRepo,
IAuthenticationService authService)
    {
        //datos = new ProductoRepository();
        datos = prodRepo;
        _authService = authService;
    }


    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        List<Productos> productos = datos.ListarProductos() ?? [];
        return View(productos);
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        // Aplicamos el chequeo de seguridad
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        return View();
    }

    [HttpPost]
    public IActionResult AltaProducto(ProductoViewModel PVM)
    {
        // Aplicamos el chequeo de seguridad
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        // Aplicamos el chequeo de seguridad
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

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
        // Aplicamos el chequeo de seguridad
 var securityCheck = CheckAdminPermissions();
 if (securityCheck != null) return securityCheck;
        
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
        // Aplicamos el chequeo de seguridad
 var securityCheck = CheckAdminPermissions();
 if (securityCheck != null) return securityCheck;
        
        List<Productos> listaProductos = datos.ListarProductos() ?? [];
        return Ok(listaProductos);
    }

    [HttpGet]
    public IActionResult GetProductoPorId(int id)
    {
        // Aplicamos el chequeo de seguridad
 var securityCheck = CheckAdminPermissions();
 if (securityCheck != null) return securityCheck;
        
        List<Productos> listaPorductos = datos.ListarProductos();
        Productos? producto = listaPorductos.Find(p => p.IdProducto == id) ?? null;
        if (producto == null) return BadRequest("Producto no encontrada");
        return Ok(producto);
    }

    [HttpGet]
    public IActionResult DeleteProducto(int id)
    {
        // Aplicamos el chequeo de seguridad
 var securityCheck = CheckAdminPermissions();
 if (securityCheck != null) return securityCheck;
        
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
    public IActionResult DeleteProductoPorId(int id)
    {
        // Aplicamos el chequeo de seguridad
 var securityCheck = CheckAdminPermissions();
 if (securityCheck != null) return securityCheck;
        
        datos.EliminarProducto(id);
        return RedirectToAction("Index");
    }

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_authService.HasAccessLevel("Administrador"))
        {
            // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }
}