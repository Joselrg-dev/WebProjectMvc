using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System.Linq;

namespace CapaNegocioWeb
{
    public class ProductoServices : CrudServices<Producto>
    {
        private readonly InvenSyncEntity _db;

        public ProductoServices(InvenSyncEntity entity) : base(entity)
        {
            if (entity == null)
                this._db = new InvenSyncEntity();
            else
                this._db = entity;
        }

        /// <summary>
        /// Validaciones antes de crear un nuevo registro de producto
        /// </summary>
        /// <param name="producto"></param>
        /// <returns></returns>
        public string ValidarAntesCrear(Producto producto)
        {
            if (_db.Producto.Any(pdt => pdt.Codigo.Trim().ToLower() == producto.Codigo.Trim().ToLower() && pdt.Id != producto.Id))
                return "Ya existe un producto con el mismo código en el sistema.";

            return string.Empty;
        }

        public string ValidarAntesActualizar(Producto producto)
        {
            var objProducto = _db.Producto.Find(producto.Id);

            if (objProducto == null)
                return "El producto a editar ya no existe en el sistema";

            if (objProducto.Codigo == producto.Codigo)
                return string.Empty;

            return ValidarAntesCrear(producto);
        }

        public string ValidarAntesEliminar(int id)
        {
            var objProducto = _db.Producto.Find(id);

            if (objProducto == null)
                return "El producto a eliminar ya no existe en el sistema.";

            if (objProducto.DetalleCompra.Count > 0)
                return "El producto no se puede eliminar porque está siendo usado por otra entidad";

            return string.Empty;
        }
    }
}