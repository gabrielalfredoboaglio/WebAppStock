using Microsoft.AspNetCore.Mvc;

namespace WebAppStock.Controllers
{
    public class ArticuloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
