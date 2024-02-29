using JSHOP_Web.Modelo;

namespace JSHOP_Web.Helper.ViewModels
{
    public class ProductoVM
    {
        public virtual string? Nombre { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual bool Estado { get; set; }

        public virtual string? Descripcion { get; set; }


        public virtual int Cantidad { get; set; }


        public virtual string? Marca { get; set; }


        public virtual float Precio { get; set; }


        public virtual IFormFile Img { get; set; }

        public virtual long CategoriaId { get; set; }
        public virtual long SucursalId { get; set; }
        public virtual long ProveedorId { get; set; }

       

    }
}
