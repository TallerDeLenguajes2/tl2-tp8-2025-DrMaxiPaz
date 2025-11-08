using Microsoft.AspNetCore.Mvc;
using Models;
using Interface;
using Repository;
using ViewModels;

namespace Controllers;

public class PresupuestosController : Controller
{

    private readonly IPresupuestos datos;

    public PresupuestosController()
    {
        datos = new PresupuestosRepository();

    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> presupuestos = datos.ListarPresupuestos() ?? [];
        return View(presupuestos);
    }


    [HttpGet]
    public IActionResult CrearPresupuesto()
    {
        return View();
    }

    /// <summary>
    /// Crea una Presupuestos
    /// </summary>
    /// <param name="nombreDestinatario">nombre del Presupuesto</param>
    /// <param name="fechaCreacion">fecha de creacion del Presupuesto</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost]
    public IActionResult CrearPresupuesto(PresupuestoViewModel PVM)
    {
        if (!ModelState.IsValid)
        {
            return View(PVM);
        }
        Presupuestos presupuesto = new Presupuestos
        {
            NombreDestinatario = PVM.NombreDestinatario,
            FechaCreacion = PVM.FechaCreacion
        };

        if (presupuesto.FechaCreacion > DateOnly.FromDateTime(DateTime.Today))
        {
            TempData["ErrorMessage"] = " Fecha incorrecta...";
            return RedirectToAction("Index");
        }
        if (presupuesto != null)
        {
            datos.CrearPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }
        else
        {
            TempData["ErrorMessage"] = "No se pudo crear el presupuesto...";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult AgregarProductoAPresupuesto(int idPresupuesto)
    {
        if (idPresupuesto <= 0)
        {
            TempData["ErrorMessage"] = " El id no puede ser negativo...";
            return RedirectToAction("Index");
        }
        Presupuestos? presupuesto = datos.ObtenerPresupuestoPorId(idPresupuesto) ?? null;
        if (presupuesto == null)
        {
            TempData["ErrorMessage"] = "el presupuesto no existe...";
            return RedirectToAction("Index");
        }
        PresupuestoViewModel PVM = new PresupuestoViewModel
        {
            IdPresupuesto = presupuesto.IdPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
        };
        return View(PVM);
    }

    /// <summary>
    /// Agrega un producto a un presupuesto
    /// </summary>
    /// <param name="idPresupuesto">id de la Presupuesto</param>
    /// <param name="idProducto">id del producto a agregar</param>
    /// <param name="cantidad">cantiadad de productos</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpPost]
    public IActionResult AgregarProductoAPresupuestos(int idPresupuesto, int idProducto, int cantidad)
    {
        if (cantidad < 0) return NotFound(" La cantidad no puede ser negativa...");
        datos.AgregarProductoAPresupuesto(idPresupuesto, idProducto, cantidad);
        return Ok(" Producto agregado correctamente a presupuesto...");
    }


    /// <summary>
    /// Devuelve la lista de Presupuestos
    /// </summary>
    /// <returns>ok</returns>
    [HttpGet]
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
    [HttpGet]
    public IActionResult GetPresupuestosPorId(int id)
    {
        Presupuestos? presupuestos = (datos.ListarPresupuestos()).Find(p => p.IdPresupuesto == id) ?? null;
        if (presupuestos == null) return BadRequest("Presupuesto no encontrado");
        return Ok(presupuestos);
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto(int id)
    {
        Presupuestos? presupuesto = datos.ObtenerPresupuestoPorId(id) ?? null;
        if (presupuesto == null)
        {
            TempData["ErrorMessage"] = "el presupuesto no existe...";
            return RedirectToAction("Index");
        }
        return View(presupuesto);
    }

    /// <summary>
    /// Elimina una Presupuestos por su id
    /// </summary>
    /// <param name="id">id de la Presupuestos a eliminar</param>
    /// <returns>Ok o BadRequest</returns>
    [HttpGet]
    public IActionResult EliminarPresupuestoPorId(int id)
    {
        datos.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }
}