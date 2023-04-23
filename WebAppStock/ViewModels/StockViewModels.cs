using CodigoComun.Modelos;
using CodigoComun.Modelos.DTO;
using CodigoComun.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace WebAppStock.ViewModels
{
    public class StockViewModels : IEnumerable<StockDTO>
    {
        public List<ArticuloDTO> ArticulosList { get; set; }
        public List<DepositoDTO> DepositosList { get; set; }

        public SelectList selectArticulosList { get; set; }
        public SelectList selectDepositosList { get; set; }
        public int SelectedArticulo { get; set; }
        public int SelectedDeposito { get; set; }
        public decimal Cantidad { get; set; } // Nuevo campo para la cantidad
        public StockDTO StockDTO { get; set; }

        public IEnumerable<StockDTO> StockDTOs
        {
            get { return _stockDTOs; }
            set { _stockDTOs = new List<StockDTO>(value); }
        }
        private List<StockDTO> _stockDTOs;

        public IEnumerator<StockDTO> GetEnumerator()
        {
            return _stockDTOs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


