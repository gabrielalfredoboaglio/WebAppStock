using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

namespace WebAppStock.Controllers
{
    public class ArticuloController : Controller
    {
        ArticuloService articuloService= new ArticuloService();
        public IActionResult Index()
        {

            var articulos = articuloService.ObtenerTodosLosArticulos();
            return View(articulos);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var articuloAAgregar = new ArticuloDTO();
            return View(articuloAAgregar);
        }

        [HttpPost]
        public IActionResult Create(ArticuloDTO articuloDTOAguardar)
        {
            articuloDTOAguardar = articuloService.AgregarArticulo(articuloDTOAguardar);

            if (articuloDTOAguardar.HuboError == false)
            {
                ViewBag.Mensaje = articuloDTOAguardar.Mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = articuloDTOAguardar.Mensaje;
                return View(articuloDTOAguardar);
            }
        }
        public IActionResult Details()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Edit(int articuloId)
        {
            var articuloAModificar = articuloService.GetArticuloPorId(articuloId);
            return View(articuloAModificar);
        }

        [HttpPost]
        public IActionResult Edit(Articulo articuloAActualizar)
        {
            string mensaje = articuloService.ActualizarArticulo(articuloAActualizar);

            if (mensaje == "Articulo Actualizado")
            {
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = mensaje;
                return View(articuloAActualizar);
            }
        }

        [HttpGet]
        public IActionResult Delete(int articuloId)
        {
            var articuloAEliminar = articuloService.GetArticuloPorId(articuloId);

            if (articuloAEliminar == null)
            {
                return NotFound();
            }

            var resultado = articuloService.EliminarArticulo(articuloId);

            if (resultado != "Articulo eliminado correctamente")
            {
                return BadRequest(resultado);
            }

            return RedirectToAction(nameof(Index));
        }


    }
}






