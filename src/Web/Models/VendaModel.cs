using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmbevWeb.Models
{
    [Table("Venda")]
    public class VendaModel
    {
        [Key]
        public int IdVenda { get; set; }
        public DateTime InicioVenda { get; set; }
        public DateTime? DataVenda { get; set; }
        public DateTime? DataEntrega { get; set; }
        public double? ValorTotal { get; set; }
        public double? CashBack { get; set; }
        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public ClienteModel Cliente { get; set; }

        public ICollection<ItemVendaModel> ItensVenda { get; set; }
    }
}