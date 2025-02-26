using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_ORM
{
    class SaleViewModel
    {
        public string? Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string? Country { get; set; }
        public string? CategoryName { get; set; }
    }
}
