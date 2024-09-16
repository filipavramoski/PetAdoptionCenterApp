using EShop.Domain.Models;
using EShop.Domain.Status;
using EShop.Repository.BookRepo.Interface;
using EShop.Repository.Interface;
using EShop.Service.BookService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.BookApp;

namespace EShop.Service.BookService.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }

       /* public Book GetBookDetails(Guid bookId)
        {
            return _bookRepository.Get(bookId);
        }

        public bool AddBook(Book book)
        {
            try
            {
                _bookRepository.Insert(book);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateBook(Book book)
        {
            try
            {
                _bookRepository.Update(book);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteBook(Guid bookId)
        {
            try
            {
                var book = _bookRepository.Get(bookId);
                if (book != null)
                {
                    _bookRepository.Delete(book);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }*/

    }
}
