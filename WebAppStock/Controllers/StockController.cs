using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Negocio;
using CodigoComun.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppStock.ViewModels;

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
            var stockRepository = new StockRepository();
            var stockService = new StockService(stockRepository);
            var stocks = stockService.ObtenerTodosLosStocks();

            // Crear una lista vacía de StockDTO
            var stockDTOs = new List<StockDTO>();

            // Convertir cada objeto Stock en un objeto StockDTO
            foreach (var stock in stocks)
            {
                var stockDTO = new StockDTO
                {
                    Id = stock.Id,
                    IdArticulo = stock.IdArticulo,
                    IdDeposito = stock.IdDeposito,
                    Cantidad = stock.Cantidad
                };
                stockDTOs.Add(stockDTO);
            }

            var stockViewModels = new StockViewModels();

            // Obtener la lista de artículos y depósitos
            var articuloService = new ArticuloService();
            var depositoService = new DepositoService();
            var articulos = articuloService.ObtenerTodosLosArticulos();
            var depositos = depositoService.ObtenerTodosLosDepositos();

            // Convertir la lista de artículos y depósitos en una lista de ArticuloDTO y DepositoDTO
            var articuloDTOs = new List<ArticuloDTO>();
            var depositoDTOs = new List<DepositoDTO>();

            foreach (var articulo in articulos)
            {
                var articuloDTO = new ArticuloDTO
                {
                    Id = articulo.Id,
                    Nombre = articulo.Nombre,
                    Codigo = articulo.Codigo
                };
                articuloDTOs.Add(articuloDTO);
            }

            foreach (var deposito in depositos)
            {
                var depositoDTO = new DepositoDTO
                {
                    Id = deposito.Id,
                    Nombre = deposito.Nombre
                };
                depositoDTOs.Add(depositoDTO);
            }

            // Asignar la lista de ArticuloDTOs y DepositoDTOs a las propiedades del modelo
            stockViewModels.ArticulosList = articuloDTOs;
            stockViewModels.DepositosList = depositoDTOs;

            // Asignar la lista de StockDTOs a la propiedad StockDTOs
            stockViewModels.StockDTOs = stockDTOs;

            return View(stockViewModels);
        }








        [HttpGet]
        public IActionResult Create()
        {
            var stockViewModels = new StockViewModels();

            // Obtener la lista de artículos y depósitos
            var articuloService = new ArticuloService();
            var depositoService = new DepositoService();
            var articulos = articuloService.ObtenerTodosLosArticulos();
            var depositos = depositoService.ObtenerTodosLosDepositos();

            // Convertir la lista de artículos y depósitos en una lista de SelectListItems
            stockViewModels.selectArticulosList = new SelectList(articulos, "Id", "Nombre");
            stockViewModels.selectDepositosList = new SelectList(depositos, "Id", "Nombre");

            return View(stockViewModels);
        }

    
        [HttpPost]
        public IActionResult Create(StockViewModels stockViewModels)
        {
            var stockRepository = new StockRepository();
            var stockService = new StockService(stockRepository);

            // Inicializar el objeto StockDTO
            stockViewModels.StockDTO = new StockDTO();

            if (stockViewModels.StockDTO != null)
            {
                stockViewModels.StockDTO.IdArticulo = stockViewModels.SelectedArticulo;
                stockViewModels.StockDTO.IdDeposito = stockViewModels.SelectedDeposito;
                stockViewModels.StockDTO.Cantidad = stockViewModels.Cantidad; // Asignar la cantidad ingresada

                stockViewModels.StockDTO = stockService.AgregarStock(stockViewModels.StockDTO);

                // Actualizar el mínimo stock del Articulo
                var articuloService = new ArticuloService();
                var articulo = articuloService.GetArticuloPorId((int)stockViewModels.StockDTO.IdArticulo);
                if (articulo != null) // Verificar que el Articulo exista
                {
                    articulo.MinimoStock += stockViewModels.Cantidad; // Sumar la cantidad ingresada al mínimo stock
                    articuloService.ActualizarArticulo(articulo); // Guardar el Articulo actualizado en la base de datos
                }
            }




            if (stockViewModels.StockDTO.HuboError == false)
            {
                // hacer algo si no hubo error
                return RedirectToAction("Index", "Stock");
            }
            else
            {
                // hacer algo si hubo error
                var articuloService = new ArticuloService();
                var depositoService = new DepositoService();
                var articulos = articuloService.ObtenerTodosLosArticulos();
                var depositos = depositoService.ObtenerTodosLosDepositos();

                stockViewModels.selectArticulosList = new SelectList(articulos, "Id", "Nombre");
                stockViewModels.selectDepositosList = new SelectList(depositos, "Id", "Nombre");

                return View(stockViewModels);
            }
        }




    }
}

