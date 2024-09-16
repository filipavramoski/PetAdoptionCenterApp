using EShop.Domain.Models;
using EShop.Domain.Status;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Implementation
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository _shelterRepo;
        private readonly IRepository<Shelter> _shelterRepository;

        public ShelterService(IShelterRepository shelterRepo, IRepository<Shelter> shelterRepository)
        {
            _shelterRepo = shelterRepo;
            _shelterRepository = shelterRepository;
        }

        public List<Shelter> GetAllShelters()
        {
            return _shelterRepo.GetAllShelters();
        }

        public Shelter GetShelterById(Guid id)
        {
            return _shelterRepo.GetShelterById(id);
        }
        public void AddShelter(Shelter shelter)
        {
          
            _shelterRepository.Insert(shelter);
        }

        public void UpdateShelter(Shelter shelter)
        {
            _shelterRepository.Update(shelter);
        }

        public void DeleteShelter(Guid id)
        {
            _shelterRepository.Delete(_shelterRepo.GetShelterById(id));
        }
    }
}
