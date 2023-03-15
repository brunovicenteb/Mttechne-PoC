using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Models
{
    [Table("SituacaoCashBack")]
    public class SituacaoCashBackModel
    {
        [Key]
        public int IdSituacaoCashBack { get; set; }

        [Required, MaxLength(32)]
        public string Nome { get; set; }

        public static readonly int DisponivelID = 1;
        public static readonly int ResgatadoID = 2;

        public static void Seed(ModelBuilder pModelBuilder)
        {
            pModelBuilder.Entity<SituacaoCashBackModel>().HasData(
                new SituacaoCashBackModel { IdSituacaoCashBack = DisponivelID, Nome = "Disponível" },
                new SituacaoCashBackModel { IdSituacaoCashBack = ResgatadoID, Nome = "Resgatado" }
            );
        }
    }
}