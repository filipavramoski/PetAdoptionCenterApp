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
    public class PetService : IPetService
    {
        private readonly IRepository<Pet> _petRepository;
        private readonly IPetRepository _petRepo;
        private readonly IUserRepository _userRepository;
        private readonly IShelterRepository _shelterRepository;

        public PetService(IRepository<Pet> petRepository, IPetRepository petRepo, IUserRepository userRepository, IShelterRepository shelterRepository)
        {
            _petRepository = petRepository;
            _petRepo = petRepo;
            _userRepository = userRepository;
            _shelterRepository = shelterRepository;
        }
        public Pet AddPet(Pet pet)
        {
            pet.Status = PetStatus.NotAdopted;
           /* var shelter = _shelterRepository.GetShelterByPet(pet.Id);
            if (shelter == null)
            {
                throw new ArgumentException("Shelter not found.");
            }

            pet.Shelter = shelter;*/

            
            return this._petRepository.Insert(pet);
        }

        public void DeletePet(Guid id)
        {
            _petRepository.Delete(GetPetById(id));
        }

        public List<Pet> GetAllPets()
        {
            return _petRepo.GetAllPets();
        }

        public Pet GetPetById(Guid id)
        {
            return _petRepo.GetPetById(id);
        }

        public List<Pet> GetPetsByShelter(Guid shelterId)
        {
            return _petRepo.GetPetsByShelter(shelterId);
        }

        public void UpdatePet(Pet pet)
        {
            _petRepository.Update(pet);
        }
        public void UpdatePetStatus(Guid petId, PetStatus newStatus)
        {
           
            var pet = _petRepo.GetPetById(petId);

            if (pet == null)
            {
                throw new ArgumentException("Pet not found.");
            }

           
            pet.Status = newStatus;
            _petRepository.Update(pet);
        }
    }
}
