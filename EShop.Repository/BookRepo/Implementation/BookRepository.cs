using EShop.Domain.BookApp;
using EShop.Repository.BookRepo.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Repository.BookRepo.Implementation
{
    public class BookRepository: IBookRepository
    {
        private readonly BooksDbContext context;
        private readonly DbSet<Book> entities;

        public BookRepository(BooksDbContext context)
        {
            this.context = context;
            entities = context.Set<Book>();
        }

        public IEnumerable<Book> GetAllBooks()
        {
           return entities.Include(x => x.Author).Include(x => x.Publisher).AsEnumerable();
        }
    }
}
