using System.ComponentModel.DataAnnotations.Schema;

namespace JSHOP_Web.Modelo
{
   
    public partial class Producto : EntidadBase
    {
      
        public virtual string? Nombre { get; set; }
       
        public virtual string? Descripcion { get; set; }

       
        public virtual int Cantidad { get; set; }

       
        public virtual string? Marca { get; set; }

       
        public virtual float Precio { get; set; }

     
        public virtual string RutaImagen { get; set; }

        public virtual long CategoriaId { get; set; }
        public virtual long SucursalId { get; set; }
        public virtual long ProveedorId { get; set; }


        
        public virtual Categoria Categoria { get; set; }
       
        public virtual Sucursal Sucursal { get; set; }
        
        public virtual Proveedor Proveedor { get; set; }
    }
}
