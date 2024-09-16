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
    public class ShelterRepository : IShelterRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Shelter> entities;

        public ShelterRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Shelter>();
        }
        public Shelter GetShelterById(Guid shelterId)
        {
            return entities
                .Include(s => s.Pets)
                .SingleOrDefault(s => s.Id == shelterId);
        }
        public List<Shelter> GetAllShelters()
        {
            return entities.Include(s => s.Pets).ToList();
        }

        public Shelter GetShelterByPet(Guid petId)
        {
            var pet = context.Pet
            .Include(p => p.Shelter)
            .SingleOrDefault(p => p.Id == petId);

            // Return the Shelter if it exists, otherwise return null
            return pet?.Shelter;
        }
        /*  public void AddShelter(Shelter shelter)
 {
     entities.Add(shelter);
     context.SaveChanges();
 }

 public void UpdateShelter(Shelter shelter)
 {
     entities.Update(shelter);
     context.SaveChanges();
 }

 public void DeleteShelter(Guid id)
 {
     var shelter = GetShelterById(id);
     entities.Remove(shelter);
     context.SaveChanges();
 }*/
    }
}
