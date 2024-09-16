using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Models;
using EShop.Repository;
using EShop.Service.Interface;
using EShop.Service.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SheltersController : Controller
    {
        private readonly IShelterService _shelterService;
        private readonly IPetService _petService;

        public SheltersController(IShelterService shelterService, IPetService petService)
        {
            _shelterService = shelterService;
            _petService = petService;
        }

        // GET: Shelter
        public IActionResult Index()
        {
            var shelters = _shelterService.GetAllShelters();
            return View(shelters);
        }

        // GET: Shelter/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var shelter = _shelterService.GetShelterById(id);
            if (shelter == null)
            {
                return NotFound();
            }

            // Fetch pets for the selected shelter
            var pets = _petService.GetPetsByShelter(id);
            ViewBag.Pets = pets;

            return View(shelter);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shelter/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name, Location")] Shelter shelter)
        {
            shelter.Id = Guid.NewGuid();
            _shelterService.AddShelter(shelter);
            return RedirectToAction(nameof(Index));
        }

        // GET: Shelter/Edit/5
        public IActionResult Edit(Guid id)
        {
            var shelter = _shelterService.GetShelterById(id);
            if (shelter == null)
            {
                return NotFound();
            }
            return View(shelter);
        }

        // POST: Shelter/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Location")] Shelter shelter)
        {
            if (id != shelter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _shelterService.UpdateShelter(shelter);
                return RedirectToAction(nameof(Index));
            }
            return View(shelter);
        }

        // GET: Shelter/Delete/5
        public IActionResult Delete(Guid id)
        {
            var shelter = _shelterService.GetShelterById(id);
            if (shelter == null)
            {
                return NotFound();
            }
            return View(shelter);
        }

        // POST: Shelter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _shelterService.DeleteShelter(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
