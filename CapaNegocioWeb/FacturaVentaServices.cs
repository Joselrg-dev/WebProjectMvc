using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocioWeb
{
    public class FacturaVentaService : CrudServices<FacturaVenta>
    {
        private InvenSyncEntity _db;

        public FacturaVentaService(InvenSyncEntity syncEntity) : base(syncEntity)
        {
            _db = syncEntity ?? new InvenSyncEntity();
        }

        /// <summary>
        /// Validaciones antes de crear una nueva factura de venta.
        /// </summary>
        /// <param name="facturaVenta"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(FacturaVenta facturaVenta)
        {
            if (string.IsNullOrWhiteSpace(facturaVenta.Codigo))
                return "El código de factura es obligatorio.";

            if (_db.FacturaVenta.Any(f => f.Codigo == facturaVenta.Codigo && f.Id != facturaVenta.Id))
                return "El código de factura ya existe.";

            if (facturaVenta.FechaFactura == default(DateTime))
                return "La fecha de emisión es obligatoria.";

            return string.Empty;
        }

        /// <summary>
        /// Validaciones antes de actualizar una factura de venta existente.
        /// </summary>
        /// <param name="facturaVenta"></param>
        /// <returns></returns>
        public string ValidarAntesActualizar(FacturaVenta facturaVenta)
        {
            var facturaDb = _db.FacturaVenta.Find(facturaVenta.Id);
            if (facturaDb == null)
                return "La factura de venta no existe.";

            if (facturaDb.Codigo == facturaVenta.Codigo)
                return string.Empty;

            return ValidarAntesCrear(facturaVenta);
        }

        /// <summary>
        /// Validaciones antes de eliminar una factura de venta.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ValidarAntesEliminar(int id)
        {
            var facturaDb = _db.FacturaVenta.Find(id);
            if (facturaDb == null)
                return "La factura de venta no existe.";

            if (facturaDb.DetalleVenta.Any())
                return "La factura no se puede eliminar porque tiene detalles asociados.";

            return string.Empty;
        }
    }
}