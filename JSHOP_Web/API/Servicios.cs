using JSHOP_Web.Helper.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using JSHOP_Web.Modelo;
using System.Text;
using Humanizer.Localisation.TimeToClockNotation;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Http;

namespace JSHOP_Web.API
{
    public class Servicios : IServicios
    {
        private static string urlAPI = "https://localhost:7011";//url de la API
        SignToken sign = new SignToken(urlAPI); 
        HttpClient Consume = new HttpClient();

        public HttpClient getback()
        {
            if (Consume.BaseAddress == null)
            {
                Consume.BaseAddress = new Uri(urlAPI);
            }
            Consume.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sign.getToken.AccessToken);

            return Consume;
        }
        #region sesion
        public async Task<Respuestas> InicioSesion(string usuario, string contraseña)
        {
            try
            {
                sign.IniciarSesion(usuario, contraseña);
                Consume.BaseAddress = new Uri(urlAPI);
                Consume.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sign.getToken.AccessToken);
                if(sign.getToken.msj!=null) //Si trae el mensaje de excepción
                {
                    return Respuestas.Malo;
                  
                }else
                if(sign.getToken.AccessToken != null) 
                {
                    return Respuestas.Bueno;//Obtuvo el token de la sesión correctamente
                }
                return Respuestas.Error;
            }
            catch {
                throw new Exception("Error de inicio de sesión"); //No se estableció la conexión
            }
        }
        public async Task<UserAuthenticated> getuser()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("usuario/GetUser");
                var r = response.Content.ReadAsStringAsync().Result;
               
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var user = JsonConvert.DeserializeObject<UserAuthenticated>(r);                    
                    return user;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Producto
        public async Task<List<Producto>> ListaProdcutos()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Producto/ListarProductos");
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var productos = JsonConvert.DeserializeObject<List<Producto>>(r);
                    return productos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Producto> BuscarProducto(long id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Producto/BuscarProducto?id="+id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var producto = JsonConvert.DeserializeObject<Producto>(r);
                    return producto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> CrearProducto(Producto producto)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(producto);
                var response = await Consume.PostAsync("Producto/AgregarProducto", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString())==200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Respuestas> EditarProducto(Producto producto)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(producto);
                var response = await Consume.PostAsync("Producto/EditarProducto", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> DeshabilitarProducto(long id)
        {
            try
            {
                if (Consume.BaseAddress == null)
                {
                    Consume.BaseAddress = new Uri(urlAPI);
                    Consume.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", sign.getToken.AccessToken);

                }

                var response = await Consume.GetAsync("Producto/DeshabilitarProducto?IdPrd=" + id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> DescontarProducto(long id, int cantidad)
        {
            try
            {
                getback(); 
                var response = await Consume.GetAsync("Producto/DescontarProducto?IdProducto=" + id+ "&Cantidad="+cantidad);
                var r = response.Content.ReadAsStringAsync().Result;
                JObject jobj = JObject.Parse(r);
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(jobj["mensaje"].ToString());
                }
                else
                {
                   
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Sucursal
        public async Task<List<Sucursal>> ListarSucursal()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Sucursal/ListarSucursal");
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var sucursals = JsonConvert.DeserializeObject<List<Sucursal>>(r);
                    return sucursals;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Sucursal> BuscarSucursal(int id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Sucursal/ListarSucursal?id=" + id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var sucursal = JsonConvert.DeserializeObject<Sucursal>(r);
                    return sucursal;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> CrearSucursal(Sucursal sucursal)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(sucursal);
                var response = await Consume.PostAsync("Sucursal/AgregarSucursal", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Respuestas> EditarSucursal(Sucursal sucursal)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(sucursal);
                var response = await Consume.PostAsync("Sucursal/EditarSucursal", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> DeshabilitarSucursal(long id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Sucursal/DeshabilitarSucursal?IdSucur=" + id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Categoria
        public async Task<List<Categoria>> ListarCategoria()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Categoria/ListarCategoria");
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var categorias = JsonConvert.DeserializeObject<List<Categoria>>(r);
                    return categorias;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Categoria> BuscarCategoria(int id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Categoria/ListarCategoria?id="+id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var categoria = JsonConvert.DeserializeObject<Categoria>(r);
                    return categoria;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> CrearCategoria(Categoria categoria)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(categoria);
                var response = await Consume.PostAsync("Categoria/AgregarCategoria", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Respuestas> EditarCategoria(Categoria categoria)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(categoria);
                var response = await Consume.PostAsync("Categoria/EditarCategoria", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> DeshabilitarCategoria(long id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Categoria/DeshabilitarCategoria?IdSucur=" + id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Proveedor
        public async Task<List<Proveedor>> ListarProveedor()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Proveedor/ListarProveedor");
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var proveedors = JsonConvert.DeserializeObject<List<Proveedor>>(r);
                    return proveedors;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Proveedor> BuscarProveedor(int id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Proveedor/ListarProveedor?id=" + id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var Proveedor = JsonConvert.DeserializeObject<Proveedor>(r);
                    return Proveedor;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> CrearProveedor(Proveedor Proveedor)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(Proveedor);
                var response = await Consume.PostAsync("Proveedor/AgregarProveedor", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Respuestas> EditarProveedor(Proveedor Proveedor)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(Proveedor);
                var response = await Consume.PostAsync("Proveedor/EditarProveedor", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> DeshabilitarProveedor(long id)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("Proveedor/DeshabilitarProveedor?IdProv=" + id);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Usuarios
        public async Task<List<Usuario>> ListaUsuarios()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("usuario/ListaUsuarios");
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(r);
                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Roles>> ListaRoles()
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("usuario/GetRoles");
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    var roles = JsonConvert.DeserializeObject<List<Roles>>(r);
                    return roles;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Respuestas> CrearUsuario(UsuarioVM usuario)
        {
            try
            {
                getback();
                var obj = JsonConvert.SerializeObject(usuario);
                var response = await Consume.PostAsync("usuario/CrearUsuario", new StringContent(obj, Encoding.Unicode, "application/json"));
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> ActualizarContraseña(string id, string contraseña)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("usuario/ActualizarContraseña?id=" + id+ "&Contraseña="+ contraseña);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Respuestas> CreateRol(string rol)
        {
            try
            {
                getback();
                var response = await Consume.GetAsync("usuario/CreateRol" + rol);
                var r = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new Exception("Error en servidor remoto: " + response.StatusCode + "\n" + response + "\n" + r);
                }
                else
                {
                    JObject jobj = JObject.Parse(r);
                    if (int.Parse(jobj["estado"].ToString()) == 200)
                    {
                        return Respuestas.Bueno;
                    }
                    else { return Respuestas.Malo; }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
