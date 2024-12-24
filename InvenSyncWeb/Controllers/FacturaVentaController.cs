using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvenSyncWeb.Controllers
{
    public class FacturaVentaController : BaseController
    {
        private readonly InvenSyncEntity _db;
        private readonly FacturaVentaService _facturaVentaService;

        public FacturaVentaController()
        {
            _db = new InvenSyncEntity();
            _facturaVentaService = new FacturaVentaService(_db);
        }

        // GET: FacturaVenta

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarFacturaVenta()
        {
            try
            {
                var facturVenta = _facturaVentaService.GetAll();
                return CreateResponse(true, "Factura Venta generada exitosamente", facturVenta);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al listar Factura Venta.");
            }
        }
    }
}