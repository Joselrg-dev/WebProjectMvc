using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocioWeb
{
    public class RolServices : CrudServices<Rol>
    {
        private readonly InvenSyncEntity _db;

        public RolServices(InvenSyncEntity entity) : base(entity)
        {
            if (entity == null)
                this._db = new InvenSyncEntity();
            else
                this._db = entity;
        }

        /// <summary>
        /// Validaciones antes de crear un neuvo registro de Rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(Rol rol)
        {
            // Verificar si hay otro Rol con el mismo código, excluyendo el Rol actual (en caso de ser actualización)
            if (_db.Rol.Any(x => x.Codigo.Trim().ToLower() == rol.Codigo.Trim().ToLower() && x.Id != rol.Id))
                return "Ya existe un código igual en Proveedores";

            return string.Empty;
        }

        /// <summary>
        /// Validaciones antes de actualizar un registro de Rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public string ValidarAntesActualizar(Rol rol)
        {
            // Buscar el Rol en la base de datos
            var RolDb = _db.Rol.Find(rol.Id);

            // Verificar si el Rol existe
            if (RolDb == null)
                return "El Rol a editar ya no existe en el sistema";

            // Si el código no ha cambiado, no hay necesidad de validar la unicidad
            if (RolDb.Codigo == rol.Codigo)
                return string.Empty;

            // Verificar si el nuevo código ya existe en otro Rol
            var RolExistente = _db.Rol
                .FirstOrDefault(p => p.Codigo == rol.Codigo && p.Id != rol.Id);

            if (RolExistente != null)
                return "El código ya está en uso por otro Rol.";

            // Si no hay problemas, puedes llamar a la validación de creación
            return ValidarAntesCrear(rol);
        }

        /// <summary>
        /// Validaciones antes de elimar un registro de Rol
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ValidarAntesEliminar(int id)
        {
            var RolDb = _db.Rol.Find(id);
            _db.Entry(RolDb).State = System.Data.Entity.EntityState.Detached;

            if (RolDb == null)
                return "El Rol a eliminar ya no existe en el sistema";

            if (RolDb.Usuario.Count > 0)
                return "El registro no se puede eliminar por que esta siendo usado por otras entidades";

            return string.Empty;
        }
    }
}