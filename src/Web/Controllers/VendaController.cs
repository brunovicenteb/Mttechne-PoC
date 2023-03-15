using System;
using System.Linq;
using System.Threading.Tasks;
using AmbevWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Controllers
{
    public class VendaController : Controller
    {
        private readonly AmbevContext _Context;

        public VendaController(AmbevContext pContext)
        {
            _Context = pContext;
        }

        public async Task<IActionResult> Index(int? cid)
        {
            if (cid.HasValue)
            {
                var cliente = await _Context.Clientes.FindAsync(cid);
                if (cliente != null)
                {
                    var vendas = await _Context.Vendas
                        .Where(p => p.IdCliente == cid)
                        .OrderByDescending(x => x.IdVenda)
                        .AsNoTracking().ToListAsync();

                    ViewBag.Cliente = cliente;
                    return View(vendas);
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado",
                        TipoMensagem.Erro);
                    return RedirectToAction("Index", "Cliente");
                }
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não informado",
                    TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? cid)
        {
            if (cid.HasValue)
            {
                var cliente = await _Context.Clientes.FindAsync(cid);
                if (cliente != null)
                {
                    VendaModel venda = null;
                    _Context.Entry(cliente).Collection(c => c.Vendas).Load();
                    if (_Context.Vendas.Any(p => p.IdCliente == cid && !p.DataVenda.HasValue))
                    {
                        venda = await _Context.Vendas
                            .FirstOrDefaultAsync(p => p.IdCliente == cid && !p.DataVenda.HasValue);
                    }
                    else
                    {
                        venda = new VendaModel { IdCliente = cid.Value, ValorTotal = 0, CashBack = 0 };
                        cliente.Vendas.Add(venda);
                        await _Context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", "ItemVenda", new { vend = venda.IdVenda });
                }
                TempData["mensagem"] = MensagemModel.Serializar("Cliente não encontrado", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
            TempData["mensagem"] = MensagemModel.Serializar("Cliente não informado", TipoMensagem.Erro);
            return RedirectToAction("Index", "Cliente");
        }

        private bool VendaExiste(int id)
        {
            return _Context.Vendas.Any(x => x.IdVenda == id);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id, [FromForm] VendaModel pedido)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (VendaExiste(id.Value))
                    {
                        _Context.Vendas.Update(pedido);
                        if (await _Context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Venda alterada com sucesso.");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar venda.", TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada.", TipoMensagem.Erro);
                    }
                }
                else
                {
                    _Context.Vendas.Add(pedido);
                    if (await _Context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Venda incluída com sucesso.");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro ao cadastrar venda.", TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pedido);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não informada.", TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            if (!VendaExiste(id.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var venda = await _Context.Vendas
                .Include(p => p.Cliente)
                .Include(p => p.ItensVenda)
                .ThenInclude(i => i.Cerveja)
                .FirstOrDefaultAsync(p => p.IdVenda == id);

            return View(venda);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            VendaModel vendaModel = await _Context.Vendas.FindAsync(id);
            var venda = vendaModel;
            if (venda != null)
            {
                _Context.Vendas.Remove(venda);
                if (await _Context.SaveChangesAsync() > 0)
                    TempData["mensagem"] = MensagemModel.Serializar("Venda excluída com sucesso.");
                else
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir a venda.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), new { cid = venda.IdCliente });
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Fechar(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não informada.", TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            if (!VendaExiste(id.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var venda = await _Context.Vendas
                .Include(p => p.Cliente)
                .Include(p => p.ItensVenda)
                .ThenInclude(i => i.Cerveja)
                .FirstOrDefaultAsync(p => p.IdVenda == id);

            return View(venda);
        }

        [HttpPost]
        public async Task<IActionResult> Fechar(int id)
        {
            if (VendaExiste(id))
            {
                var venda = await _Context.Vendas
                    .Include(p => p.Cliente)
                    .Include(p => p.ItensVenda)
                    .ThenInclude(i => i.Cerveja)
                    .FirstOrDefaultAsync(p => p.IdVenda == id);

                if (venda.ItensVenda.Count() > 0)
                {
                    venda.DataVenda = DateTime.Now;
                    foreach (var item in venda.ItensVenda)
                        item.Cerveja.Estoque -= item.Quantidade;
                    if (await _Context.SaveChangesAsync() > 0)
                        TempData["mensagem"] = MensagemModel.Serializar("Venda fechada com sucesso.");
                    else
                        TempData["mensagem"] = MensagemModel.Serializar("Não foi possível fechar a venda.", TipoMensagem.Erro);
                    return RedirectToAction("Index", new { cid = venda.IdCliente });
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não é possível fechar uma venda sem cerveja.", TipoMensagem.Erro);
                    return RedirectToAction("Index", new { cid = venda.IdCliente });
                }
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrada.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Entregar(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não informada.", TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            if (!VendaExiste(id.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var pedido = await _Context.Vendas
                .Include(p => p.Cliente)
                .Include(p => p.ItensVenda)
                .ThenInclude(i => i.Cerveja)
                .FirstOrDefaultAsync(p => p.IdVenda == id);

            return View(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Entregar(int idVenda)
        {
            if (!VendaExiste(idVenda))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Venda não encontrado.", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
            var venda = await _Context.Vendas.FindAsync(idVenda);
            venda.DataEntrega = DateTime.Now;
            if (await _Context.SaveChangesAsync() > 0)
                TempData["mensagem"] = MensagemModel.Serializar("Entrega de venda registrada com sucesso.");
            else
                TempData["mensagem"] = MensagemModel.Serializar("Não foi possível registrar a entrega da venda.", TipoMensagem.Erro);
            return RedirectToAction("Index", new { cid = venda.IdCliente });
        }
    }
}