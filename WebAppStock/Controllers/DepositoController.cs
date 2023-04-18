using CodigoComun.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace WebAppStock.Controllers
{
    public class DepositoController : Controller
    {
        public IActionResult Index()
        {
            DepositoService depositoService = new DepositoService();
            var depositos = depositoService.ObtenerTodosLosDepositos();
            return View(depositos);
        }
    }
}

