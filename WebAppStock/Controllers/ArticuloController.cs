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
        [HttpGet]
        public IActionResult Edit(int articuloId)
        {
            ArticuloDTO articulo = articuloService.GetArticuloPorId(articuloId);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }


        [HttpPost]
        public IActionResult Edit(int id, ArticuloDTO articuloActualizado)
        {
            if (id != articuloActualizado.Id)
            {
                return BadRequest();
            }

            var resultado = articuloService.ActualizarArticulo(articuloActualizado);

            if (resultado != null)
            {
                TempData["Mensaje"] = "Articulo Actualizado";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Mensaje = "No se pudo actualizar el articulo";
                return View(articuloActualizado);
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

            if (resultado.Mensaje != "Articulo eliminado correctamente")
            {
                return BadRequest(resultado.Mensaje);
            }

            TempData["Mensaje"] = resultado.Mensaje;

            return RedirectToAction(nameof(Index));
        }



    }
}






