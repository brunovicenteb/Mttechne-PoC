using System.Linq;
using System.Threading.Tasks;
using AmbevWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AmbevWeb.Controllers
{
    public class CervejaController : Controller
    {
        private readonly AmbevContext _context;

        public CervejaController(AmbevContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Cervejas.OrderBy(x => x.Nome).AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? id)
        {
            if (id.HasValue)
            {
                var produto = await _context.Cervejas.FindAsync(id);
                if (produto == null)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cerveja não encontrada.", TipoMensagem.Erro);
                    return RedirectToAction(nameof(Index));
                }
                return View(produto);
            }
            return View(new CervejaModel());
        }

        private bool CervejaExiste(int id)
        {
            return _context.Cervejas.Any(x => x.IdCerveja == id);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id, [FromForm] CervejaModel cerveja)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (CervejaExiste(id.Value))
                    {
                        _context.Cervejas.Update(cerveja);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Cerveja alterada com sucesso.");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar cerveja.", TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Cerveja não encontrada.", TipoMensagem.Erro);
                    }
                }
                else
                {
                    _context.Cervejas.Add(cerveja);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Cerveja cadastrada com sucesso.");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro ao cadastrar cerveja.", TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(cerveja);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cerveja não informada.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            var produto = await _context.Cervejas.FindAsync(id);
            if (produto == null)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cerveja não informada.", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index));
            }

            return View(produto);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var produto = await _context.Cervejas.FindAsync(id);
            if (produto != null)
            {
                _context.Cervejas.Remove(produto);
                if (await _context.SaveChangesAsync() > 0)
                    TempData["mensagem"] = MensagemModel.Serializar("Cerveja excluída com sucesso.");
                else
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possível excluir a cerveja.", TipoMensagem.Erro);                
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cerveja não encontrada.", TipoMensagem.Erro);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}