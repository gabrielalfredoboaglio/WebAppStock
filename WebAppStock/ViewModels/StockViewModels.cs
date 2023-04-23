using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace WebAppStock.ViewModels
{
    public class StockViewModels
    {
      
        
            public SelectList selectArticulosList { get; set; }
            public SelectList selectDepositosList { get; set; }
            public int SelectedArticulo { get; set; }
            public int SelectedDeposito { get; set; }
            public decimal Cantidad { get; set; } // Nuevo campo para la cantidad
            public StockDTO StockDTO { get; set; }
        }

    }

