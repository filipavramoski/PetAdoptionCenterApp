using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Models
{
    public class Shelter:BaseEntity
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public List<Pet>? Pets { get; set; }
    }
}
