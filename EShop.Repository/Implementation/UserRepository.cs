using EShop.Domain.Identity;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<PetAdoptionCenterUser> entities;


        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<PetAdoptionCenterUser>();
        }
        public IEnumerable<PetAdoptionCenterUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public PetAdoptionCenterUser Get(string id)
        {
            return entities
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(PetAdoptionCenterUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(PetAdoptionCenterUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(PetAdoptionCenterUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

    }
}
