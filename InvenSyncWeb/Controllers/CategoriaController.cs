using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvenSyncWeb.Controllers
{
    public class CategoriaController : BaseController
    {
        private readonly InvenSyncEntity _db;
        private readonly CategoriaServices _categoria;

        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }

        public CategoriaController()
        {
            _db = new InvenSyncEntity();
            _categoria = new CategoriaServices(_db);
        }

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            try
            {
                var categoria = _categoria.GetAll();
                return CreateResponse(true, "Categoria obtenidas exitosamente", categoria);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al listar categoria.");
            }
        }
    }
}