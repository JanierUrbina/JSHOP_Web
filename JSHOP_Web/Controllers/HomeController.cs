using JSHOP_Web.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JSHOP_Web.Controllers
{
    public class HomeController : Controller
    {
        public IServicios _iservicios;
        public HomeController(IServicios servicios) { _iservicios = servicios; }

        public IActionResult Index()
        {
            ViewBag.msj = null;
            var msj = HttpContext.Session.GetString("wrong");
            if (msj!=null)//Hay mensaje de credemciales incorrectas 
            {
                ViewBag.msj = msj;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string contraseña)
        {
            try
            {
                if(await _iservicios.InicioSesion(usuario, contraseña) == Helper.ViewModels.Respuestas.Bueno)
                {
                    return RedirectToAction("menu");
                }
                else
                {
                    HttpContext.Session.SetString("wrong","Credenciales inválidas.");
                    return RedirectToAction("Index");
                }

            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
         
        }
      
        [JSHOPAuthorize]
        [Route("menu")]        
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
