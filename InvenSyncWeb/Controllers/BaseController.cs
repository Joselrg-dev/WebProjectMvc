using System;
using System.Web.Mvc;

namespace InvenSyncWeb.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Método para manejar respuestas JSON estándar
        /// </summary>
        public JsonResult CreateResponse(bool success, string message, object data = null)
        {
            return Json(new { success, message, data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Método para manejar errores y formatear la respuesta
        /// </summary>
        public JsonResult HandleError(Exception ex, string customMessage = "Ocurrió un error inesperado")
        {
            return CreateResponse(false, customMessage, new { error = ex.Message });
        }
    }
}