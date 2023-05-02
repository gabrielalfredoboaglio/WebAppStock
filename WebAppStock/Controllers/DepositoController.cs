using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Models;
using CodigoComun.Negocio;
using CodigoComun.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAppStock.ViewModels;

namespace WebAppStock.Controllers
{
    public class DepositoController : Controller
    {
        private readonly DepositoService depositoRepository = new DepositoService();
        private readonly DepositoService depositoService = new DepositoService();
        public IActionResult Index()
        {
            var depositos = depositoRepository.ObtenerTodosLosDepositos();
            return View(depositos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var depositoAAgregar = new DepositoDTO();
            return View(depositoAAgregar);
        }

        [HttpPost]
        public IActionResult Create(DepositoDTO depositoDTOAguardar)
        {
            depositoDTOAguardar = depositoRepository.AgregarDeposito(depositoDTOAguardar);

            if (depositoDTOAguardar.HuboError == false)
            {
                ViewBag.Mensaje = depositoDTOAguardar.Mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = depositoDTOAguardar.Mensaje;
                return View(depositoDTOAguardar);
            }
        }



        [HttpGet]
        public IActionResult Edit(int idDepositoAGuardar)
        {
            DepositoDTO deposito = depositoService.ObtenerDepositoPorId(idDepositoAGuardar);
            if (deposito == null)
            {
                return NotFound();
            }

            return View(deposito);
        }


        [HttpPost]
        public IActionResult Edit(int id, DepositoDTO depositoActualizado)
        {
            if (id != depositoActualizado.Id)
            {
                return BadRequest();
            }

            var resultado = depositoService.ModificarDeposito(depositoActualizado);

            if (resultado != null)
            {
                TempData["Mensaje"] = "Depósito Actualizado";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo actualizar el depósito";
                return View(depositoActualizado);
            }
        }

        [HttpGet]
        public IActionResult Delete(int idDepositoEliminar)
        {
            var depositoAEliminar = depositoService.ObtenerDepositoPorId(idDepositoEliminar);

            if (depositoAEliminar == null)
            {
                return NotFound();
            }

            var resultado = depositoService.EliminarDeposito(idDepositoEliminar);

            if (resultado.Mensaje != "Depósito eliminado correctamente")
            {
                return BadRequest(resultado.Mensaje);
            }

            TempData["Mensaje"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Stock(int idDeposito)
        {
            StockRepository stockRepository = new StockRepository();
            StockService stockService = new StockService(stockRepository);
            var stocksDTO = stockService.ObtenerStockPorDeposito(idDeposito);

          
            ArticuloService articuloService = new ArticuloService();
            var articulosDTO = articuloService.ObtenerTodosLosArticulos();

            
            DepositoService depositoService = new DepositoService();
            var depositosDTO = depositoService.ObtenerTodosLosDepositos();

            var stockViewModels = new StockViewModels
            {
                StockDTOs = stocksDTO,
                ArticulosList = articulosDTO,
                DepositosList = depositosDTO
            };

            return View("Stock", stockViewModels);
        }
    }
}



