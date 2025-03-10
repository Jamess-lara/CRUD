using Microsoft.EntityFrameworkCore;
using Crud.DL.DataContext;
using Crud.DL.Repositorio;
using Crud.Models;
using Crud.BL.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbpruebasContext>(opciones=>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

builder.Services.AddScoped<IGenericRepository<Contacto>, ContactoRepository>();
builder.Services.AddScoped<IContactoService, ContactoService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
