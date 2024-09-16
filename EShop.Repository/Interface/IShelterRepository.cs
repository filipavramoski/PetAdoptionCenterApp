using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Interface
{
    public interface IShelterRepository
    {
        Shelter GetShelterById(Guid shelterId);
        Shelter GetShelterByPet(Guid petId);
        List<Shelter> GetAllShelters();
       /* void AddShelter(Shelter shelter);
        void UpdateShelter(Shelter shelter);
        void DeleteShelter(Guid id);*/
    }
}
