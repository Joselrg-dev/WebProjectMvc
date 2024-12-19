using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvenSyncWeb.Controllers
{
    public class ProveedorController : BaseController
    {
        private readonly InvenSyncEntity _db;
        private readonly ProveedorServices _proveedorService;

        public ProveedorController()
        {
            _db = new InvenSyncEntity();
            _proveedorService = new ProveedorServices(_db);
        }

        // GET: Proveedor
        public ActionResult Index()
        {
            try
            {
                var proveedores = _proveedorService.GetAll();
                return View(proveedores);
            }
            catch (Exception ex)
            {
                // Manejo de errores en vista
                ViewBag.ErrorMessage = "Error al cargar la lista de proveedores: " + ex.Message;
                return View(new List<Proveedor>());
            }
        }

        [HttpPost]
        public JsonResult EliminarProveedor(int id)
        {
            try
            {
                // Llama al servicio para eliminar el proveedor
                bool resultado = _proveedorService.Eliminar(id);

                if (resultado)
                {
                    return Json(new { success = true, message = "Proveedor eliminado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Error al eliminar el proveedor. Intente nuevamente." });
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return Json(new { success = false, message = "Error en el servidor, contactar al administrador." + ex.Message});
            }
        }

        [HttpPost]
        public JsonResult RegistrarProveedor(Proveedor proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CreateResponse(false, "Datos inválidos", proveedor);

                var errorMessage = _proveedorService.ValidarAntesCrear(proveedor);
                if (string.IsNullOrEmpty(errorMessage) && proveedor.Id == 0)
                {

                    bool idProveedor = _proveedorService.Crear(proveedor);
                    return CreateResponse(true, "Proveedor registrado exitosamente", new { idProveedor });
                }
                else
                {
                    _proveedorService.Actualizar(proveedor);
                    return CreateResponse(true, "Proveedor actualizado exitosamente", proveedor);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(false, "Error al registrar/actualizar el proveedor.", ex.Message);
            }
        }

        [HttpGet]
        public JsonResult ListarProveedor()
        {
            try
            {
                var proveedores = _proveedorService.GetAll();
                return CreateResponse(true, "Proveedores obtenidos exitosamente", proveedores);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al listar proveedores.");
            }
        }
    }
}