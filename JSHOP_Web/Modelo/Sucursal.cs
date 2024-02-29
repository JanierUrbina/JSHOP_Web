using System.ComponentModel.DataAnnotations.Schema;

namespace JSHOP_Web.Modelo
{
    
    public partial class Sucursal : EntidadBase
    {
      
       
        public string Nombre { get; set; }

       
        public string Dirección { get; set; }
       
        
        public string Descripcion { get; set; }
       
    }
}
