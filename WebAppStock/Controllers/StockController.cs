using Microsoft.AspNetCore.Mvc;

using CodigoComun.Models;
using CodigoComun.Repository;

namespace WebAppStock.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            StockRepository stockRepository = new StockRepository();
            StockService stockServices = new StockService(stockRepository);
            List<Stock> stocksDeLaBaseDeDatos = stockServices.ObtenerTodosLosStocks();
            return View(stocksDeLaBaseDeDatos);
        }

    }
}
