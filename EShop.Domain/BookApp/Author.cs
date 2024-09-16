using EShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.BookApp
{
    public class Author : BaseEntity
    {
        public string FullName => $"{FirstName} {LastName}";
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
