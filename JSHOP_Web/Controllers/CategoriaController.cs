using JSHOP_Web.API;
using JSHOP_Web.Helper.ViewModels;
using JSHOP_Web.Modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Net;
using System.Reflection;
using System.Text;

namespace JSHOP_Web.Controllers
{
    [JSHOPAuthorize]
    public class CategoriaController : Controller
    {
        private IServicios iservicios;
        public CategoriaController(IServicios servicios)
        {
            this.iservicios = servicios;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Categoria/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var lc = await iservicios.ListarCategoria();
                var listaCategorias = from c in lc where !c.Estado select c; //Solo tomar los estados falsos (Habilitado)
                return Json(listaCategorias);
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
        public async Task<JsonResult> Create(Categoria categoria)
        {
            try
            {
               
                var Crear = await iservicios.CrearCategoria(categoria);
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
                var cate = await iservicios.DeshabilitarCategoria(id);

                if (cate == Respuestas.Bueno)//Se deshabilitó correctamente
                {
                    return Json(new { success = true, message = "Se deshabilitó correctamente" });

                }
                else
                {
                    return Json(new { success = false, message = "Error al deshabilitar esta categoria." });

                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message});
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            var categoria = await iservicios.BuscarCategoria((int)id);           
            HttpContext.Session.SetString("idCategoria", categoria.Id.ToString());
            var p = new Categoria
            {
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };
            return View(p);
        }
        [HttpPost]
        public async Task<JsonResult> Edit(Categoria categoria)
        {
            try
            {
                long id = long.Parse((HttpContext.Session.GetString("idCategoria") as string));
                var c = new Categoria
                {
                    Id = id,
                    Nombre = categoria.Nombre,
                    Descripcion = categoria.Descripcion
                };
                var edit = await iservicios.EditarCategoria(c);
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
