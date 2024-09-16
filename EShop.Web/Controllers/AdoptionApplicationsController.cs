using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Models;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using EShop.Domain.Status;
using Microsoft.AspNetCore.Identity;
using EShop.Domain.Identity;
using System.Security.Claims;
using Newtonsoft.Json;
using Stripe.Climate;
using System.Text;
using GemBox.Document;
using ClosedXML.Excel;
using Stripe;
using Stripe.Checkout;
using Microsoft.AspNetCore.Authorization;


namespace EShop.Web.Controllers
{
    [Authorize]
    public class AdoptionApplicationsController : Controller
    {
        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly IUserRepository _userRepository;
        private readonly IPetService _petService;
        private readonly UserManager<PetAdoptionCenterUser> _userManager;

        public AdoptionApplicationsController(IAdoptionApplicationService adoptionApplicationService, UserManager<PetAdoptionCenterUser> userManager, IUserRepository userRepository, IPetService petService)
        {
            _adoptionApplicationService = adoptionApplicationService;
            _userRepository = userRepository;
            _petService = petService;
            _userManager=userManager;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        // GET: AdoptionApplication
        public IActionResult Index()
        {
            var applications = _adoptionApplicationService.GetAllAdoptionApplications();

            if (!User.IsInRole("Admin"))
            {
                applications = applications.Where(x => x.AdopterId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
            }
            return View(applications);
        }

        // GET: AdoptionApplication/Details/5
        public IActionResult Details(Guid id)
        {
            var application = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // GET: AdoptionApplication/Apply
        [Authorize(Roles ="Adopter")]
        public IActionResult Apply(Guid petId)
        {
            var pet = _petService.GetPetById(petId);
            if (pet == null || pet.Status == PetStatus.Adopted)
            {
                return NotFound();
            }
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _userRepository.Get(userId); 

            var application = new AdoptionApplication
            {
                Pet = pet,
                Adopter = user,
                PetId = petId,
                AdopterId = userId,
                ApplicationDate = DateTime.UtcNow
            };

            return View(application);
        }

        // POST: AdoptionApplication/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply([Bind("AdopterId,PetId,AdoptionApplicationStatus,ApplicationDate")] AdoptionApplication application)
        {
            var pet = _petService.GetPetById(application.PetId.Value);
            if (pet == null || pet.Status == PetStatus.Adopted)
            {
                ModelState.AddModelError(string.Empty, "Cannot apply for an already adopted pet.");
                return View(application);
            }

            if (ModelState.IsValid)
            {
                application.Id = Guid.NewGuid();
                application.ApplicationDate = DateTime.UtcNow;
                application.AdoptionApplicationStatus = PetAdoptionApplicationStatus.Pending;

                _adoptionApplicationService.AddAdoptionApplication(application);
                return RedirectToAction(nameof(Index));
            }

            return View(application);
        }

        // GET: AdoptionApplication/Edit/5
        /*[Authorize(Roles = "Adopter")]
        public IActionResult Edit(Guid id)
        {
            var adoptionApplication = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (adoptionApplication == null)
            {
                return NotFound();
            }

            ViewData["AdopterId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            ViewData["PetId"] = new SelectList(_petService.GetAllPets(), "Id", "Id");
            return View(adoptionApplication);
        }

        // POST: AdoptionApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,AdopterId,PetId,AdoptionApplicationStatus,ApplicationDate,AdoptionDate")] AdoptionApplication application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _adoptionApplicationService.UpdateAdoptionApplication(application);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("MyApplications", new { id = application.AdopterId });
            }

            ViewData["AdopterId"] = new SelectList(_userRepository.GetAll(), "Id", "Id");
            ViewData["PetId"] = new SelectList(_petService.GetAllPets(), "Id", "Id");
            return View(application);
        }*/

        // GET: AdoptionApplication/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var application = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: AdoptionApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _adoptionApplicationService.DeleteAdoptionApplication(id);
            return RedirectToAction(nameof(Index));
        }

        public FileContentResult CreateInvoice(Guid Id)
        {

            var application = _adoptionApplicationService.GetAdoptionApplicationById(Id);
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "ApplicationDetails.docx");
            var document = DocumentModel.Load(templatePath);
            document.Content.Replace("{{ApplicationStatus}}", application.AdoptionApplicationStatus.ToString());
            document.Content.Replace("{{Pet}}", application.Pet.Name);
            document.Content.Replace("{{Adopter}}", application.Adopter.FirstName + " "+application.Adopter.LastName);
            document.Content.Replace("{{ApplicationDate}}", application.ApplicationDate.ToString());
            //document.Content.Replace("{{AdoptionDate}}", application.AdoptionDate.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ApplicationDetails.pdf");

        }
        [HttpGet]
        public FileContentResult ExportApplications()
        {
            string fileName = "AllApplications.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("Apps");

                worksheet.Cell(1, 1).Value = "Application ID";
                worksheet.Cell(1, 2).Value = "Pet";
                worksheet.Cell(1, 3).Value = "Pet Type";
                worksheet.Cell(1, 4).Value = "Pet Breed";
                worksheet.Cell(1, 5).Value = "Adopter";
                worksheet.Cell(1, 6).Value = "Application Date";

                var applications = _adoptionApplicationService.GetAllAdoptionApplications();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user =  _userManager.FindByIdAsync(userId);
                for (int i = 0; i < applications.Count(); i++)
                {
                    var application = applications[i];
                    worksheet.Cell(i + 2, 1).Value = application.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = application.Pet.Name;
                    worksheet.Cell(i + 2, 3).Value = application.Pet.Type.ToString();
                    worksheet.Cell(i + 2, 4).Value = application.Pet.Breed;
                    worksheet.Cell(i + 2, 5).Value = user.Result.FirstName;
                    worksheet.Cell(i + 2, 6).Value =application.ApplicationDate.ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Review(Guid id)
        {
            var application = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: AdoptionApplication/Review/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Review(Guid id, string decision)
        {
            var application = _adoptionApplicationService.GetAdoptionApplicationById(id);
            if (application == null)
            {
                return NotFound();
            }

            if (decision == "Accept")
            {
                application.AdoptionApplicationStatus = PetAdoptionApplicationStatus.Approved;
                _petService.UpdatePetStatus(application.Pet.Id, PetStatus.Adopted); 
            }
            else if (decision == "Deny")
            {
                application.AdoptionApplicationStatus = PetAdoptionApplicationStatus.Rejected;
            }

            _adoptionApplicationService.UpdateAdoptionApplication(application);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Adopter")]
        public IActionResult PayForAdoption(Guid id) 
        { 
            var pet = _adoptionApplicationService.GetAdoptionApplicationById(id).Pet;
            ViewData["Price"] = pet?.Price;
            return View("PayAdoption");
        }

        //[Authorize(Roles = "Adopter")]
        public IActionResult PayAdoption(string stripeEmail, string stripeToken,Guid Id)
        {
            StripeConfiguration.ApiKey = "sk_test_51PyaAyEesqNygYRke2NeuxXfeBRQDnuSxzPN8oluLkC1uZlr2xOs9SvXhiiByXVc7FBiRRmxTdtbxjZxdUZ0Ny0m00FC8mML5V";
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var application = this._adoptionApplicationService.GetAdoptionApplicationById(Id);
            var pet = application.Pet;

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(application.Pet.Price) * 100),
                Description = "Pet Adoption Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                pet.Status = PetStatus.Adopted;
                _petService.UpdatePet(pet);

                
                application.AdoptionApplicationStatus = PetAdoptionApplicationStatus.Completed;
                _adoptionApplicationService.UpdateAdoptionApplication(application);

                return RedirectToAction("SuccessPayment");
            }
            else
            {
               
                return RedirectToAction("FailedPayment");
            }
        }

       
        public IActionResult SuccessPayment()
        {
           /* var application = _adoptionApplicationService.GetAdoptionApplicationById(applicationId);
            if (application != null)
            {
                var pet = application.Pet;
                pet.Status = PetStatus.Adopted;
                _petService.UpdatePet(pet);
                application.AdoptionApplicationStatus = PetAdoptionApplicationStatus.Completed;
                _adoptionApplicationService.UpdateAdoptionApplication(application);
            }*/

            return View();
        }

        
        public IActionResult FailedPayment()
        {
            return View();
        }


    }
}
