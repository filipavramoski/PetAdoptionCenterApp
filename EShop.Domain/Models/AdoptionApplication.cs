using EShop.Domain.Identity;
using EShop.Domain.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models
{
    public class AdoptionApplication : BaseEntity
    {
        public PetAdoptionApplicationStatus AdoptionApplicationStatus { get; set; }
        public string? AdopterId { get; set; }
        public PetAdoptionCenterUser? Adopter { get; set; }
        public Guid? PetId { get; set; }
        public Pet? Pet { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? AdoptionDate { get; set; }
    }
}
