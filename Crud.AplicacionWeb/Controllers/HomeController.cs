using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crud.AplicacionWeb.Models;
using Crud.BL.Service;
using Crud.Models;
using Crud.AplicacionWeb.Models.ViewModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Crud.AplicacionWeb.Controllers;

public class HomeController : Controller
{
    private readonly IContactoService _contactoService;

    public HomeController(IContactoService contactoServ)
    {
        _contactoService = contactoServ;
    }


    public IActionResult Index()
    {
        return View();
    }

    //Este es el metodo para listar
    [HttpGet]
    public async Task<IActionResult> Lista()
    {
        var queryContactoSQL = await _contactoService.ObtenerTodos();

        List<VMContacto> lista = queryContactoSQL
                                 .Select(c => new VMContacto(){
                                   IdContacto = c.IdContacto,
                                   Nombre = c.Nombre,
                                   Telefono = c.Telefono,
                                   FechaNacimiento = c.FechaNacimiento.Value.ToString()
                                 }).ToList();

        return StatusCode(StatusCodes.Status200OK, lista);
    }

    //Este es el metodo para Insertar
    [HttpPost]
    public async Task<IActionResult> Insertar([FromBody] VMContacto modelo)
    {
        Contacto NuevoModelo = new Contacto()
        {
            Nombre = modelo.Nombre,
            Telefono = modelo.Telefono,
            FechaNacimiento = DateTime.ParseExact(modelo.FechaNacimiento,"dd/MM/yyyy",CultureInfo.CreateSpecificCulture("es-PE"))
        };
        bool respuesta = await _contactoService.Insertar(NuevoModelo);

        return StatusCode(StatusCodes.Status200OK, new {valor = respuesta});
    }

    //Este es el metodo para Actualizar
    [HttpPut]
    public async Task<IActionResult> Actualizar([FromBody] VMContacto modelo)
    {
        Contacto NuevoModelo = new Contacto()
        {
            IdContacto = modelo.IdContacto,
            Nombre = modelo.Nombre,
            Telefono = modelo.Telefono,
            FechaNacimiento = DateTime.ParseExact(modelo.FechaNacimiento, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-PE"))
        };
        bool respuesta = await _contactoService.Actualizar(NuevoModelo);

        return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
    }


    //Este es el metodo para Eliminar
    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id)
    {
        bool respuesta = await _contactoService.Eliminar(id);

        return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
