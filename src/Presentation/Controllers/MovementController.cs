using System;
using System.Linq;
using System.Threading.Tasks;
using Mttechne.Domain.Models;
using Mttechne.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Mttechne.Application.ViewModel;
using Mttechne.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mttechne.UI.Web.Controllers;

public class MovementController : Controller
{
    private const string _RibbonElement = "message";
    private const string _MovementNotFound = "Movement not found.";
    private const string _MovementNotDelete = "Could not delete movement from database.";
    private const string _MovementNotInformed = "Movement not informed.";
    private const string _MovementSaved = "Movement saved successfully.";
    private const string _MovementRemoved = "Movement removed successfully.";
    private const string _DefaultNewType = "Credit";

    public MovementController(IMovementAppService service)
    {
        _service = service;
    }

    private readonly IMovementAppService _service;

    private void AddRibbonError(string errorMessage)
        => AddRibbonMessage(errorMessage, TypeMessage.Error);

    private void AddRibbonMessage(string message, TypeMessage typeMesage = TypeMessage.Info)
        => TempData[_RibbonElement] = MessageModel.Serialize(message, typeMesage);

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var movements = await _service.GetAllAsync();
        return View(movements);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue)
        {
            AddRibbonError(_MovementNotInformed);
            return RedirectToAction(nameof(Index));
        }
        var movement = await _service.GetByIdAsync(id.Value);
        if (movement is null)
        {
            AddRibbonError(_MovementNotFound);
            return RedirectToAction(nameof(Index));
        }
        return View(movement);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var sucess = await _service.DeleteAsync(id);
        if (sucess)
        {
            AddRibbonMessage(_MovementRemoved);
            return RedirectToAction(nameof(Index));
        }
        AddRibbonError(_MovementNotFound);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var types = await FillTypes();
        var defaultType = types.Single(o => o.Id == MovementType.CreditId);
        var itm = new MovementViewModel
        {
            MovementTypeId = defaultType.Id,
            MovementType = defaultType.Name
        };
        return View(itm);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] MovementViewModel movement)
    {
        if (!ModelState.IsValid)
        {
            await FillTypes();
            return View(movement);
        }
        await _service.CreateAsync(movement);
        AddRibbonMessage(_MovementSaved);
        return RedirectToAction("Index");
    }

    private async Task<IEnumerable<MovementTypeViewModel>> FillTypes()
    {
        var types = await _service.GetAllTypesAsync();
        ViewBag.Types = new SelectList(types, "Id", "Name");
        return types;
    }
}