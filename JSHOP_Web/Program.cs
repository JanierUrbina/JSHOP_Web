using JSHOP_Web.API;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IServicios, Servicios>();
builder.Services.AddScoped<JSHOPAuthorize>();

builder.Services.AddSession();
builder.Services.AddAuthentication();
builder.Services.AddAuthentication();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //Registranos el IHttpContextAccessor
//builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); //Establece Home/Index como la primera vista en aparecer
    endpoints.MapRazorPages();
});

app.Run();
