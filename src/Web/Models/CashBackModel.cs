using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Models
{
    [Table("CashBack")]
    public class CashBackModel
    {
        [Key]
        public int IdCachBack { get; set; }

        public int IdCerveja { get; set; }

        public double Porcentagem { get; set; }

        public int IdDiaDaSemana { get; set; }

        [ForeignKey("IdCerveja")]
        public CervejaModel Cerveja { get; set; }

        [ForeignKey("IdDiaDaSemana")]
        public DiaDaSemanaModel DiaDaSemana { get; set; }

        public static void Seed(ModelBuilder pModelBuilder)
        {
            pModelBuilder.Entity<CashBackModel>().HasData(
                // Skok
                new CashBackModel { IdCachBack = 1, IdCerveja = 1, IdDiaDaSemana = 1, Porcentagem = 25 }, // Domingo
                new CashBackModel { IdCachBack = 2, IdCerveja = 1, IdDiaDaSemana = 2, Porcentagem = 7 }, // Segunda-Feira
                new CashBackModel { IdCachBack = 3, IdCerveja = 1, IdDiaDaSemana = 3, Porcentagem = 6 }, // Terça-Feira
                new CashBackModel { IdCachBack = 4, IdCerveja = 1, IdDiaDaSemana = 4, Porcentagem = 2 }, // Quarta-Feira
                new CashBackModel { IdCachBack = 5, IdCerveja = 1, IdDiaDaSemana = 5, Porcentagem = 10 }, // Quinta-Feira
                new CashBackModel { IdCachBack = 6, IdCerveja = 1, IdDiaDaSemana = 6, Porcentagem = 15 }, // Sexta-Feira
                new CashBackModel { IdCachBack = 7, IdCerveja = 1, IdDiaDaSemana = 7, Porcentagem = 20 }, // Sábado

                // Brahma
                new CashBackModel { IdCachBack = 8, IdCerveja = 2, IdDiaDaSemana = 1, Porcentagem = 30 }, // Domingo
                new CashBackModel { IdCachBack = 9, IdCerveja = 2, IdDiaDaSemana = 2, Porcentagem = 5 }, // Segunda-Feira
                new CashBackModel { IdCachBack = 10, IdCerveja = 2, IdDiaDaSemana = 3, Porcentagem = 10 }, // Terça-Feira
                new CashBackModel { IdCachBack = 11, IdCerveja = 2, IdDiaDaSemana = 4, Porcentagem = 15 }, // Quarta-Feira
                new CashBackModel { IdCachBack = 12, IdCerveja = 2, IdDiaDaSemana = 5, Porcentagem = 20 }, // Quinta-Feira
                new CashBackModel { IdCachBack = 13, IdCerveja = 2, IdDiaDaSemana = 6, Porcentagem = 25 }, // Sexta-Feira
                new CashBackModel { IdCachBack = 14, IdCerveja = 2, IdDiaDaSemana = 7, Porcentagem = 30 }, // Sábado

                // Stella
                new CashBackModel { IdCachBack = 15, IdCerveja = 3, IdDiaDaSemana = 1, Porcentagem = 35 }, // Domingo
                new CashBackModel { IdCachBack = 16, IdCerveja = 3, IdDiaDaSemana = 2, Porcentagem = 3 }, // Segunda-Feira
                new CashBackModel { IdCachBack = 17, IdCerveja = 3, IdDiaDaSemana = 3, Porcentagem = 5 }, // Terça-Feira
                new CashBackModel { IdCachBack = 18, IdCerveja = 3, IdDiaDaSemana = 4, Porcentagem = 8 }, // Quarta-Feira
                new CashBackModel { IdCachBack = 19, IdCerveja = 3, IdDiaDaSemana = 5, Porcentagem = 13 }, // Quinta-Feira
                new CashBackModel { IdCachBack = 20, IdCerveja = 3, IdDiaDaSemana = 6, Porcentagem = 18 }, // Sexta-Feira
                new CashBackModel { IdCachBack = 21, IdCerveja = 3, IdDiaDaSemana = 7, Porcentagem = 25 }, // Sábado

                // Bohemia
                new CashBackModel { IdCachBack = 22, IdCerveja = 4, IdDiaDaSemana = 1, Porcentagem = 40 }, // Domingo
                new CashBackModel { IdCachBack = 23, IdCerveja = 4, IdDiaDaSemana = 2, Porcentagem = 10 }, // Segunda-Feira
                new CashBackModel { IdCachBack = 24, IdCerveja = 4, IdDiaDaSemana = 3, Porcentagem = 15 }, // Terça-Feira
                new CashBackModel { IdCachBack = 25, IdCerveja = 4, IdDiaDaSemana = 4, Porcentagem = 15 }, // Quarta-Feira
                new CashBackModel { IdCachBack = 26, IdCerveja = 4, IdDiaDaSemana = 5, Porcentagem = 15 }, // Quinta-Feira
                new CashBackModel { IdCachBack = 27, IdCerveja = 4, IdDiaDaSemana = 6, Porcentagem = 20 }, // Sexta-Feira
                new CashBackModel { IdCachBack = 28, IdCerveja = 4, IdDiaDaSemana = 7, Porcentagem = 40 } // Sábado   
            );
        }
    }
}