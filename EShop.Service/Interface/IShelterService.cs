using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IShelterService
    {
        Shelter GetShelterById(Guid id);
        List<Shelter> GetAllShelters();
        void AddShelter(Shelter shelter);
        void UpdateShelter(Shelter shelter);
        void DeleteShelter(Guid id);
    }
}
