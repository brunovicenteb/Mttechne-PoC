using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AmbevWeb.Models
{
    public class AmbevContext : DbContext
    {
        public DbSet<CervejaModel> Cervejas { get; set; }
        public DbSet<DiaDaSemanaModel> DiaDaSemana { get; set; }
        public DbSet<SituacaoCashBackModel> SituacaoCashBack { get; set; }
        public DbSet<CashBackModel> CashBack { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<VendaModel> Vendas { get; set; }
        public DbSet<ItemVendaModel> ItensVendas { get; set; }

        public AmbevContext(DbContextOptions<AmbevContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder pModelBuilder)
        {
            pModelBuilder.Entity<CervejaModel>().ToTable("Cerveja");
            pModelBuilder.Entity<DiaDaSemanaModel>().ToTable("DiaDaSemana");
            pModelBuilder.Entity<CashBackModel>().ToTable("CashBack");
            pModelBuilder.Entity<ClienteModel>().ToTable("Cliente");
            pModelBuilder.Entity<VendaModel>().ToTable("Venda");
            pModelBuilder.Entity<SituacaoCashBackModel>().ToTable("SituacaoCashBack");

            pModelBuilder.Entity<VendaModel>().Property(u => u.InicioVenda)
                .HasDefaultValueSql("datetime('now', 'localtime')")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            pModelBuilder.Entity<UsuarioModel>().Property(u => u.DataCadastro)
                .HasDefaultValueSql("datetime('now', 'localtime')")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            pModelBuilder.Entity<CervejaModel>().Property(p => p.Estoque)
                .HasDefaultValue(0);
            pModelBuilder.Entity<ItemVendaModel>()
                            .HasKey(ip => new { ip.IdVenda, ip.IdCerveja });

            CervejaModel.Seed(pModelBuilder);
            DiaDaSemanaModel.Seed(pModelBuilder);
            CashBackModel.Seed(pModelBuilder);
            SituacaoCashBackModel.Seed(pModelBuilder);
            ClienteModel.Seed(pModelBuilder);
        }
    }
}