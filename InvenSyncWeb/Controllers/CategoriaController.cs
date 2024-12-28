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

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CreateResponse(false, "Datos inválidos", categoria);
                }

                var errorMessage = _categoria.ValidarAntesCrear(categoria);
                if (string.IsNullOrEmpty(errorMessage) && categoria.Id == 0)
                {
                    bool idCategoria = _categoria.Crear(categoria);
                    return CreateResponse(true, "Categoria registrado exitosamente", new { idCategoria });
                }
                else
                {
                    _categoria.Actualizar(categoria);
                    return CreateResponse(true, "Categoria actualizado exitosamente", categoria);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(false, "Error al registrar/actualizar el categoria.", ex.Message);
            }
        }
    }
}