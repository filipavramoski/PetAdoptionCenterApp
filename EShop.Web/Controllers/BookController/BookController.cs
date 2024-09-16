using EShop.Domain.Models;
using EShop.Service.BookService.Interface;
using Microsoft.AspNetCore.Mvc;
using EShop.Domain.BookApp;

namespace EShop.Web.Controllers.BookController
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly HttpClient _httpClient;

        public BookController(IBookService bookService, IHttpClientFactory httpClientFactory)
        {
            _bookService = bookService;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IActionResult Index()
        {
            var test = _bookService.GetAllBooks().ToList();
            return View(_bookService.GetAllBooks().ToList());
        }

        /*public async Task<IActionResult> Index()
        {
            try
            {
                string URL = "https://isproject2024.azurewebsites.net/api/books"; // Adjust this URL to the correct endpoint
                HttpResponseMessage response = await _httpClient.GetAsync(URL);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<IEnumerable<Book>>();
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                
                return View("Error");
            }
        }*/
    }
}
