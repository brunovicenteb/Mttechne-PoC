using System;
using System.Linq;
using AmbevWeb.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

namespace AmbevWeb.Controllers
{

    [Route("api/[controller]")]
    public class BeerController : Controller
    {
        private static readonly int _PageSize = 2;
        private readonly AmbevContext _Context;

        public BeerController(AmbevContext pContext)
        {
            _Context = pContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        //http://{host}:{porta}/api/ConsultarCervejas?pagina=xx&nome=yy
        [HttpGet("ConsultarCervejas/{pagina?}/{nome?}")]
        public async Task<IActionResult> ConsultarCervejas(int? pagina = 0, string nome = null)
        {
            // Paginando [OK]
            // Filtrando por nome [OK]
            // Ordenar por nome de forma crescente [OK]
            bool noFilter = string.IsNullOrEmpty(nome);
            nome = noFilter ? nome : nome.ToLower();
            int jump = (pagina ?? 0) * _PageSize;
            var beer = await _Context.Cervejas
                .Where(o => noFilter || o.Nome.ToLower().Contains(nome))
                .OrderBy(o => o.Nome)
                .Skip(jump)
                .Take(_PageSize)
                .AsNoTracking().ToListAsync();
            return Ok(beer);
        }

        //http://{host}:{porta}/api/ConsultarCervejaPeloIdentificador?id=xx
        [HttpGet("ConsultarCervejaPeloIdentificador/{id?}")]
        public async Task<IActionResult> ConsultarCervejaPeloIdentificador(int? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Nenhum identificador de cerveja foi informado.");
            var beer = await _Context.Cervejas.FindAsync(id);
            if (beer == null)
                return BadRequest("Nenhuma cerveja foi encontrada pelo identificador " + id + ".");
            return Ok(beer);
        }

        //http://{host}:{porta}/api/ConsultarVendaPeloIdentificador?id=xx
        [HttpGet("ConsultarVendaPeloIdentificador/{id?}")]
        public async Task<IActionResult> ConsultarVendaPeloIdentificador(int? id = null)
        {
            if (!id.HasValue)
                return BadRequest("Nenhum identificador de venda foi informado.");
            var venda = await _Context.Vendas
                .Include(p => p.Cliente)
                .Include(p => p.ItensVenda)
                .ThenInclude(i => i.Cerveja)
                .SingleOrDefaultAsync(p => p.IdVenda == id);
            if (venda == null)
                return BadRequest("Nenhuma venda foi encontrada pelo identificador " + id + ".");
            return Ok(venda);
        }

        //http://{host}:{porta}/api/ConsultarVendas?pagina=xx&inicio=yy&final=zz
        [HttpGet("ConsultarVendas/{pagina?}/{inicio?}/{final?}")]
        public async Task<IActionResult> ConsultarVendas(int? pagina = 0, DateTime? inicio = null, DateTime? final = null)
        {
            // Páginando [OK]
            // Ordenando de forma decrescente pela data do início da venda [OK]
            // Range [OK]
            int jump = (pagina ?? 0) * _PageSize;
            string debug = !inicio.HasValue ? "Início não chegou" : inicio.Value.ToString("dd/MM/yyyy mm:HH:ss");
            debug += "   <===>   " + (!final.HasValue ? "Final não chegou" : final.Value.ToString("dd/MM/yyyy mm:HH:ss"));
            Console.WriteLine(debug);
            var vendas = await _Context.Vendas
                .Where(o => (!inicio.HasValue || o.InicioVenda >= inicio.Value) &&
                            (!final.HasValue || o.InicioVenda <= final.Value))
                .OrderByDescending(o => o.InicioVenda)
                .Skip(jump)
                .Take(_PageSize)
                .Include(o => o.Cliente)
                .Include(o => o.ItensVenda)
                .AsNoTracking().ToListAsync();
            return Ok(vendas);
        }

        //http://{host}:{porta}/api/RegistrarVendaDeCerveja
        [HttpPost("RegistrarVendaDeCerveja")]
        public async Task<IActionResult> RegistrarVendaDeCerveja([FromBody] VendaModel pVenda)
        {
            if (pVenda == null)
                return BadRequest("Nenhum dado válido de venda foi informado.");
            if (pVenda.IdVenda > 0)
                return BadRequest("Não é necessário informar um id para a venda.");
            if (pVenda.ItensVenda == null || pVenda.ItensVenda.Count == 0)
                return BadRequest("Não é possível gerar uma venda sem nenhum item.");
            if (pVenda.ItensVenda.Any(o => o.IdCerveja == 0))
                return BadRequest("Existem itens da venda não vinculados a uma cerveja.");
            IDbContextTransaction t = null;
            try
            {
                t = await _Context.Database.BeginTransactionAsync();
                pVenda.Cliente = ManipularCliente(pVenda);
                pVenda.IdCliente = pVenda.Cliente.IdUsuario;
                pVenda.IdVenda = ManipularVenda(pVenda);
                await t.CommitAsync();
            }
            catch (Exception e)
            {
                await t.RollbackAsync();
                return BadRequest(e.Message);
            }
            return await ConsultarVendaPeloIdentificador(pVenda.IdVenda);
        }

        private int ManipularVenda(VendaModel pVenda)
        {
            foreach (var itmVenda in  pVenda.ItensVenda)
            {
                CervejaModel cv = _Context.Cervejas.Find(itmVenda.IdCerveja);
                if (cv == null)
                    throw new Exception("Não foi possível encontrar uma cerveja com o id passado no ítem da venda.");

                itmVenda.IdSituacaoCashBack = SituacaoCashBackModel.DisponivelID;
                itmVenda.ValorUnitario = cv.Preco;

                int diaSemanaNum = Convert.ToInt32(DateTime.Now.DayOfWeek) + 1; // Nosso dia da semana no BD começa em 0;
                var diaSemana = _Context.DiaDaSemana.
                    Where(o => o.IdDiaDaSemana == diaSemanaNum).SingleOrDefault();
                if (diaSemana == null)
                    throw new Exception("Não foi possível obter o Dia a Semana para calcular o CashBack.");
                var cashBack = _Context.CashBack.Where(o => o.IdCerveja == itmVenda.IdCerveja && o.IdDiaDaSemana == diaSemana.IdDiaDaSemana)
                    .SingleOrDefault();
                if (cashBack == null)
                    throw new Exception("Não foi possível obter o CashBack configurado para o produto.");

                itmVenda.IdCashBack = cashBack.IdCachBack;
                itmVenda.FracaoCachBack = cashBack.Porcentagem / 100;
            }
            pVenda.ValorTotal = pVenda.ItensVenda.Sum(i => i.ValorUnitario * i.Quantidade);
            pVenda.CashBack = pVenda.ItensVenda.Sum(i => i.ValorUnitario * i.Quantidade * i.FracaoCachBack);
            _Context.Vendas.Add(pVenda);
            _Context.SaveChanges();
            return pVenda.IdVenda;
        }

        private ClienteModel ManipularCliente(VendaModel pVenda)
        {
            ClienteModel c = null;
            if (pVenda.IdCliente > 0)
            {
                c = _Context.Clientes.Find(pVenda.IdCliente);
                if (c == null)
                    throw new Exception("Não existe cliente com o id informado.");
                return c;
            }
            if (pVenda.Cliente == null)
                throw new Exception("Nenhuma informação de cliente foi informada.");
            if (string.IsNullOrEmpty(pVenda.Cliente.Nome))
                throw new Exception("Informe o nome do cliente.");
            if (string.IsNullOrEmpty(pVenda.Cliente.CPF) && string.IsNullOrEmpty(pVenda.Cliente.Email))
                throw new Exception("Informe o e-mail ou o CPF do cliente.");
            string nome = pVenda.Cliente.Nome.ToLower();
            c = _Context.Clientes.FirstOrDefault(o => o.Nome.ToLower() == nome &&
                (o.CPF == pVenda.Cliente.CPF || o.Email == pVenda.Cliente.Email));
            if (c != null)
                return c;
            c = new ClienteModel();
            c.Nome = pVenda.Cliente.Nome;
            c.CPF = pVenda.Cliente.CPF ?? string.Empty;
            c.Email = pVenda.Cliente.Email ?? string.Empty;
            c.DataNascimento = pVenda.Cliente.DataNascimento;
            _Context.Clientes.Add(c);
            _Context.SaveChanges();
            return c;
        }
    }
}