using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvenSyncWeb.Controllers
{
    public class ClienteController : BaseController
    {
        private readonly InvenSyncEntity _db;
        private readonly ClienteServices _clienteService;

        public ClienteController()
        {
            _db = new InvenSyncEntity();
            _clienteService = new ClienteServices(_db);
        }


        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarCliente()
        {
            try
            {
                var clientes = _clienteService.GetAll();
                return CreateResponse(true, "Clientes obtenidos exitosamente", clientes);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al listar clientes.");
            }
        }

        [HttpPost]
        public JsonResult GuardarCliente(Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CreateResponse(false, "Datos inválidos", cliente);

                var errorMessage = _clienteService.ValidarAntesCrear(cliente);
                if (string.IsNullOrEmpty(errorMessage) && cliente.Id == 0)
                {
<<<<<<< HEAD
                    cliente.Codigo = _clienteService.GenerarCodigoCliente();
=======
>>>>>>> 74359c0d6097fad34d5222f0190d3674af346238
                    bool idCliente = _clienteService.Crear(cliente);
                    return CreateResponse(true, "Cliente registrado exitosamente", new { idCliente });
                }
                else
                {
                    _clienteService.Actualizar(cliente);
                    return CreateResponse(true, "Cliente actualizado exitosamente", cliente);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(false, "Se ha ocasionado un error en el servidor, contacte al administrador...", ex.Message);
            }
        }

        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            try
            {
                // Validar si el cliente puede ser eliminado
                var errorMessage = _clienteService.ValidarAntesEliminar(id);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    // Si hay un mensaje de error, no se permite eliminar
                    return Json(new { success = false, message = errorMessage });
                }

                // Intentar eliminar el cliente
                bool resultado = _clienteService.Eliminar(id);
                if (resultado)
                {
                    return Json(new { success = true, message = "Cliente eliminado exitosamente." });
                }
                else
                {
                    return Json(new { success = false, message = "Error al eliminar el cliente. Intente nuevamente." });
                }
            }
            catch (Exception ex)
            {
                // Registro del error (puedes agregar un logger aquí si usas uno)
                Console.WriteLine($"Error eliminando cliente con ID {id}: {ex.Message}");

                // Retornar un mensaje genérico para no exponer detalles sensibles
                return Json(new { success = false, message = "Ocurrió un error en el servidor. Por favor, contacte al administrador." });
            }
        }
    }
}