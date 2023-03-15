using System;
using System.Linq;
using System.Threading.Tasks;
using AmbevWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Controllers
{
    public class ItemVendaController : Controller
    {
        private readonly AmbevContext _Context;

        public ItemVendaController(AmbevContext context)
        {
            this._Context = context;
        }

        public async Task<IActionResult> Index(int? vend)
        {
            if (vend.HasValue)
            {
                if (_Context.Vendas.Any(p => p.IdVenda == vend))
                {
                    var venda = await _Context.Vendas
                        .Include(p => p.Cliente)
                        .Include(p => p.ItensVenda.OrderBy(i => i.Cerveja.Nome))
                        .ThenInclude(i => i.Cerveja)
                        .FirstOrDefaultAsync(p => p.IdVenda == vend);

                    ViewBag.Venda = venda;
                    return View(venda.ItensVenda);
                }
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
            TempData["mensagem"] = MensagemModel.Serializar("Venda não informada.", TipoMensagem.Erro);
            return RedirectToAction("Index", "Cliente");
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? vend, int? cerv)
        {
            if (vend.HasValue)
            {
                if (_Context.Vendas.Any(p => p.IdVenda == vend))
                {
                    var cervejas = _Context.Cervejas
                        .OrderBy(x => x.Nome)
                        .Select(p => new { p.IdCerveja, NomePreco = $"{p.Nome} ({p.Preco:C})" })
                        .AsNoTracking().ToList();
                    var cervejasSelectList = new SelectList(cervejas, "IdCerveja", "NomePreco");
                    ViewBag.Cervejas = cervejasSelectList;

                    if (cerv.HasValue && ItemVendaExiste(vend.Value, cerv.Value))
                    {
                        var itemVenda = await _Context.ItensVendas
                            .Include(i => i.Cerveja)
                            .FirstOrDefaultAsync(i => i.IdVenda == vend && i.IdCerveja == cerv);
                        return View(itemVenda);
                    }
                    else
                    {
                        return View(new ItemVendaModel() { IdVenda = vend.Value, ValorUnitario = 0, FracaoCachBack = 0, Quantidade = 1 });
                    }
                }
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
            TempData["mensagem"] = MensagemModel.Serializar("Venda não informada.", TipoMensagem.Erro);
            return RedirectToAction("Index", "Cliente");
        }

        private bool ItemVendaExiste(int vend, int cerv)
        {
            return _Context.ItensVendas.Any(x => x.IdVenda == vend && x.IdCerveja == cerv);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromForm] ItemVendaModel itemVenda)
        {
            if (ModelState.IsValid)
            {
                if (itemVenda.IdVenda > 0)
                {
                    var cerveja = await _Context.Cervejas.FindAsync(itemVenda.IdCerveja);
                    itemVenda.ValorUnitario = cerveja.Preco;
                    itemVenda.IdSituacaoCashBack = SituacaoCashBackModel.DisponivelID;
                    int diaSemanaNum = Convert.ToInt32(DateTime.Now.DayOfWeek) + 1; // Nosso dia da semana no BD começa em 0;
                    var diaSemana = await _Context.DiaDaSemana.
                        Where(o => o.IdDiaDaSemana == diaSemanaNum).SingleOrDefaultAsync();
                    if (diaSemana == null)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Não foi possível obter o Dia a Semana para calcular o CashBack.");
                        return RedirectToAction("Index", new { vend = itemVenda.IdVenda });
                    }
                    var cashBack = await _Context.CashBack.Where(o => o.IdCerveja == itemVenda.IdCerveja && o.IdDiaDaSemana == diaSemana.IdDiaDaSemana).
                        SingleOrDefaultAsync();
                    if (cashBack == null)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Não foi possível obter o CashBack configurado para o produto.");
                        return RedirectToAction("Index", new { vend = itemVenda.IdVenda });
                    }
                    itemVenda.IdCashBack = cashBack.IdCachBack;
                    itemVenda.FracaoCachBack = cashBack.Porcentagem / 100;
                    if (ItemVendaExiste(itemVenda.IdVenda, itemVenda.IdCerveja))
                    {
                        _Context.ItensVendas.Update(itemVenda);
                        if (await _Context.SaveChangesAsync() > 0)
                            TempData["mensagem"] = MensagemModel.Serializar("Item de venda alterado com sucesso.");
                        else
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar item de venda.", TipoMensagem.Erro);
                    }
                    else
                    {
                        _Context.ItensVendas.Add(itemVenda);
                        if (await _Context.SaveChangesAsync() > 0)
                            TempData["mensagem"] = MensagemModel.Serializar("Item de venda cadastrado com sucesso.");
                        else
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao cadastrar item de venda.", TipoMensagem.Erro);
                    }
                    var venda = await _Context.Vendas.FindAsync(itemVenda.IdVenda);
                    venda.ValorTotal = _Context.ItensVendas.Where(i => i.IdVenda == itemVenda.IdVenda)
                        .Sum(i => i.ValorUnitario * i.Quantidade);
                    venda.CashBack = _Context.ItensVendas.Where(i => i.IdVenda == itemVenda.IdVenda)
                        .Sum(i => i.ValorUnitario * i.Quantidade * i.FracaoCachBack);
                    await _Context.SaveChangesAsync();
                    return RedirectToAction("Index", new { vend = itemVenda.IdVenda });
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Venda não informada.", TipoMensagem.Erro);
                    return RedirectToAction("Index", "Cliente");
                }
            }
            else
            {
                var produtos = _Context.Cervejas
                        .OrderBy(x => x.Nome)
                        .Select(p => new { p.IdCerveja, NomePreco = $"{p.Nome} ({p.Preco:C})" })
                        .AsNoTracking().ToList();
                var produtosSelectList = new SelectList(produtos, "IdCerveja", "NomePreco");
                ViewBag.Cervejas = produtosSelectList;

                return View(itemVenda);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? vend, int? cerv)
        {
            if (!vend.HasValue || !cerv.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Item de venda não informado.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            if (!ItemVendaExiste(vend.Value, cerv.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Item de venda não encontrado.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var itemVenda = await _Context.ItensVendas.FindAsync(vend, cerv);
            _Context.Entry(itemVenda).Reference(i => i.Cerveja).Load();
            return View(itemVenda);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int idVenda, int IdCerveja)
        {
            var itemvendido = await _Context.ItensVendas.FindAsync(idVenda, IdCerveja);
            if (itemvendido != null)
            {
                _Context.ItensVendas.Remove(itemvendido);
                if (await _Context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Item de venda excluído com sucesso.");
                    var vendido = await _Context.Vendas.FindAsync(itemvendido.IdVenda);
                    vendido.ValorTotal = _Context.ItensVendas
                        .Where(i => i.IdVenda == itemvendido.IdVenda)
                        .Sum(i => i.ValorUnitario * i.Quantidade);
                    await _Context.SaveChangesAsync();
                }
                else
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir o item de venda.", TipoMensagem.Erro);
                return RedirectToAction("Index", new { vend = idVenda });
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Item de venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction("Index", new { vend = idVenda });
            }
        }
    }
}