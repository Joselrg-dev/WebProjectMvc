using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System.Linq;

namespace CapaNegocioWeb
{
    public class CategoriaServices : CrudServices<Categoria>
    {
        private readonly InvenSyncEntity _db;

        public CategoriaServices(InvenSyncEntity entity) : base(entity)
        {
            if (entity == null)
                this._db = new InvenSyncEntity();
            else
                this._db = entity;
        }

        /// <summary>
        /// Validaciones antes de crear un neuvo registro de cliente
        /// </summary>
        /// <param name="Categoria"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(Categoria Categoria)
        {
            if (_db.Categoria.Any(pdt => pdt.Codigo.Trim().ToLower() == Categoria.Codigo.Trim().ToLower() && pdt.Id != Categoria.Id))
                return "Ya existe una Categoria con el mismo código en el sistema.";

            return string.Empty;
        }

        public string ValidarAntesActualizar(Categoria Categoria)
        {
            var objCategoria = _db.Categoria.Find(Categoria.Id);

            if (objCategoria == null)
                return "La Categoria a editar ya no existe en el sistema";

            if (objCategoria.Codigo == Categoria.Codigo)
                return string.Empty;

            return ValidarAntesCrear(Categoria);
        }

        public string ValidarAntesEliminar(int id)
        {
            var objCategoria = _db.Categoria.Find(id);

            if (objCategoria == null)
                return "La Categoria a eliminar ya no existe en el sistema.";

            if (objCategoria.Producto.Count > 0)
                return "La Categoria no se puede eliminar porque está siendo usado por otra entidad";

            return string.Empty;
        }
    }
}