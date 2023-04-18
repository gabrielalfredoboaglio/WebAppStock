using CodigoComun.Modelos;
using CodigoComun.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace WebAppStock.Controllers
{
    public class ArticuloController : Controller
    {
        public IActionResult Index()
        {
            ArticuloService articuloService = new ArticuloService();
            var articulos = articuloService.ObtenerTodosLosArticulos();
            return View(articulos);
        }
    }
}
