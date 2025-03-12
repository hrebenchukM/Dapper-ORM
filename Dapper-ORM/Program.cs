using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;
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

                    Console.WriteLine("1. Показать всеx покупателей");
                    Console.WriteLine("2. Показать email всех покупателей");
                    Console.WriteLine("3. Показать список разделов");
                    Console.WriteLine("4. Показать список акций");
                    Console.WriteLine("5. Показать список товаров");
                    Console.WriteLine("6. Показать все города");
                    Console.WriteLine("7. Показать все страны");
                    Console.WriteLine("8. Показать всех покупателей из конкретного города");
                    Console.WriteLine("9. Показать всех покупателей из конкретной страны");
                    Console.WriteLine("10. Показать все акции для конкретной страны");
                    Console.WriteLine("11. Показать список городов конкретной страны");
                    Console.WriteLine("12. Показать  список разделов конкретного покупателя");
                    Console.WriteLine("13. Показать  список акций конкретного раздела.");
                    Console.WriteLine("14. Добавить новый раздел");
                    Console.WriteLine("15. Добавить новую акцию");
                    Console.WriteLine("16. Добавить новый товар");
                    Console.WriteLine("17. Добавить нового покупателя");
                    Console.WriteLine("18. Редактировать раздел");
                    Console.WriteLine("19. Редактировать акцию");
                    Console.WriteLine("20. Редактировать покупателя");
                    Console.WriteLine("21. Редактировать товар");
                    Console.WriteLine("22. Удалить раздел");
                    Console.WriteLine("23. Удалить акцию");
                    Console.WriteLine("24. Удалить покупателя");
                    Console.WriteLine("25. Удалить товар");

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
                            ShowAllProducts();
                            break;
                        case 6:
                            ShowAllCities();
                            break;
                        case 7:
                            ShowAllCountries();
                            break;
                        case 8:
                            ShowAllCustomersByCity();
                            break;
                        case 9:
                            ShowAllCustomersByCountry();
                            break;
                        case 10:
                            ShowAllSalesByCountry();
                            break;
                        case 11:
                            ShowAllCitiesByCountry();
                            break;
                        case 12:
                            ShowAllCategoriesByCustomer();
                            break;
                        case 13:
                            ShowAllPromoByCategory();
                            break;
                        case 14:
                            AddNewCategory();
                            break;
                        case 15:
                            AddNewSale();
                            break;
                        case 16:
                            AddNewProduct();
                            break;

                        case 17:
                            AddNewCustomer();
                            break;
                        case 18:
                            EditCategory();
                            break;
                        case 19:
                            EditSale();
                            break;

                        case 20:
                            EditCustomer();
                            break;
                        case 21:
                            EditProduct();
                            break;


                        case 22:
                            RemoveCategory();
                            break;
                        case 23:
                            RemoveSale();
                            break;

                        case 24:
                            RemoveCustomer();
                            break;
                        case 25:
                            RemoveProduct();
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
                "SELECT sales.name, sales.date_start AS DateStart, sales.date_end AS DateEnd, sales.country, categories.name AS CategoryName " +
                "FROM categories INNER JOIN sales ON categories.Id = sales.category_id");


                int iter = 0;
                foreach (var st in sales)
                {
                    Console.Write($"Акция #{++iter,-3} {st.Name,-20} ");      
                    Console.Write($"{st.DateStart:yyyy-MM-dd,} ");       
                    Console.Write($"{st.DateEnd:yyyy-MM-dd,} ");           
                    Console.Write($"{st.Country,-15} ");                       
                    Console.WriteLine($"{st.CategoryName,-15}");
                }
                Console.WriteLine();

            }
            Console.ReadKey();
        }



        static void ShowAllProducts()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var products = db.Query<ProductViewModel>(
                "SELECT products.name , products.price, sales.name AS SaleName " +
                "FROM products " +
                "INNER JOIN sales ON products.sale_id = sales.id");
                int iter = 0;
                foreach (var p in products)
                {
                    Console.Write($"Товар #{++iter,-3} {p.Name,-20} ");
                    Console.Write($"{p.Price,-20} ");
                    Console.WriteLine($"{p.SaleName,-15}");
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
                    "SELECT sales.name, sales.date_start AS DateStart, sales.date_end AS DateEnd, sales.country, categories.name AS CategoryName " +
                    "FROM categories INNER JOIN sales ON categories.Id = sales.category_id WHERE sales.country = @Country",
                    new { Country = country });

                int iter = 0;
                foreach (var st in sales)
                {
                    Console.Write($"Акция #{++iter,-3} {st.Name,-20} ");
                    Console.Write($"{st.DateStart:yyyy-MM-dd,} ");
                    Console.Write($"{st.DateEnd:yyyy-MM-dd,} ");
                    Console.Write($"{st.Country,-15} ");
                    Console.WriteLine($"{st.CategoryName,-15}");
                }
            }
            Console.ReadKey();
        }



        static void ShowAllCitiesByCountry()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите страну:");
                string country = Console.ReadLine()!;

                var cities = db.Query<CustomerViewModel>(
                    "SELECT name, date_birth, gender, email, country, city FROM customers WHERE country = @Country",
                    new { Country = country });

                int iter = 0;
                foreach (var c in cities)
                {
                    Console.WriteLine($"{c.City,10}");
                }
            }
            Console.ReadKey();
        }

        static void ShowAllCategoriesByCustomer()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите имейл покупателя: ");
                string email = Console.ReadLine();
                var customer = db.QueryFirstOrDefault<Customer>("SELECT id FROM customers WHERE email = @Email", new { Email = email });


                var categories = db.Query<CategoryViewModel>(
                "SELECT c.name " +
                "FROM customers_categories cc " +
                "JOIN categories c ON cc.category_id = c.id " +
                "WHERE cc.customer_id = @CustomerId",
                new { CustomerId = customer.Id }
                ).ToList();

                int iter = 0;
                foreach (var c in categories)
                {
                    Console.WriteLine($"Категория #{++iter}: {c.Name,-15}");
                }


            }
            Console.ReadKey();
        }

        static void ShowAllPromoByCategory()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите название категории: ");
                string categoryname = Console.ReadLine();

                var category = db.QueryFirstOrDefault<Category>("SELECT * FROM categories WHERE name = @CategoryName",
                new { CategoryName = categoryname });

                var promotions = db.Query<SaleViewModel>("SELECT name, date_start AS DateStart, date_end AS DateEnd FROM sales WHERE category_id = @CategoryId",
                new { CategoryId = category.Id }).ToList();
                int iter = 0;
                foreach (var p in promotions)
                {
                    Console.Write($"Акция #{++iter}{p.Name,15}");
                    Console.Write($"{p.DateStart:yyyy-MM-dd,} ");
                    Console.Write($"{p.DateEnd:yyyy-MM-dd,} ");
                    Console.Write($"{p.Country,10}");
                    Console.WriteLine($"{category?.Name,10}");
                }
            }
            Console.ReadKey();
        }















        static void AddNewCategory()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string categoryname;
                do
                {
                    Console.WriteLine("Введите название новой категории: ");
                    categoryname = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(categoryname.Trim()));
                var category = new Category { Name = categoryname };
                var sqlQuery = "INSERT INTO categories (name) VALUES(@Name)";
                int number = db.Execute(sqlQuery, category);
                if (number != 0)
                    Console.WriteLine($"Категория успешно добавлена!");
            }
            Console.ReadKey();
        }


        static void AddNewProduct()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string name;
                do
                {
                    Console.WriteLine("Введите название товара: ");
                    name = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(name.Trim()));

                Console.WriteLine("Введите цену товара: ");
                decimal productPrice = decimal.Parse(Console.ReadLine()!);


                Console.WriteLine("Введите название акции к которой прнадлежит товар: ");
                string saleName = Console.ReadLine()!;


                var sale = db.QueryFirstOrDefault<Sale>("SELECT * FROM sales WHERE name = @SaleName", new { SaleName = saleName });
                var product = new Product
                {
                    SaleId = sale.Id,
                    Name = name,
                    Price = productPrice
                };

                var sqlQuery = "INSERT INTO products (sale_id, name, price) " +
                "VALUES(@SaleId, @Name, @Price)";

                int number = db.Execute(sqlQuery, product);
                if (number != 0)
                    Console.WriteLine("Товар успешно добавлен!");
            }
            Console.ReadKey();
        }


        static void AddNewCustomer()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string name, email, country, city, gender;
               

                do
                {
                    Console.WriteLine("Введите имя покупателя: ");
                    name = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(name.Trim()));

                Console.WriteLine("Введите дату рождения (в формате YYYY-MM-DD): ");
                DateTime dateOfBirth = DateTime.Parse(Console.ReadLine()!);
              
                Console.WriteLine("Введите пол покупателя: ");
                gender = Console.ReadLine()!;


                do
                {
                    Console.WriteLine("Введите email покупателя: ");
                    email = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(email.Trim()));

                Console.WriteLine("Введите страну покупателя: ");
                country = Console.ReadLine()!;

                Console.WriteLine("Введите город покупателя: ");
                city = Console.ReadLine()!;

                var customer = new
                {
                    Name = name,
                    DateOfBirth = dateOfBirth,
                    Gender = gender,
                    Email = email,
                    Country = country,
                    City = city
                };

                var sqlQuery = "INSERT INTO customers (name, date_birth, gender, email, country, city) " +
                               "VALUES(@Name, @DateOfBirth, @Gender, @Email, @Country, @City)";

                int number = db.Execute(sqlQuery, customer);

                if (number != 0)
                    Console.WriteLine("Покупатель успешно добавлен!");
                
            }

            Console.ReadKey();
        }


        static void AddNewSale()
        {
            Console.Clear();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string name;
                do
                {
                    Console.WriteLine("Введите название акции: ");
                    name = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(name.Trim()));
                 
                Console.WriteLine("Введите дату начала акции: ");
                DateTime dateStart = DateTime.Parse(Console.ReadLine()!);

                Console.WriteLine("Введите дату конца акции: ");
                DateTime dateEnd = DateTime.Parse(Console.ReadLine()!);


                Console.WriteLine("Введите страну проведения акции : ");
                string country = Console.ReadLine()!;

                Console.WriteLine("Введите название категории: ");
                string categoryName = Console.ReadLine()!;


                var category = db.QueryFirstOrDefault<Category>("SELECT * FROM categories WHERE name = @CategoryName", new { CategoryName = categoryName });
                var sale = new Sale
                {
                    Name = name,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    Country = country,
                    CategoryId = category.Id
                };

                var sqlQuery = "INSERT INTO sales (name, date_start, date_end, country, category_id) " +
               "VALUES(@Name, @DateStart, @DateEnd, @Country, @CategoryId)";


                int number = db.Execute(sqlQuery, sale);
                if (number != 0)
                    Console.WriteLine("Акция успешно добавлена!");
            }
            Console.ReadKey();
        }


        static void EditCategory()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string oldCategoryName;
                do
                {
                    Console.WriteLine("Введите старое название категории для редактирования: ");
                    oldCategoryName = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(oldCategoryName.Trim()));


                var category = db.QueryFirstOrDefault<Category>("SELECT * FROM categories WHERE name = @OldCategoryName", new { OldCategoryName = oldCategoryName });
                string categoryname;
                do
                {
                    Console.WriteLine("Введите новое название категории : ");
                    categoryname = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(categoryname.Trim()));
                category.Name = categoryname;

                var sqlQuery = "UPDATE categories SET name = @Name WHERE id = @Id";
               int  number = db.Execute(sqlQuery, category);
                if (number != 0)
                    Console.WriteLine("Раздел успешно изменен!");
            }
            Console.ReadKey();
        }



        static void EditCustomer()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string oldEmail;
                do
                {
                    Console.WriteLine("Введите старый email покупателя для редактирования: ");
                    oldEmail = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(oldEmail.Trim()));


                var customer = db.QueryFirstOrDefault<Customer>("SELECT * FROM customers WHERE email = @OldEmail", new { OldEmail = oldEmail });
                string customerName;
                do
                {
                    Console.WriteLine("Введите новое имя покупателя: ");
                    customerName = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(customerName.Trim()));



                Console.WriteLine("Введите новую дату рождения: ");
                DateTime dateStart = DateTime.Parse(Console.ReadLine()!);

                Console.WriteLine("Введите новый пол: ");
                string gender = Console.ReadLine()!;

                string newEmail;
                do
                {
                    Console.WriteLine("Введите новый email покупателя: ");
                    newEmail = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(newEmail.Trim()));



                Console.WriteLine("Введите новую страну покупателя : ");
                string country = Console.ReadLine()!;

                Console.WriteLine("Введите новый город покупателя: ");
                string city = Console.ReadLine()!;


                customer.Name = customerName;
                customer.DateBirth = dateStart;
                customer.Gender = gender;
                customer.Email = newEmail;
                customer.Country = country;
                customer.City = city;

                var sqlQuery = "UPDATE customers SET name = @Name, date_birth = @DateBirth, gender = @Gender, email = @Email, country = @Country, city = @City WHERE id = @Id";
                int number = db.Execute(sqlQuery, customer);
                if (number != 0)
                    Console.WriteLine("Покупатель успешно изменен!");
            }
            Console.ReadKey();
        }





        static void EditSale()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string oldSaleName;
                do
                {
                    Console.WriteLine("Введите старое название акции для редактирования: ");
                    oldSaleName = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(oldSaleName.Trim()));


                var sale = db.QueryFirstOrDefault<Sale>("SELECT * FROM sales WHERE name = @oldSaleName", new { OldSaleName = oldSaleName });


                string salename;
                do
                {
                    Console.WriteLine("Введите новое название акции : ");
                    salename = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(salename.Trim()));



                Console.WriteLine("Введите новую дату начала акции: ");
                DateTime dateStart = DateTime.Parse(Console.ReadLine()!);

                Console.WriteLine("Введите новую дату конца акции: ");
                DateTime dateEnd = DateTime.Parse(Console.ReadLine()!);


                Console.WriteLine("Введите новую страну проведения акции : ");
                string country = Console.ReadLine()!;

                Console.WriteLine("Введите название категории: ");
                string categoryName = Console.ReadLine()!;


                var category = db.QueryFirstOrDefault<Category>("SELECT * FROM categories WHERE name = @CategoryName", new { CategoryName = categoryName });

                sale.Name = salename;
                sale.DateStart = dateStart;
                sale.DateEnd = dateEnd;
                sale.CategoryId = category.Id;

                var sqlQuery = "UPDATE sales SET name = @Name, date_start = @DateStart, date_end = @DateEnd, " +
                    "category_id = @CategoryId WHERE id = @Id";
                int number = db.Execute(sqlQuery, sale);
                if (number != 0)
                    Console.WriteLine("Акция успешно изменена!");
            }
            Console.ReadKey();
        }




        static void EditProduct()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string oldProductName;
                do
                {
                    Console.WriteLine("Введите старое название товара для редактирования: ");
                    oldProductName = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(oldProductName.Trim()));


                var product = db.QueryFirstOrDefault<Product>("SELECT * FROM products WHERE name = @OldProductName ", new { OldProductName = oldProductName });


                string productname;
                do
                {
                    Console.WriteLine("Введите новое название товара : ");
                    productname = Console.ReadLine()!;
                }
                while (string.IsNullOrEmpty(productname.Trim()));

                Console.WriteLine("Введите новую цену товара : ");
                decimal price = decimal.Parse(Console.ReadLine()!);

                Console.WriteLine("Введите название акции ,к которой относится товар: ");
                string saleName = Console.ReadLine()!;


                var sale = db.QueryFirstOrDefault<Sale>("SELECT * FROM sales WHERE name = @SaleName", new { SaleName = saleName });

                product.SaleId = sale.Id;
                product.Name = productname;
                product.Price = price;

                var sqlQuery = "UPDATE products SET name = @Name, price = @Price, sale_id = @SaleId WHERE id = @Id";
       
                int number = db.Execute(sqlQuery, product);
                if (number != 0)
                    Console.WriteLine("Товар успешно изменен!");
            }
            Console.ReadKey();
        }







        static void RemoveCategory()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите название раздела: ");
                string categoryname = Console.ReadLine()!;
                var category = db.QueryFirstOrDefault<Category>("SELECT * FROM categories WHERE name = @Name", new { Name = categoryname });
                var sqlQuery = "DELETE FROM categories WHERE id = @Id";
                int number = db.Execute(sqlQuery, new { category.Id });
                if (number != 0)
                    Console.WriteLine("Раздел успешно удален!");
            }
            Console.ReadKey();
        }



        static void RemoveCustomer()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите имя покупателя: ");
                string customername = Console.ReadLine()!;
                var customer = db.QueryFirstOrDefault<Customer>("SELECT * FROM customers WHERE name = @Name", new { Name = customername });
                var sqlQuery = "DELETE FROM customers WHERE id = @Id";
                int number = db.Execute(sqlQuery, new { customer.Id });
                if (number != 0)
                    Console.WriteLine("Покупатель успешно удален!");
            }
            Console.ReadKey();
        }


        static void RemoveSale()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите название акции: ");
                string salename = Console.ReadLine()!;
                var sale = db.QueryFirstOrDefault<Sale>("SELECT * FROM sales WHERE name = @Name", new { Name = salename });
                var sqlQuery = "DELETE FROM sales WHERE id = @Id";
                int number = db.Execute(sqlQuery, new { sale.Id });
                if (number != 0)
                    Console.WriteLine("Акция успешно удалена!");
            }
            Console.ReadKey();
        }



        static void RemoveProduct()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите название товара: ");
                string productname = Console.ReadLine()!;
                var product = db.QueryFirstOrDefault<Product>("SELECT * FROM products WHERE name = @Name", new { Name = productname });
                var sqlQuery = "DELETE FROM products WHERE id = @Id";
                int number = db.Execute(sqlQuery, new { product.Id });
                if (number != 0)
                    Console.WriteLine("Товар успешно удален!");
            }
            Console.ReadKey();
        }


    }
}