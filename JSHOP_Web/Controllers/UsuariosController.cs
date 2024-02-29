using JSHOP_Web.API;
using JSHOP_Web.Helper.ViewModels;
using JSHOP_Web.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JSHOP_Web.Controllers
{
    public class UsuariosController : Controller
    {
        private IServicios iservicios;
        public UsuariosController(IServicios servicios)
        {
            this.iservicios = servicios;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Usuarios/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var usuarios = await iservicios.ListaUsuarios(); //Lista de usuarios actual
                var sucursal = await iservicios.ListarSucursal(); //Lista de sucursales actual

              
                var list = new List<Usuario>(); //Lista nueva (vacía)
                foreach (var item in usuarios)
                {
                    item.Sucursal = sucursal.Where(x=>x.Id==item.IdSucursal).FirstOrDefault(); //Añade la sucursal por su Id
                    list.Add(item); //Llena la lista nueva, añadiendo la sucursal 
                }

                return Json(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IActionResult Create()
        {
            var listasucursal = iservicios.ListarSucursal();
            ViewBag.sucursal = new SelectList(listasucursal.Result.ToList(), "Id", "Nombre");
            var roles = iservicios.ListaRoles();
            ViewBag.roles = new SelectList(roles.Result.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create(UsuarioVM usuario)
        {
            try
            {

                var Crear = await iservicios.CrearUsuario(usuario);
                if (Crear == Respuestas.Bueno)
                {
                    return Json(new { success = true, mensaje = "Creado con éxito" });
                }
                return Json(new { success = false, mensaje = "No se pudo crear" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> ActualizarContraseña(string id, string contraseña)
        {
            try
            {

                var Crear = await iservicios.ActualizarContraseña(id, contraseña);
                if (Crear == Respuestas.Bueno)
                {
                    return Json(new { success = true, mensaje = "Actualizado con éxito" });
                }
                return Json(new { success = false, mensaje = "No se pudo crear" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }
    }
}
