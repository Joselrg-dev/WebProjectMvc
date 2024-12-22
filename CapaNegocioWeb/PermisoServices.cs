using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocioWeb
{
    public class PermisoServices : CrudServices<Permiso>
    {
        private readonly InvenSyncEntity _db;

        public PermisoServices(InvenSyncEntity entity) : base(entity)
        {
            if (entity == null)
                this._db = new InvenSyncEntity();
            else
                this._db = entity;
        }

        /// <summary>
        /// Validaciones antes de crear un neuvo registro de permiso
        /// </summary>
        /// <param name="permsio"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(Permiso permiso)
        {
            // Verificar si hay otro permiso con el mismo código, excluyendo el permiso actual (en caso de ser actualización)
            if (_db.Permiso.Any(x => x.Codigo.Trim().ToLower() == permiso.Codigo.Trim().ToLower() && x.Id != permiso.Id))
                return "Ya existe un código igual en Permiso";

            return string.Empty;
        }

        /// <summary>
        /// Validaciones antes de actualizar un registro de permiso
        /// </summary>
        /// <param name="permiso"></param>
        /// <returns></returns>
        public string ValidarAntesActualizar(Permiso permiso)
        {
            // Buscar el permiso en la base de datos
            var permisoDb = _db.Permiso.Find(permiso.Id);

            // Verificar si el permiso existe
            if (permisoDb == null)
                return "El permiso a editar ya no existe en el sistema";

            // Si el código no ha cambiado, no hay necesidad de validar la unicidad
            if (permisoDb.Codigo == permiso.Codigo)
                return string.Empty;

            // Verificar si el nuevo código ya existe en otro permiso
            var permisoExistente = _db.Permiso
                .FirstOrDefault(p => p.Codigo == permiso.Codigo && p.Id != permiso.Id);

            if (permisoExistente != null)
                return "El código ya está en uso por otro permiso.";

            // Si no hay problemas, puedes llamar a la validación de creación
            return ValidarAntesCrear(permiso);
        }

        /// <summary>
        /// Validaciones antes de elimar un registro de permiso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ValidarAntesEliminar(int id)
        {
            var permisoDb = _db.Permiso.Find(id);
            _db.Entry(permisoDb).State = System.Data.Entity.EntityState.Detached;

            if (permisoDb == null)
                return "El permiso a eliminar ya no existe en el sistema";

            if (permisoDb.Rol.Count > 0)
                return "El registro no se puede eliminar por que esta siendo usado por otras entidades";

            return string.Empty;
        }
    }
}