using JSHOP_Web.Helper.ViewModels;
using JSHOP_Web.Modelo;
using System.Diagnostics.Eventing.Reader;

namespace JSHOP_Web.API
{
    public interface IServicios
    {
        #region sesion
        Task<Respuestas> InicioSesion(string usuario, string contraseña);
        Task<UserAuthenticated> getuser();
        #endregion
        #region Productos
        Task<List<Producto>> ListaProdcutos();
        Task<Producto> BuscarProducto(long id);
        Task<Respuestas> CrearProducto(Producto producto);
        Task<Respuestas> EditarProducto(Producto producto);
        Task<Respuestas> DeshabilitarProducto(long id);
        Task<Respuestas> DescontarProducto(long id, int cantidad);
        #endregion
        #region Sucursal
        Task<List<Sucursal>> ListarSucursal();
        Task<Sucursal> BuscarSucursal(int id);
        Task<Respuestas> CrearSucursal(Sucursal sucursal);
        Task<Respuestas> EditarSucursal(Sucursal sucursal);
        Task<Respuestas> DeshabilitarSucursal(long id);
        #endregion

        #region Categoria
        Task<List<Categoria>> ListarCategoria();
        Task<Categoria> BuscarCategoria(int id);
        Task<Respuestas> CrearCategoria(Categoria categoria);
        Task<Respuestas> EditarCategoria(Categoria categoria);
        Task<Respuestas> DeshabilitarCategoria(long id);
        #endregion

        #region Proveedor
        Task<List<Proveedor>> ListarProveedor();
        Task<Proveedor> BuscarProveedor(int id);
        Task<Respuestas> CrearProveedor(Proveedor Proveedor);
        Task<Respuestas> EditarProveedor(Proveedor Proveedor);
        Task<Respuestas> DeshabilitarProveedor(long id);
        #endregion

        #region Usuario
        Task<List<Usuario>> ListaUsuarios();
        Task<List<Roles>> ListaRoles();
        Task<Respuestas> CrearUsuario(UsuarioVM usuario);
        Task<Respuestas> ActualizarContraseña(string id, string contraseña);
        Task<Respuestas> CreateRol(string rol);
        #endregion
    }
}
