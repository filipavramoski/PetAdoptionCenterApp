using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interface
{
    public interface IAdoptionApplicationService
    {
        List<AdoptionApplication> GetAllAdoptionApplications();
        AdoptionApplication GetAdoptionApplicationById(Guid id);
        List<AdoptionApplication> GetAdoptionApplicationsByAdopterId(string adopterId);
        List<AdoptionApplication> GetAdoptionApplicationsByPetId(Guid petId);

        void AddAdoptionApplication(AdoptionApplication application);
        void UpdateAdoptionApplication(AdoptionApplication application);
        void DeleteAdoptionApplication(Guid id);
    }
}
