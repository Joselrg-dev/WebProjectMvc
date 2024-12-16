using CapaDatosWeb.Modelado;
using CapaNegocioWeb;
using CapaNegocioWeb.ClaseGenerica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocioWeb
{
    public class ProveedorServices : CrudServices<Proveedor>
    {
        private InvenSyncEntity _db;

        public ProveedorServices(InvenSyncEntity syncEntity) : base(syncEntity)
        {
            if (syncEntity == null)
                this._db = new InvenSyncEntity();
            else
                this._db = syncEntity;
        }

        /// <summary>
        /// Validaciones antes de crear un neuvo registro de proveedor
        /// </summary>
        /// <param name="proveedor"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(Proveedor proveedor)
        {
            // Verificar si hay otro proveedor con el mismo código, excluyendo el proveedor actual (en caso de ser actualización)
            if (_db.Proveedor.Any(x => x.Codigo.Trim().ToLower() == proveedor.Codigo.Trim().ToLower() && x.Id != proveedor.Id))
                return "Ya existe un código igual en Proveedores";

            return string.Empty;
        }

        /// <summary>
        /// Validaciones antes de actualizar un registro de proveedor
        /// </summary>
        /// <param name="proveedor"></param>
        /// <returns></returns>
        public string ValidarAntesActualizar(Proveedor proveedor)
        {
            // Buscar el proveedor en la base de datos
            var proveedorDb = _db.Proveedor.Find(proveedor.Id);

            // Verificar si el proveedor existe
            if (proveedorDb == null)
                return "El proveedor a editar ya no existe en el sistema";

            // Si el código no ha cambiado, no hay necesidad de validar la unicidad
            if (proveedorDb.Codigo == proveedor.Codigo)
                return string.Empty;

            // Verificar si el nuevo código ya existe en otro proveedor
            var proveedorExistente = _db.Proveedor
                .FirstOrDefault(p => p.Codigo == proveedor.Codigo && p.Id != proveedor.Id);

            if (proveedorExistente != null)
                return "El código ya está en uso por otro proveedor.";

            // Si no hay problemas, puedes llamar a la validación de creación
            return ValidarAntesCrear(proveedor);
        }

        /// <summary>
        /// Validaciones antes de elimar un registro de proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ValidarAntesEliminar(int id)
        {
            var proveedorDb = _db.Proveedor.Find(id);
            _db.Entry(proveedorDb).State = System.Data.Entity.EntityState.Detached;

            if (proveedorDb == null)
                return "El proveedor a eliminar ya no existe en el sistema";

            if (proveedorDb.FacturaCompra.Count > 0)
                return "El registro no se puede eliminar por que esta siendo usado por otras entidades";

            return string.Empty;
        }
    }
}
