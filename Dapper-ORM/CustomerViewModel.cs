using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_ORM
{
    class CustomerViewModel
    {
        public string? Name { get; set; }
        public DateTime? DateBirth { get; set; }
        public string? Gender { get; set; }
        public string Email { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
