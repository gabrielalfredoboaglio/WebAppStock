using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Models;
using CodigoComun.Negocio;
using CodigoComun.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppStock.ViewModels;

namespace WebAppStock.Controllers
{
    public class StockController : Controller
    {
        private readonly StockService _stockRepository;

        public StockController()
        {
            var stockRepository = new StockRepository();
            _stockRepository = new StockService(stockRepository);
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
            try
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
            catch (Exception)
            {
                return BadRequest("Error! El stock de ese articulo y deposito ya existe");
            }
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var stock = _stockRepository.ObtenerStockPorId(id);

                if (stock == null)
                {
                    return NotFound();
                }

                var resultado = _stockRepository.EliminarStock(id);
                if (resultado.Mensaje == "Stock eliminado correctamente")
                {
                    return RedirectToAction("Index");
                }

                else if (resultado.Equals("0"))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest("Error al eliminar el stock");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var stockRepository = new StockRepository();
            var stockService = new StockService(stockRepository);

            var stockDTO = stockService.ObtenerStockPorId(id);

            if (stockDTO == null)
            {
                return NotFound();
            }

            var stockViewModels = new StockViewModels
            {
                Id = stockDTO.Id,
                SelectedArticulo = (int)stockDTO.IdArticulo,
                SelectedDeposito = (int)stockDTO.IdDeposito,
                Cantidad = (decimal)stockDTO.Cantidad
            };

            // Obtener la lista de artículos y depósitos
            var articuloService = new ArticuloService();
            var depositoService = new DepositoService();
            var articulos = articuloService.ObtenerTodosLosArticulos();
            var depositos = depositoService.ObtenerTodosLosDepositos();

            // Convertir la lista de artículos y depósitos en una lista de SelectListItems
            stockViewModels.selectArticulosList = new SelectList(articulos, "Id", "Nombre");
            stockViewModels.selectDepositosList = new SelectList(depositos, "Id", "Nombre");

            // Obtener los objetos ArticuloDTO y DepositoDTO del stockDTO
            var articuloDTO = articuloService.GetArticuloPorId((int)stockDTO.IdArticulo);
            var depositoDTO = depositoService.ObtenerDepositoPorId((int)stockDTO.IdDeposito);

            // Asignar los objetos ArticuloDTO y DepositoDTO a la vista
            ViewData["Articulo"] = articuloDTO;
            ViewData["Deposito"] = depositoDTO;

            return View(stockViewModels);
        }


        [HttpPost]
        public IActionResult Edit(StockViewModels stockViewModels)
        {
            var stockRepository = new StockRepository();
            var stockService = new StockService(stockRepository);

            // Obtener el objeto StockDTO de la base de datos usando su Id
            var stockDTO = stockService.ObtenerStockPorId(stockViewModels.Id);

            if (stockDTO != null)
            {
                stockDTO.Cantidad = stockViewModels.Cantidad; // Actualizar la cantidad ingresada
                stockService.ActualizarStock(stockDTO); // Guardar el objeto StockDTO actualizado en la base de datos
            }

            // Redirigir al Index de Stock
            return RedirectToAction("Index", "Stock");
        }

    }
}
    






