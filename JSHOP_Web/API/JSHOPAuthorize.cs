using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using JSHOP_Web.Helper.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace JSHOP_Web.API
{
    public class JSHOPAuthorize : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext filtercontext)
        {
            try
            {
                var Servicios = (IServicios)filtercontext.HttpContext.RequestServices.GetService(typeof(IServicios));   // Inyección directa
                var Tarea = Task.Run<UserAuthenticated>(() => Servicios.getuser());
                Tarea.Wait();

                var User = Tarea.Result;
                if (User != null)
                {
                    if (!User.IsAuthenticated)
                    {
                        // Redirige a Home/Index
                        filtercontext.Result = new RedirectToActionResult("Index", "Home", null);
                    }
                    filtercontext.HttpContext.Session.SetString("rol", User.Role);
                }
                else
                {
                    filtercontext.Result = new RedirectToActionResult("Index", "Home", null);
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                filtercontext.Result = new RedirectToActionResult("Error", "Home", null);
            }
        }

    }
}
