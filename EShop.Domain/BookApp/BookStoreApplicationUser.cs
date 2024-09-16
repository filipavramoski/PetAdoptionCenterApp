using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.BookApp
{
    public  class BookStoreApplicationUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
       // public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
