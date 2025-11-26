using Microsoft.AspNetCore.Mvc;
using Models;
using Interface;
using Repository;
using ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList

namespace Controllers;

public class PresupuestosController : Controller
{

    private readonly IPresupuestoRepository datos;
    private readonly IProductoRepository repo_produ;

    public PresupuestosController()
    {
        datos = new PresupuestoRepository();
        repo_produ = new ProductoRepository();

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
    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        /*if (idPresupuesto <= 0)
        {
            TempData["ErrorMessage"] = " El id no puede ser negativo...";
            return RedirectToAction("Index");
        }*/
        Presupuestos? presupuesto = datos.ObtenerPresupuestoPorId(id) ?? null;
        if (presupuesto == null)
        {
            TempData["ErrorMessage"] = "el presupuesto no existe...";
            return RedirectToAction("Index");
        }
        List<Productos> productos = repo_produ.ListarProductos();

        AgregarProductoViewModel APVM = new AgregarProductoViewModel
        {
            IdPresupuesto = presupuesto.IdPresupuesto,
            ListaProductos = new SelectList(productos, "IdProducto","Descripcion")
        };
        return View(APVM);
    }


    [HttpPost]
    public IActionResult AgregarProductoAPresupuestos(AgregarProductoViewModel APVM)
    {
        if (!ModelState.IsValid)
        {
            // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
            // debemos recargar el SelectList porque se pierde en el POST.
            var productos = repo_produ.ListarProductos();
            APVM.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");

            // Devolvemos el modelo con los errores y el dropdown recargado
            return View(APVM);
        }
        // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
        datos.AgregarProductoAPresupuesto(APVM.IdPresupuesto, APVM.IdProducto, APVM.Cantidad);
        // 3. Redirigimos al detalle del presupuesto
        return RedirectToAction("MostrarPresupuesto", new { id = APVM.IdPresupuesto });
    }

    [HttpGet]
    public IActionResult GetPresupuestos()
    {

        List<Presupuestos> listaPresupuestos = datos.ListarPresupuestos() ?? [];
        return Ok(listaPresupuestos);
    }

    [HttpGet]
    public IActionResult MostrarPresupuesto(int id)
    {
        List<Presupuestos> list = datos.ListarPresupuestos() ?? [];
        Presupuestos? presupuesto = list.Find(p => p.IdPresupuesto == id) ?? null;
        if (presupuesto == null) return BadRequest("Presupuesto no encontrado...");
        return View(presupuesto);
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

    [HttpPost]
    public IActionResult EliminarPresupuestoPorId(int id)
    {
        datos.EliminarPresupuesto(id);
        return RedirectToAction("Index");
    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }
}