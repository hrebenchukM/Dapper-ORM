using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_ORM
{
    class Product
    {
        public int Id { get; set; }
        public int? SaleId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
