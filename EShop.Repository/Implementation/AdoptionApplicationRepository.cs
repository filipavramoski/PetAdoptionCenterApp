using EShop.Domain.Models;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Implementation
{
    public class AdoptionApplicationRepository : IAdoptionApplicationRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<AdoptionApplication> entities;

        public AdoptionApplicationRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<AdoptionApplication>();
        }
        public List<AdoptionApplication> GetAdoptionApplicationByAdopterId(string? AdopterId)
        {
            if (string.IsNullOrEmpty(AdopterId))
            {
                throw new ArgumentException("Adopter ID must be provided.", nameof(AdopterId));
            }

            return entities
                .Include(a => a.Pet)
                .Where(a => a.AdopterId == AdopterId)
                .ToList();
        }

        public AdoptionApplication GetAdoptionApplicationById(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "ID cannot be null.");
            }

            return entities
                .Include(a => a.Adopter)
                .Include(a => a.Pet)
                .SingleOrDefault(a => a.Id == id);
        }

        public List<AdoptionApplication> GetAdoptionApplicationByPetId(Guid? petId)
        {
            if (petId == null)
            {
                throw new ArgumentNullException(nameof(petId), "Pet ID cannot be null.");
            }

            return entities
                .Include(a => a.Adopter)
                .Where(a => a.PetId == petId)
                .ToList();
        }

        public List<AdoptionApplication> GetAllAdoptionApplications()
        {
            return entities
               .Include(a => a.Adopter)
               .Include(a => a.Pet)
               .ToList();
        }
    }
}
