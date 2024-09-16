using EShop.Domain.Models;
using EShop.Domain.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IPetService
    {
        List<Pet> GetAllPets();
        Pet GetPetById(Guid id);
        List<Pet> GetPetsByShelter(Guid shelterId);
        Pet AddPet(Pet pet);
        void UpdatePet(Pet pet);
        void DeletePet(Guid id);
        void UpdatePetStatus(Guid petId, PetStatus newStatus);
    }
}
