using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.BookApp;

namespace EShop.Repository.BookRepo.Interface
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBooks();
        
    }
}
