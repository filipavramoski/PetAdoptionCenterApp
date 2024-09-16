using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.BookApp;

namespace EShop.Service.BookService.Interface
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
    }
}
