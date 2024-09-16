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
    public class PetRepository : IPetRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Pet> entities;

        public PetRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Pet>();
        }
        public List<Pet> GetAllPets()
        {
            return entities
                .Include(x => x.Shelter)
                .ToList();
        }

        public Pet GetPetById(Guid id)
        {
            return entities
                .Include(z => z.Shelter)
                .SingleOrDefaultAsync(x => x.Id == id).Result;

            // .Include(x=>x.Shelter.Id)
        }

        public List<Pet> GetPetsByShelter(Guid shelterId)
        {
            return entities
               .Include(x => x.Shelter)
               .Where(x => x.Shelter.Id.Equals(shelterId))
               .ToList();
        }
    }
}
