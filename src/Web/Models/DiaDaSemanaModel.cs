using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Models
{
    [Table("DiaDaSemana")]
    public class DiaDaSemanaModel
    {
        [Key]
        public int IdDiaDaSemana { get; set; }

        [Required, MaxLength(32)]
        public string Nome { get; set; }

        public static void Seed(ModelBuilder pModelBuilder)
        {
            pModelBuilder.Entity<DiaDaSemanaModel>().HasData(
                new DiaDaSemanaModel { IdDiaDaSemana = 1, Nome = "Domingo" },
                new DiaDaSemanaModel { IdDiaDaSemana = 2, Nome = "Segunda-Feira" },
                new DiaDaSemanaModel { IdDiaDaSemana = 3, Nome = "Terça-Feira" },
                new DiaDaSemanaModel { IdDiaDaSemana = 4, Nome = "Quarta-Feira" },
                new DiaDaSemanaModel { IdDiaDaSemana = 5, Nome = "Quinta-Feira" },
                new DiaDaSemanaModel { IdDiaDaSemana = 6, Nome = "Sexta-Feira" },
                new DiaDaSemanaModel { IdDiaDaSemana = 7, Nome = "Sábado" });
        }
    }
}