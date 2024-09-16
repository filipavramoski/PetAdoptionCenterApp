using EShop.Domain.Models;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class AdoptionApplicationService : IAdoptionApplicationService
    {
        private readonly IRepository<AdoptionApplication> _adoptionApplicationRepository;
        private readonly IAdoptionApplicationRepository _adoptionApplicationRepo;
        private readonly IUserRepository _userRepository;
        private readonly IPetService _petService;

        public AdoptionApplicationService(IRepository<AdoptionApplication> adoptionApplicationRepository,
            IAdoptionApplicationRepository adoptionApplicationRepoInclude,
            IUserRepository userRepository,
            IPetService petService)
        {
            _adoptionApplicationRepository = adoptionApplicationRepository;
            _adoptionApplicationRepo = adoptionApplicationRepoInclude;
            _userRepository = userRepository;
            _petService = petService;
        }

        public List<AdoptionApplication> GetAllAdoptionApplications()
        {
            return _adoptionApplicationRepo.GetAllAdoptionApplications();
        }

        public AdoptionApplication GetAdoptionApplicationById(Guid id)
        {
            return _adoptionApplicationRepo.GetAdoptionApplicationById(id);
        }

        public List<AdoptionApplication> GetAdoptionApplicationsByAdopterId(string adopterId)
        {
            return _adoptionApplicationRepo.GetAdoptionApplicationByAdopterId(adopterId);
        }

        public List<AdoptionApplication> GetAdoptionApplicationsByPetId(Guid petId)
        {
            return _adoptionApplicationRepo.GetAdoptionApplicationByPetId(petId);
        }


        public void AddAdoptionApplication(AdoptionApplication application)
        {

            _adoptionApplicationRepository.Insert(application);
        }

        public void UpdateAdoptionApplication(AdoptionApplication application)
        {
            _adoptionApplicationRepository.Update(application);
        }

        public void DeleteAdoptionApplication(Guid id)
        {
            _adoptionApplicationRepository.Delete(GetAdoptionApplicationById(id));
        }
    }
}
