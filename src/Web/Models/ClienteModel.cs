using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Models
{
    [Table("Cliente")]
    public class ClienteModel : UsuarioModel
    {
        [Required, Column(TypeName = "char(14)")]
        public string CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        [NotMapped]
        public int Idade
        {
            get => (int)Math.Floor((DateTime.Now - DataNascimento).TotalDays / 365.2425);
        }
        public ICollection<VendaModel> Vendas { get; set; }

        public static void Seed(ModelBuilder pModelBuilder)
        {
            pModelBuilder.Entity<ClienteModel>().HasData(
                new ClienteModel { IdUsuario = 1, Nome = "Natalie Portman", Email = "natalie.portman@ambev.com.br", Senha = "c157a6c950350e1cff1c37cd370782ce", 
                    CPF = "791.647.240-69", DataNascimento = new DateTime(1981, 6, 9) },
                new ClienteModel { IdUsuario = 2, Nome = "Penelope Cruz", Email = "penelope.cruz@ambev.com.br", Senha = "7688ab571b8c9112c74f634b0942c5f4", 
                    CPF = "409.579.270-10", DataNascimento = new DateTime(1981, 6, 9) },
                new ClienteModel { IdUsuario = 3, Nome = "Sharon Stone", Email = "sharon.stone@ambev.com.br", Senha = "03d93326278650c09d0f825734230724", 
                    CPF = "616.150.370-04", DataNascimento = new DateTime(1958, 3, 10) },
                new ClienteModel { IdUsuario = 4, Nome = "Jennifer Connelly", Email = "jennifer.connelly@ambev.com.br", Senha = "fad05d338687a585d335d6d122034990", 
                    CPF = "345.930.500-22", DataNascimento = new DateTime(1970, 12, 12) },
                new ClienteModel { IdUsuario = 5, Nome = "Scarlett Johansson", Email = "scarlett.johansson@ambev.com.br", Senha = "318faad8da75364f28d3af4c01cf8783", 
                    CPF = "139.385.260-25", DataNascimento = new DateTime(1984, 4, 22) },
                new ClienteModel { IdUsuario = 6, Nome = "Liv Tyler", Email = "liv.tyler@ambev.com.br", Senha = "d2137db845f757d43f654f8eabc80762", 
                    CPF = "932.830.890-94", DataNascimento = new DateTime(1977, 6, 1) },
                new ClienteModel { IdUsuario = 7, Nome = "Jennifer Lopez", Email = "jennifer.lopez@ambev.com.br", Senha = "d30579c85039820a61f8426c5a12704d", 
                    CPF = "074.477.660-03", DataNascimento = new DateTime(1969, 6, 24) },
                new ClienteModel { IdUsuario = 8, Nome = "Gal Gadot", Email = "gal.gadot@ambev.com.br", Senha = "4fdf95910c5e7817123b3e09936e853f", 
                    CPF = "760.947.580-72", DataNascimento = new DateTime(1985, 4, 30) },
                new ClienteModel { IdUsuario = 9, Nome = "Odette Annable", Email = "odette.annable@com.br", Senha = "5f87be8e29c305ccd4ee19b0dd898f2b", 
                    CPF = "141.237.860-57", DataNascimento = new DateTime(1985, 5, 10) },
                new ClienteModel { IdUsuario = 10, Nome = "Jennifer Lawrence", Email = "jennifer.lawrence@ambev.com.br", Senha = "c4ad31baad701357a17d7389aa68b7e8", 
                    CPF = "290.137.010-19", DataNascimento = new DateTime(1990, 8, 15) }
            );
        }
    }
}