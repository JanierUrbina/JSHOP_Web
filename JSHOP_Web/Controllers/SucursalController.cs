using JSHOP_Web.API;
using JSHOP_Web.Helper.ViewModels;
using JSHOP_Web.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JSHOP_Web.Controllers
{
    [JSHOPAuthorize]
    public class SucursalController : Controller
    {
        private IServicios iservicios;
        public SucursalController(IServicios servicios)
        {
            this.iservicios = servicios;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Sucursal/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var ls = await iservicios.ListarSucursal();
                var listaSucursal = from s in ls where !s.Estado select s; //Solo tomar los estados falsos (Habilitado)
                return Json(listaSucursal);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create(Sucursal sucursal)
        {
            try
            {

                var Crear = await iservicios.CrearSucursal(sucursal);
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
        public async Task<JsonResult> Deshabilitar([FromForm] long id)
        {
            try
            {
                var cate = await iservicios.DeshabilitarSucursal(id);

                if (cate == Respuestas.Bueno)//Se deshabilitó correctamente
                {
                    return Json(new { success = true, message = "Se deshabilitó correctamente" });

                }
                else
                {
                    return Json(new { success = false, message = "Error al deshabilitar esta Sucursal." });

                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            var su = await iservicios.BuscarSucursal((int)id);
            HttpContext.Session.SetString("idSucursal", su.Id.ToString());
            var p = new Sucursal
            {
                Nombre = su.Nombre,
                Descripcion = su.Descripcion,
                Dirección = su.Dirección
            };
            return View(p);
        }
        [HttpPost]
        public async Task<JsonResult> Edit(Sucursal sucursal)
        {
            try
            {
                long id = long.Parse((HttpContext.Session.GetString("idSucursal") as string));
                var s = new Sucursal
                {
                    Id = id,
                    Nombre = sucursal.Nombre,
                    Descripcion = sucursal.Descripcion,
                    Dirección = sucursal.Dirección
                };
                var edit = await iservicios.EditarSucursal(s);
                if (edit == Respuestas.Bueno)
                {
                    return Json(new { success = true, mensaje = "Editado con éxito" });
                }
                return Json(new { success = false, mensaje = "No se pudo editar" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }
    }
}
