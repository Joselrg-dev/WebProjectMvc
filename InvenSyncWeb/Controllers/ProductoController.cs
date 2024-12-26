using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpPost]
        public JsonResult ObtenerCategorias()
        {
            var categorias = _db.Categoria
                .Select(c => new { c.Id, c.Descripcion })
                .ToList();
            return Json(categorias, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarImagen(HttpPostedFileBase imagen)
        {
            if (imagen != null && imagen.ContentLength > 0)
            {
                // Generar un nombre único para la imagen
                string nombreArchivo = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                string ruta = Path.Combine(Server.MapPath("~/Content/Imagenes/Productos"), nombreArchivo);

                // Guardar imagen en el servidor
                imagen.SaveAs(ruta);

                // Devolver la ruta de la imagen
                return Json(new { success = true, ruta = "/Content/Imagenes/Productos/" + nombreArchivo });
            }
            return Json(new { success = false, message = "No se pudo guardar la imagen." });
        }

        [HttpPost]
        public JsonResult GuardarProducto(Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return CreateResponse(false, "Datos inválidos", producto);
                }

                var errorMessage = _productoService.ValidarAntesCrear(producto);
                if (string.IsNullOrEmpty(errorMessage) && producto.Id == 0)
                {
                    bool idProducto = _productoService.Crear(producto);
                    return CreateResponse(true, "Producto registrado exitosamente", new { idProducto });
                }
                else
                {
                    _productoService.Actualizar(producto);
                    return CreateResponse(true, "Producto actualizado exitosamente", producto);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(false, "Error al registrar/actualizar el producto.", ex.Message);
            }
        }
    }
}