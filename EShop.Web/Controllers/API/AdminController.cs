/*using EShop.Domain;
using EShop.Domain.Identity;
using EShop.Domain.Models;
using EShop.Service.Interface;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using Microsoft.AspNetCore.Identity;

namespace EshopWebApplication1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdoptionApplicationService _adoptionApplicationService;
        private readonly IPetService _petService;

        private readonly IShelterService _shelterService;
        private readonly UserManager<PetAdoptionCenterUser> userManager;

        public AdminController(IAdoptionApplicationService adoptionApplicationService, IPetService petService, IShelterService shelterService, UserManager<PetAdoptionCenterUser> userManager)
        {
            _adoptionApplicationService = adoptionApplicationService;
            _petService = petService;
            _shelterService = shelterService;
            this.userManager = userManager;
        }

        [HttpGet("[action]")]
        public List<Pet> GetAllPets()
        {
            return _petService.GetAllPets();
        }
        [HttpGet("[action]")]
        public List<AdoptionApplication> GetAllApplications()
        {
            return _adoptionApplicationService.GetAllAdoptionApplications();
        }

        [HttpPost("[action]")]
        public AdoptionApplication GetDetailsForApplication(BaseEntity model)
        {
            return _adoptionApplicationService.GetAdoptionApplicationsByPetId(model.Id); ;
        }
    }
}*/