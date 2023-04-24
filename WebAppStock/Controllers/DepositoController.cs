using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Models;
using CodigoComun.Negocio;
using CodigoComun.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAppStock.Controllers
{
    public class DepositoController : Controller
    {
        private readonly DepositoService depositoRepository = new DepositoService();

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





        public IActionResult Details(int depositoId)
        {
            var deposito = depositoRepository.ObtenerDepositoPorId(depositoId);

            if (deposito == null)
            {
                return NotFound();
            }

            return View(deposito);
        }

        [HttpGet]
        public IActionResult Edit(int idDepositoAModificar)
        {
            Deposito depositoAActualizar = depositoRepository.ObtenerDepositoPorId(idDepositoAModificar);
            if (depositoAActualizar == null)
            {
                return NotFound();
            }
            return View(depositoAActualizar);
        }

        [HttpPost]
        public IActionResult Edit(Deposito depositoAModificar)
        {
            int resultado = depositoRepository.ModificarDeposito(depositoAModificar);

            if (resultado == 1)
            {
                TempData["Mensaje"] = "Depósito modificado correctamente";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Mensaje = "Error al modificar el depósito";
                return View(depositoAModificar);
            }
        }


        [HttpGet]
        public IActionResult Delete(int depositoId)
        {
            try
            {
                var deposito = depositoRepository.ObtenerDepositoPorId(depositoId);

                if (deposito == null)
                {
                    return NotFound();
                }

                var resultado = depositoRepository.EliminarDeposito(depositoId).ToString();
                if (resultado.Equals("Depósito eliminado correctamente"))
                {
                    return RedirectToAction("Index");
                }
                else if (resultado.Equals("0"))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest("Error al eliminar el depósito");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}


