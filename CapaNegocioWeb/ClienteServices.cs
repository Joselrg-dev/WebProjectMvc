using CapaDatosWeb.Modelado;
using CapaNegocioWeb.ClaseGenerica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocioWeb
{
    public class ClienteServices : CrudServices<Cliente>
    {
        private readonly InvenSyncEntity _db;

        public ClienteServices(InvenSyncEntity entity) : base(entity)
        {
            if (entity == null)
                this._db = new InvenSyncEntity();
            else
                this._db = entity;
        }

        /// <summary>
        /// Validaciones antes de crear un neuvo registro de cliente
        /// </summary>
        /// <param name="cliente></param>
        /// <returns></returns>
        public string ValidarAntesCrear(Cliente cliente) 
        {
            if (_db.Cliente.Any(clt => clt.Codigo.Trim().ToLower() == cliente.Codigo.Trim().ToLower() && clt.Id != cliente.Id))
                return "Ya existe un cliente con el mismo código en el sistema.";

            return string.Empty;
        }


        public string ValidarAntesActualizar(Cliente cliente)
        {
            var objCliente = _db.Cliente.Find(cliente.Id);

            if(objCliente == null)
                return "El cliente a editar ya no existe en el sistema";

            if (objCliente.Codigo == cliente.Codigo)
                return string.Empty;

            return ValidarAntesCrear(cliente);
        }

        public string ValidarAntesEliminar(int id)
        {
            var objCliente = _db.Cliente.Find(id);

            if (objCliente == null)
                return "El cliente a eliminar ya no existe en el sistema.";

            if (objCliente.FacturaVenta.Count > 0)
                return "El cliente no se puede eliminar porque está siendo usado por otra entidad";

            return string.Empty;
        }

        public string GenerarCodigoCliente()
        {
            // Obtener el último cliente registrado (puedes usar Entity Framework o similar)
            var ultimoCliente = _db.Cliente
                                 .OrderByDescending(c => c.Id) // Ordenar por ID para obtener el último registro
                                 .FirstOrDefault();

            string nuevoCodigo;

            if (ultimoCliente == null || string.IsNullOrEmpty(ultimoCliente.Codigo))
            {
                // Si no hay clientes, empieza desde 0001
                nuevoCodigo = "CLI0001";
            }
            else
            {
                // Extraer el número y sumarle 1
                int numero = int.Parse(ultimoCliente.Codigo.Substring(3)) + 1;
                nuevoCodigo = "CLI" + numero.ToString("D4"); // Formato con 4 dígitos
            }

            return nuevoCodigo;
        }
    }
}
