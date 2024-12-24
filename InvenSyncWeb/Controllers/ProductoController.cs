using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvenSyncWeb.Controllers
{
    public class ProductoController : BaseController
    {

        private readonly InvenSyncEntity _db;
        private readonly ProductoServices _productoService;

        public ProductoController()
        {
            _db = new InvenSyncEntity();
            _productoService = new ProductoServices(_db);
        }

        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarProducto()
        {
            try
            {
                var producto = _productoService.GetAll();
                return CreateResponse(true, "Producto generado exitosamente", producto);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al listar producto.");
            }
        }
    }
}