//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaDatosWeb.Modelado
{
    using System;
    using System.Collections.Generic;
    
    public partial class MovimientoInventario
    {
        public int Id { get; set; }
        public Nullable<int> ProductoId { get; set; }
        public string TipoMovimiento { get; set; }
        public int Cantidad { get; set; }
        public Nullable<System.DateTime> FechaMovimiento { get; set; }
    
        public virtual Producto Producto { get; set; }
    }
}
