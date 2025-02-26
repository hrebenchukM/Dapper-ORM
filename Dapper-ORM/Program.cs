using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;

namespace Dapper_ORM
{

    class MainClass
    {
        static string? connectionString;

        static void Main()
        {
            var builder = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builder.SetBasePath(path);
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");

            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1. Показать все покупателей");
                    Console.WriteLine("2. Показать email всех покупателей");
                    Console.WriteLine("3. Показать список разделов");
                    Console.WriteLine("4. Показать список акционных товаров");
                    Console.WriteLine("5. Показать все города");
                    Console.WriteLine("6. Показать все страны");
                    Console.WriteLine("7. Показать всех покупателей из конкретного города");
                    Console.WriteLine("8. Показать всех покупателей из конкретной страны");
                    Console.WriteLine("9. Показать все акции для конкретной страны");
                    Console.WriteLine("0. Выход");
                    int result = int.Parse(Console.ReadLine()!);
                    switch (result)
                    {
                        case 1:
                            ShowAllCustomers();
                            break;
                        case 2:
                            ShowAllEmails();
                            break;
                        case 3:
                            ShowAllCategories();
                            break;
                        case 4:
                            ShowAllSales();
                            break;
                        case 5:
                            ShowAllCities();
                            break;
                        case 6:
                            ShowAllCountries();
                            break;
                        case 7:
                            ShowAllCustomersByCity();
                            break;
                        case 8:
                            ShowAllCustomersByCountry();
                            break;
                        case 9:
                            ShowAllSalesByCountry();
                            break;
                        case 0:
                            return;
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       

        static void ShowAllEmails()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var customers = db.Query<CustomerViewModel>( "SELECT email FROM customers");

                int iter = 0;
                foreach (var st in customers)
                {
                    Console.Write($"Покупатель #{++iter}{st.Email,15}");

                }
                Console.WriteLine();
              
            }
            Console.ReadKey();
        }
        static void ShowAllCustomers()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var customers = db.Query<CustomerViewModel>("SELECT name, date_birth, gender, email, country, city FROM customers");

                int iter = 0;
                foreach (var st in customers)
                {
                    Console.Write($"Покупатель #{++iter}{st.Name,15}");
                    Console.Write($"{st.DateBirth,10}");
                    Console.Write($"{st.Gender,10}");
                    Console.Write($"{st.Email,10}");
                    Console.WriteLine($"{st.Country,10}");
                    Console.WriteLine($"{st.City,10}");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }


        static void ShowAllCategories()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var categories = db.Query<Category>("SELECT * FROM categories");
                int iter = 0;
                foreach (var category in categories)
                    Console.WriteLine($"Категория #{++iter} {category.Name}");
            }
            Console.ReadKey();
        }


        static void ShowAllSales()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sales = db.Query<SaleViewModel>(
                "SELECT sales.name, sales.date_start, sales.date_end, sales.country, categories.name AS CategoryName " +
                "FROM categories INNER JOIN sales ON categories.Id = sales.category_id");

                int iter = 0;
                foreach (var st in sales)
                {
                    Console.Write($"Акция #{++iter}{st.Name,15}");
                    Console.Write($"{st.DateStart,15}");
                    Console.Write($"{st.DateEnd,10}");
                    Console.Write($"{st.Country,10}");
                    Console.Write($"{st.CategoryName,10}");
                }
                Console.WriteLine();

            }
            Console.ReadKey();
        }

        static void ShowAllCities()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {

                var cities = db.Query<string>("SELECT city FROM customers");

                int iter = 0;
                foreach (var city in cities)
                {
                    Console.Write($"Город #{++iter}{city,15}");
                }
            }
            Console.ReadKey();
        }


        static void ShowAllCountries()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countries = db.Query<string>("SELECT country FROM customers");

                int iter = 0;
                foreach (var country in countries)
                {
                    Console.Write($"Страна #{++iter}{country,15}");
                }
            }
            Console.ReadKey();
        }

        static void ShowAllCustomersByCity()
        {
            Console.Clear();
            

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите город: ");
                string city = Console.ReadLine()!;

                var customers = db.Query<CustomerViewModel>(
                    "SELECT name, date_birth, gender, email, country, city FROM customers WHERE city = @City",
                    new { City = city });

                int iter = 0;
                foreach (var st in customers)
                {
                    Console.Write($"Покупатель #{++iter}{st.Name,15}");
                    Console.Write($"{st.DateBirth,10}");
                    Console.Write($"{st.Gender,10}");
                    Console.Write($"{st.Email,10}");
                    Console.WriteLine($"{st.Country,10}");
                    Console.WriteLine($"{st.City,10}");
                }
            }
         

            Console.ReadKey();
        }

        static void ShowAllCustomersByCountry()
        {
            Console.Clear();
        
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите страну:");
                string country = Console.ReadLine()!;

                var customers = db.Query<CustomerViewModel>(
                    "SELECT name, date_birth, gender, email, country, city FROM customers WHERE country = @Country",
                    new { Country = country });

                int iter = 0;
                foreach (var st in customers)
                {
                    Console.Write($"Покупатель #{++iter}{st.Name,15}");
                    Console.Write($"{st.DateBirth,10}");
                    Console.Write($"{st.Gender,10}");
                    Console.Write($"{st.Email,10}");
                    Console.WriteLine($"{st.Country,10}");
                    Console.WriteLine($"{st.City,10}");
                }
            }
            Console.ReadKey();
        }

        static void ShowAllSalesByCountry()
        {
            Console.Clear();
            

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите страну:");
                string country = Console.ReadLine()!;

                var sales = db.Query<SaleViewModel>(
                    "SELECT sales.name, sales.date_start, sales.date_end, sales.country, categories.name AS CategoryName " +
                    "FROM categories INNER JOIN sales ON categories.Id = sales.category_id WHERE sales.country = @Country",
                    new { Country = country });

                int iter = 0;
                foreach (var st in sales)
                {
                    Console.Write($"Акция #{++iter}{st.Name,15}");
                    Console.Write($"{st.DateStart,15}");
                    Console.Write($"{st.DateEnd,10}");
                    Console.Write($"{st.Country,10}");
                    Console.Write($"{st.CategoryName,10}");
                }
            }
            Console.ReadKey();
        }

    }
}