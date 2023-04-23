using CodigoComun.Repository;
using Microsoft.AspNetCore.Mvc;

namespace WebAppStock.Controllers
{
    public class StockController : Controller
    {
        private readonly StockService _stockService;

        public StockController()
        {
            var stockRepository = new StockRepository();
            _stockService = new StockService(stockRepository);
        }

        public IActionResult Index()
        {
            var stocks = _stockService.ObtenerTodosLosStocks();
            return View(stocks);
        }
    }
}

