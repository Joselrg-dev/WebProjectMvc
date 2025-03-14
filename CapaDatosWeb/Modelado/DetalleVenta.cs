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
    using System.ComponentModel.DataAnnotations;

    public partial class DetalleVenta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo FacturaVentaId es obligatorio.")]
        public Nullable<int> FacturaVentaId { get; set; }

        [Required(ErrorMessage = "El campo ProductoId es obligatorio.")]
        public Nullable<int> ProductoId { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "La Cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El campo PrecioVenta es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Precio de Venta debe ser mayor a 0.")]
        public decimal PrecioVenta { get; set; }

        [Required(ErrorMessage = "El campo Subtotal es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Subtotal debe ser mayor a 0.")]
        public decimal Subtotal { get; set; }
    
        public virtual FacturaVenta FacturaVenta { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
