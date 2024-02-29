using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace JSHOP_Web.Modelo
{
    
    public class Usuario:IdentityUser
    {
      
        public virtual int CodigoUsuario { get; set; }
        
        public virtual string Nombre { get; set; }
      
        public virtual long IdSucursal { get; set; }
        public virtual Sucursal Sucursal { get; set; }

    }

    public class UsuarioVM
    {       
        public virtual int CodigoUsuario { get; set; }       
        public virtual string Nombre { get; set; }
        public string Password { get; set; }
        public string nombrerol { get; set; }
        public string correo { get; set; }
        public virtual long IdSucursal { get; set; }
        public virtual Sucursal Sucursal { get; set; }
    }

    public class Roles
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }       
        public virtual string NormalizedName { get; set; }


    }

}
