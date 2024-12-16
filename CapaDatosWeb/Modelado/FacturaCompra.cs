namespace CapaDatosWeb.Modelado
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class FacturaCompra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FacturaCompra()
        {
            this.DetalleCompra = new HashSet<DetalleCompra>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Código es obligatorio.")]
        [StringLength(50, ErrorMessage = "El Código no puede exceder los 50 caracteres.")]
        public string Codigo { get; set; }

        [StringLength(200, ErrorMessage = "La Descripción no puede exceder los 200 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Monto de Compra es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El Monto de Compra debe ser mayor o igual a 0.")]
        public Nullable<decimal> MontoCompra { get; set; }

        [Required(ErrorMessage = "El campo ProveedorId es obligatorio.")]
        public Nullable<int> ProveedorId { get; set; }

        [Required(ErrorMessage = "El campo UsuarioId es obligatorio.")]
        public Nullable<int> UsuarioId { get; set; }

        [Required(ErrorMessage = "El campo Fecha de Factura es obligatorio.")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la Fecha de Factura no es válido.")]
        public Nullable<System.DateTime> FechaFactura { get; set; }

        [Required(ErrorMessage = "El campo Tipo de Entrada es obligatorio.")]
        public Nullable<int> TipoEntradaId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleCompra> DetalleCompra { get; set; }

        public virtual Proveedor Proveedor { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
