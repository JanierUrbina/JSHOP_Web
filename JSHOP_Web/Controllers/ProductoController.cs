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
    public class ProductoController : Controller
    {
        private IServicios iservicios;
        public ProductoController(IServicios servicios)
        {
            this.iservicios = servicios;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("Producto/IndexJSON")]
        public async Task<JsonResult> IndexJSON()
        {
            try
            {
                var lp = await iservicios.ListaProdcutos();
                var listaProdcutos = from p in lp where !p.Estado select p; //Solo tomar los estados falsos (Habilitado)
                return Json(listaProdcutos);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        public IActionResult Create()
        {
            var listasucursal = iservicios.ListarSucursal();
            var listacategoria = iservicios.ListarCategoria();
            var listaproveedor = iservicios.ListarProveedor();

            ViewBag.sucursal =  new SelectList(listasucursal.Result.ToList(),"Id","Nombre");
            ViewBag.proveedor = new SelectList(listaproveedor.Result.ToList(),"Id","Nombre");
            ViewBag.categoria = new SelectList(listacategoria.Result.ToList(), "Id", "Nombre");
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Create(ProductoVM producto)
        {
            try
            {
                string ruta = "";
                string guid = null;
                if (producto.Img != null)
                {
                    string filePath = @"C:\\Imagenes";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);//Si no existe, crea la ruta
                    }
                    
                   
                    guid = Guid.NewGuid().ToString() + producto.Img.FileName;
                    ruta = Path.Combine(filePath, guid); //Creamos la ruta completa
                    producto.Img.CopyTo(new FileStream(ruta, FileMode.Create)); //Guardamos la imagen en la ruta
                }
               
                var p = new Producto
                {
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Marca = producto.Marca,
                    Precio = producto.Precio,
                    SucursalId = producto.SucursalId,
                    ProveedorId = producto.ProveedorId,
                    Cantidad = producto.Cantidad,
                    CategoriaId = producto.CategoriaId,
                    RutaImagen = ruta                
                };
                //p.Categoria = iservicios.ListarCategoria(Convert.ToInt16(p.CategoriaId)).Result.FirstOrDefault();
                var Crear = await iservicios.CrearProducto(p);
               
                if (Crear==Respuestas.Bueno)
                {
                    return Json(new{success=true, mensaje = "Creado con éxito" });
                }
                return Json(new { success = false, mensaje = "No se pudo crear" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: "+ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Deshabilitar([FromForm] long id)
        {
            try
            {
                var producto = await iservicios.DeshabilitarProducto(id);
                
                if (producto == Respuestas.Bueno)//Se deshabilitó correctamente
                {
                    return Json(new { success = true, message = "Se deshabilitó correctamente" });

                }
                else
                {
                    return Json(new { success = false, message = "Error al deshabilitar este producto." });

                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "deshabilitar este producto." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ObtenerImagenModal([FromForm] long id)
        {
            try
            {
                var producto = await iservicios.BuscarProducto(id);
               
                using (var fileStream = new FileStream(producto.RutaImagen, FileMode.Open, FileAccess.Read))
                {
                    //genera bytes apartir de ruta
                    var bytes = System.IO.File.ReadAllBytes(producto.RutaImagen);
                    string archivoruta = Convert.ToBase64String(bytes);
                    //Retorna el base64 de la imagen
                    return Json(new { imagen = archivoruta});
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cargar imagen, vuelva a intentarlo más tarde." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DescontarProducto(long id, int cantidad)
        {
            try
            {
                var producto = await iservicios.DescontarProducto(id, cantidad);
                if(producto == Respuestas.Bueno)
                {
                    return Json(new { success = true, message = "Se descontó este producto de stock" });
                }
                return Json(new { success = false, message = "No se descontó este producto de stock" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message});
            }
        }

        public async Task<IActionResult> Edit(long id)
        {
            var listasucursal = iservicios.ListarSucursal();
            var listacategoria = iservicios.ListarCategoria();
            var listaproveedor = iservicios.ListarProveedor();

            ViewBag.sucursal = new SelectList(listasucursal.Result.ToList(), "Id", "Nombre");
            ViewBag.proveedor = new SelectList(listaproveedor.Result.ToList(), "Id", "Nombre");
            ViewBag.categoria = new SelectList(listacategoria.Result.ToList(), "Id", "Nombre");
            var producto = await iservicios.BuscarProducto(id);
            HttpContext.Session.SetString("ruta", producto.RutaImagen);
            HttpContext.Session.SetString("id", producto.Id.ToString());
            var p = new ProductoVM
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Marca = producto.Marca,
                Precio = producto.Precio,
                SucursalId = producto.SucursalId,
                ProveedorId = producto.ProveedorId,
                Cantidad = producto.Cantidad,
                CategoriaId = producto.CategoriaId               
            };
            return View(p);
        }
        [HttpPost]
        public async Task<JsonResult> Edit(ProductoVM producto)
        {
            try
            {
                string ruta = "";
                string guid = null;
                var rutasesion = HttpContext.Session.GetString("ruta") as string;
                if (producto.Img != null)
                {
                    if (System.IO.File.Exists(rutasesion)) // verifica si el archivo existe en la ruta especificada
                    {
                        System.IO.File.Delete(rutasesion); // elimina el archivo
                    }


                    string filePath = @"C:\\Imagenes";
                    guid = Guid.NewGuid().ToString() + producto.Img.FileName;
                    ruta = Path.Combine(filePath, guid);
                    producto.Img.CopyTo(new FileStream(ruta, FileMode.Create));
                }
                else
                {
                    ruta = rutasesion;
                }
                long id = long.Parse((HttpContext.Session.GetString("id") as string));
                var p = new Producto
                {
                    Id = id,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Marca = producto.Marca,
                    Precio = producto.Precio,
                    SucursalId = producto.SucursalId,
                    ProveedorId = producto.ProveedorId,
                    Cantidad = producto.Cantidad,
                    CategoriaId = producto.CategoriaId,
                    RutaImagen = ruta
                };
                var edit = await iservicios.EditarProducto(p);
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
