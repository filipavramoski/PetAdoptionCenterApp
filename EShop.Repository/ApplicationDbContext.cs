

using EShop.Domain.Identity;
using EShop.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.BookApp;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<PetAdoptionCenterUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pet { get; set; } = default!;
        public DbSet<AdoptionApplication> AdoptionApplication { get; set; } = default!;
        public DbSet<Shelter> Shelters { get; set; } = default!;

    }
    public class BooksDbContext : DbContext
    {
        public DbSet<Book> Books  { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
           : base(options)
        {
        }
    }
}
