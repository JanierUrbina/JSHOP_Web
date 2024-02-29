using System.ComponentModel.DataAnnotations.Schema;

namespace JSHOP_Web.Modelo
{
    public class EntidadBase
    {
       
        public virtual long Id { get; set; }
      
        public virtual DateTime FechaCreacion { get; set; }
       
        public virtual bool Estado { get; set; }
    }
}
