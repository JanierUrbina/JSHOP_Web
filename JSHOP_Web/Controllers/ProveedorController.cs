using JSHOP_Web.API;
using JSHOP_Web.Helper.ViewModels;
using JSHOP_Web.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JSHOP_Web.Controllers
{
    public class ProveedorController : Controller
    {
        private IServicios iservicios;
        public ProveedorController(IServicios servicios)
        {
            this.iservicios = servicios;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Proveedor/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var lp = await iservicios.ListarProveedor();
                var listaProveedores = from p in lp where !p.Estado select p; //Solo tomar los estados falsos (Habilitado)
                return Json(listaProveedores);
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
        public async Task<JsonResult> Create(Proveedor proveedor)
        {
            try
            {

                var Crear = await iservicios.CrearProveedor(proveedor);
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
                var cate = await iservicios.DeshabilitarProveedor(id);

                if (cate == Respuestas.Bueno)//Se deshabilitó correctamente
                {
                    return Json(new { success = true, message = "Se deshabilitó correctamente" });

                }
                else
                {
                    return Json(new { success = false, message = "Error al deshabilitar este Proveedor." });

                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            var proveedor = await iservicios.BuscarProveedor((int)id);
            HttpContext.Session.SetString("idProveedor", proveedor.Id.ToString());
            var p = new Proveedor
            {
                Nombre = proveedor.Nombre,
                Descripcion = proveedor.Descripcion
            };
            return View(p);
        }
        [HttpPost]
        public async Task<JsonResult> Edit(Proveedor proveedor)
        {
            try
            {
                long id = long.Parse((HttpContext.Session.GetString("idProveedor") as string));
                var p = new Proveedor
                {
                    Id = id,
                    Nombre = proveedor.Nombre,
                    Descripcion = proveedor.Descripcion
                };
                var edit = await iservicios.EditarProveedor(p);
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
