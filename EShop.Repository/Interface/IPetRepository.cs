using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Interface
{
    public interface IPetRepository
    {
        List<Pet> GetAllPets();
        Pet GetPetById(Guid id);
        List<Pet> GetPetsByShelter(Guid shelterId);
    }
}
