using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Models;
using EShop.Repository;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using EShop.Domain.Status;
using System.Security.Claims;
using EShop.Service.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Web.Controllers
{
    [Authorize]
    public class PetsController : Controller
    {
        private readonly IPetService _petService;
        private readonly IUserRepository _userRepository;
        private readonly IShelterService _shelterService;
        public PetsController(IPetService petService, IUserRepository userRepository, IShelterService shelterService)
        {
            _petService = petService;
            _userRepository = userRepository;
            _shelterService = shelterService;
        }
        public IActionResult Index(Guid? shelterId)
        {

            ViewData["ShelterId"] = new SelectList(
         _shelterService.GetAllShelters(),
         "Id", "Name");

            var pets = shelterId.HasValue
                ? _petService.GetPetsByShelter(shelterId.Value)
                : _petService.GetAllPets();

            return View(pets);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Populate PetType dropdown
            ViewData["PetType"] = new SelectList(
                Enum.GetValues(typeof(PetType))
                .Cast<PetType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() }),
                "Id", "Name");

            // Populate Shelter dropdown
            ViewData["ShelterId"] = new SelectList(
                _shelterService.GetAllShelters(),
                "Id", "Name");

            return View();
        }

        // POST: Pets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Age,Type,Breed,Sex,Description,PhotoUrl,Status,ShelterId,Price")] Pet pet)
        {
            
                pet.Id = Guid.NewGuid();
                _petService.AddPet(pet);
                return RedirectToAction(nameof(Index));
            

            // Repopulate dropdowns if model state is invalid
            ViewData["PetType"] = new SelectList(
                Enum.GetValues(typeof(PetType))
                .Cast<PetType>()
                .Select(e => new { Id = (int)e, Name = e.ToString() }),
                "Id", "Name");

            ViewData["ShelterId"] = new SelectList(
                _shelterService.GetAllShelters(),
                "Id", "Name");

            return View(pet);
        }

        /*public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetDetailsForProduct(id);

            ProductInShoppingCart ps = new ProductInShoppingCart();

            if (product != null)
            {
                ps.ProductId = product.Id;
            }

            return View(ps);
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(ProductInShoppingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.AddToShoppingConfirmed(model, userId);



            return View("Index", _productService.GetAllProducts());
        }*/


        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                return NotFound();
            }
            return View(pet);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Age,Type,Breed,Sex,Description,PhotoUrl,Status,ShelterId,Price")] Pet pet)
        {
            if (id != pet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _petService.UpdatePet(pet);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pet);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = _petService.GetPetById(id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _petService.DeletePet(id);
            return RedirectToAction(nameof(Index));
        }
        // GET: Pets

    }
}
