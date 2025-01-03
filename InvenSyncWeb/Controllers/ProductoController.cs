using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                var productos = _productoService.GetAll();
                return CreateResponse(true, "Productos generados exitosamente", productos);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al listar productos.");
            }
        }

        [HttpGet]
        public JsonResult ObtenerCategorias()
        {
            var categorias = _db.Categoria
                .Select(c => new { c.Id, c.Descripcion })
                .ToList();
            return Json(categorias, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GuardarImagen(HttpPostedFileBase imagen)
        {
            try
            {
                if (imagen == null || imagen.ContentLength <= 0)
                {
                    return Json(new { success = false, message = "Archivo inválido." });
                }

                var validacion = ValidarImagen(imagen);
                if (!validacion.success)
                {
                    return Json(validacion);
                }

                string nombreArchivo = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                string ruta = Path.Combine(Server.MapPath("~/Content/Imagenes/Productos"), nombreArchivo);

                // Asegúrate de que la carpeta exista
                Directory.CreateDirectory(Path.GetDirectoryName(ruta));

                await Task.Run(() => imagen.SaveAs(ruta));

                var rutaRelativa = Url.Content("~/Content/Imagenes/Productos/" + nombreArchivo);
                return Json(new { success = true, ruta = rutaRelativa });
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al guardar la imagen.");
            }
        }

        private (bool success, string message) ValidarImagen(HttpPostedFileBase imagen)
        {
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imagen.FileName).ToLower();

            if (!extensionesPermitidas.Contains(extension))
            {
                return (false, "Formato de archivo no permitido.");
            }

            return (true, string.Empty);
        }

        [HttpPost]
        public JsonResult GuardarProducto(Producto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return Json(new { success = false, message = "Datos inválidos", errores });
                }

                var errorMessage = _productoService.ValidarAntesCrear(producto);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    if (producto.Id == 0)
                    {
                        bool idProducto = _productoService.Crear(producto);
                        if (idProducto)
                        {
                            return CreateResponse(true, "Producto registrado exitosamente", producto);
                        }
                        else
                        {
                            return CreateResponse(false, "Error al crear el producto.");
                        }
                    }
                    else
                    {
                        bool actualizado = _productoService.Actualizar(producto);
                        if (actualizado)
                        {
                            return CreateResponse(true, "Producto actualizado exitosamente", producto);
                        }
                        else
                        {
                            return CreateResponse(false, "Error al actualizar el producto.");
                        }
                    }
                }
                else
                {
                    return Json(new { success = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al guardar el producto.");
            }
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            try
            {
                bool eliminado = _productoService.Eliminar(id);
                if (eliminado)
                {
                    return CreateResponse(true, "Producto eliminado exitosamente");
                }
                else
                {
                    return CreateResponse(false, "Error al eliminar el producto.");
                }
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Error al eliminar el producto.");
            }
        }
    }
}