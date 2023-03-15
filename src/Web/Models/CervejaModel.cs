using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Models
{
    [Table("Cerveja")]
    public class CervejaModel
    {
        [Key]
        public int IdCerveja { get; set; }

        [Required, MaxLength(128)]
        public string Nome { get; set; }

        public int Estoque { get; set; }

        public double Preco { get; set; }
        
        public static void Seed(ModelBuilder pModelBuilder)
        {
            pModelBuilder.Entity<CervejaModel>().HasData(
                new CervejaModel { IdCerveja = 1, Nome = "Skol", Estoque = 2500, Preco = 5.5 },
                new CervejaModel { IdCerveja = 2, Nome = "Brahma", Estoque = 2250, Preco = 5.25 },
                new CervejaModel { IdCerveja = 3, Nome = "Stella", Estoque = 650, Preco = 7.25 },
                new CervejaModel { IdCerveja = 4, Nome = "Bohemia", Estoque = 850, Preco = 6.89 });
        }
    }
}