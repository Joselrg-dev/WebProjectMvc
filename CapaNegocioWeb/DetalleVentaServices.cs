using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System.Linq;

namespace CapaNegocioWeb
{
    public class DetalleVentaService : CrudServices<DetalleVenta>
    {
        private InvenSyncEntity _db;

        public DetalleVentaService(InvenSyncEntity syncEntity) : base(syncEntity)
        {
            _db = syncEntity ?? new InvenSyncEntity();
        }

        /// <summary>
        /// Validaciones antes de crear un nuevo detalle de venta.
        /// </summary>
        /// <param name="detalleVenta"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(DetalleVenta detalleVenta)
        {
            if (detalleVenta.Cantidad <= 0)
                return "La cantidad debe ser mayor a 0.";

            if (detalleVenta.PrecioVenta <= 0)
                return "El precio de venta debe ser mayor a 0.";

            if (!_db.Producto.Any(p => p.Id == detalleVenta.ProductoId))
                return "El producto especificado no existe.";

            return string.Empty;
        }

        /// <summary>
        /// Validaciones antes de actualizar un detalle de venta existente.
        /// </summary>
        /// <param name="detalleVenta"></param>
        /// <returns></returns>
        public string ValidarAntesActualizar(DetalleVenta detalleVenta)
        {
            var detalleDb = _db.DetalleVenta.Find(detalleVenta.Id);
            if (detalleDb == null)
                return "El detalle de venta no existe.";

            return ValidarAntesCrear(detalleVenta);
        }

        /// <summary>
        /// Validaciones antes de eliminar un detalle de venta.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ValidarAntesEliminar(int id)
        {
            var detalleDb = _db.DetalleVenta.Find(id);
            if (detalleDb == null)
                return "El detalle de venta no existe.";

            return string.Empty;
        }
    }
}