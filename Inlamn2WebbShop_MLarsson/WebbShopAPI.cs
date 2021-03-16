using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Xml.Xsl;
using Inlamn2WebbShop_MLarsson.Controllers;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;
using Inlamn2WebbShop_MLarsson.Views;
using Microsoft.EntityFrameworkCore;

namespace Inlamn2WebbShop_MLarsson
{
    public static class WebbShopAPI
    {
        /// <summary>
        /// Öpnnar en koppling till databasen, för att kunna använda i hela klassen.
        /// </summary>
        private static WebbShopContext db = new WebbShopContext();

        /// <summary>
        /// Lägg till ny bok.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        /// <returns>true om boken redan finns eller lades till korrekt, annars false.</returns>
        public static bool AddBook(int adminId, string title, string author, int price, int amount)
        {
            var book = db.Books.FirstOrDefault(b => b.Title == title);
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                if (book != null)
                {
                    book.Amount += amount;
                    Console.WriteLine($"The book already exists in store, and the stock is refilled with {amount} books.");
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    db.Books.Add(new Book() { Title = title, Author = author, Price = price, Amount = amount });
                    db.SaveChanges();
                    book = db.Books.FirstOrDefault(b => b.Title == title);
                    if (book != null)
                    {
                        Console.WriteLine($"The book {title} is added to the store.");
                        return true;
                    }
                    else
                    {
                        return View.SomethingWentWrong();
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// tittar om användare är admin, lägger till bok i kategori, 
        /// eller skapar kategori och sedan lägger till.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="categoryName"></param>
        /// <returns>true om bok är tillagd i kategori, annars false</returns>
        public static bool AddBookToCategory(int adminId, int bookId, int categoryId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var book = (from b in db.Books
                            where b.Id == bookId
                            select b).FirstOrDefault();
                var cat = (from c in db.Categories.Include(b=>b.Books)
                           where c.Id == categoryId
                           select c).FirstOrDefault();
                
                if (cat == null || book == null)
                {
                    return View.SomethingWentWrong();
                }
                cat.Books.Add(book);
                db.Update(cat);
                db.SaveChanges();
                Console.WriteLine($"Book added to category {cat.Name}. ");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tittar om användare är admin, lägger till ny kategori om den inte redan finns.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="name"></param>
        /// <returns>true om kategori är tillagd, annars false</returns>
        public static bool AddCategory(int adminId, string name)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                //Testar på LINQ-Query istället för lambda...
                var cat = (from c in db.Categories
                           where c.Name == name
                           select c).FirstOrDefault();
                if (cat == null)
                {
                    db.Categories.Add(new Category() { Name = name });
                    db.SaveChanges();
                    cat = (from c in db.Categories
                           where c.Name == name
                           select c).FirstOrDefault();
                    if (Helper.DoesCategoryExist((from c in db.Categories
                                                  where c.Name == name
                                                  select c).FirstOrDefault()))
                    {
                        Console.WriteLine($"You have added {name} as a category.");
                        return true;
                    }
                    return View.SomethingWentWrong();
                }
            }
            return false;
        }
        /// <summary>
        /// Tittar om användare är admin, lägger till användare
        /// om lösenord inte saknas eller användarnamn inte är upptaget.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>true om ny användare är tillagd, annars false</returns>
        public static bool AddUser(int adminId, string name, string password = "")
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                if (password == "")
                {
                    Console.WriteLine("You need to enter a password!");
                    return false;
                }
                if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Name == name.Trim())))
                {
                    Console.WriteLine("User already exists!");
                    return false;
                }
                else
                {
                    db.Users.Add(new User() { Name = name.Trim(), Password = password, IsActive = true, IsAdmin = false, SoldBooks = new List<SoldBook>() });
                    db.SaveChanges();
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Name == name.Trim())))
                    {
                        Console.WriteLine($"You have added a new user: {name.Trim()} - {password}");
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// Metod för att köpa en bok. Kollar om användaren är inloggad och att boken finns.
        /// Kopierar boken till tabellen SoldBooks och skapar koppling mellan den och användaren.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static User BuyBook(int userId, int bookId)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            var book = db.Books.Include(c=>c.Categories).FirstOrDefault(b => b.Id == bookId);

            if (user.SessionTimer! > DateTime.Now.AddMinutes(-10) && book!=null && book.Amount > 0)
            {
                user.SessionTimer = DateTime.Now;
                book.Amount --;
                if (book.Amount < 1)
                {
                    book.Amount = 0;
                }
                var soldBook = new SoldBook() { Title = book.Title, Author = book.Author, Price = book.Price, 
                                                PurchasedDate = DateTime.Now, Categories = new List<Category>(), Users = new List<User>() };
                soldBook.Users.Add(user);

                foreach (var cat in book.Categories)
                {
                    soldBook.Categories.Add(cat);
                }
                db.SoldBooks.Add(soldBook);
                db.Update(user);
                db.SaveChanges();
                Console.WriteLine($"\nYou have bought {soldBook.Title}\n");
            }
           return user;

        }

        /// <summary>
        ///  Tittar om användare är admin, tar bort bok om den finns.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <returns>true om boken är borttagen, annars false.</returns>
        public static bool DeleteBook(int adminId, int bookId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var book = db.Books.FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                {
                    while (book.Amount > 0)
                    {
                        book.Amount--;
                    }
                    db.Books.Remove(book);
                    db.SaveChanges();

                    if (!Helper.DoesBookExist(db.Books.FirstOrDefault(b => b.Id == bookId)))
                    {
                        Console.WriteLine($"You have deleted book {book.Title}");
                        return true;
                    }
                }
                return View.SomethingWentWrong();
            }
            return false;
        }

        /// <summary>
        ///  Tittar om användare är admin, tar bort kategori om den finns.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="categoryId"></param>
        /// <returns>true om kategorin är borttagen, annars false.</returns>
        public static bool DeleteCategory(int adminId, int categoryId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var cat = db.Categories.Include(b => b.Books).FirstOrDefault(c => c.Id == categoryId);

                if (cat != null && cat.Books.Count() <= 0)
                {
                    db.Categories.Remove(cat);
                    db.SaveChanges();
                    if (!Helper.DoesCategoryExist(db.Categories.FirstOrDefault(c => c.Id == categoryId)))
                    {
                        Console.WriteLine($"You have deleted category {cat.Name}");
                        return true;
                    }
                }
                return View.SomethingWentWrong();
            }
            return false;
        }

        /// <summary>
        /// Tittar om användare är admin, 
        /// och listar alla användare som har keyword in sitt namn.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="keyword"></param>
        /// <returns>Lista på användare</returns>
        public static List<User> FindUser(int adminId, string keyword)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                return db.Users.Include(s => s.SoldBooks).Where(u => u.Name.Contains(keyword)).OrderBy(o => o.Name).ToList();
            }
            return null;
        }

        /// <summary>
        /// Hämtar böcker där författarens namn innenhåller 
        /// valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>Lista på böcker</returns>
        public static List<Book> GetAuthors(string keyword)
        {
            return db.Books.Include(c=>c.Categories).Where(b => b.Author.Contains(keyword)).OrderBy(a => a.Author).ToList();
        }

        /// <summary>
        /// Hämtar böcker in en viss kategori baserad på kategori-id, 
        /// och antal böcker fler än 0.
        /// </summary>
        /// <param name="categoryId"></param>
        public static List<Book> GetAvailableBooks(int categoryId)
        {
            List<Book> books = new List<Book>();
            foreach (var cat in db.Categories.Include(b=>b.Books).Where(c => c.Id == categoryId))
            {
                foreach (var book in cat.Books.Where(b => b.Amount > 0))
                {
                    books.Add(book);
                }
            }
            return books;

        }
        /// <summary>
        /// Hämtar en bok baserad på bok-id.
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>ett Book object. </returns>
        public static Book GetBook(int bookId)
        {
            return db.Books.Include(c=>c.Categories).FirstOrDefault(b => b.Id == bookId);
        }

        /// <summary>
        /// Hämtar alla böcker som hinnehåller valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>En lista med böcker.</returns>
        public static List<Book> GetBooks(string keyword)
        {
            return db.Books.Include(c => c.Categories).Where(b => b.Title.Contains(keyword)).OrderBy(o => o.Title).ToList();
        }

        /// <summary>
        /// Hämtar kategorier som innehåller valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>En lista med kategorier</returns>
        public static List<Category> GetCategories(string keyword)
        {
            return db.Categories.Where(c => c.Name.Contains(keyword)).OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Hämtar alla kategorier.
        /// </summary>
        /// <returns>En lista med kategorier</returns>
        public static List<Category> GetCategories()
        {
            return db.Categories.OrderBy(c => c.Name).ToList();
        }

        /// <summary>
        /// Hämtar böcker i en kategori
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Lista på böcker i en kategori</returns>
        public static List<Book> GetCategory(int categoryId) 
        {
            List<Book> bookList = new List<Book>();
            var catList = db.Categories.Include(b => b.Books).FirstOrDefault(c=>c.Id== categoryId);
            foreach(var book in catList.Books)
            {
                bookList.Add(book);
            }
            return bookList;
        }
        /// <summary>
        /// Tittar om användare är admin,
        /// och listar alla användare som finns.
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns>Lista på användare</returns>
        public static List<User> ListUsers(int adminId)
        {
            var userList = new List<User>();
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                return db.Users.OrderBy(u => u.Name).ToList();
            }
            return null;
        }

        /// <summary>
        /// Metod som kollar om en användare och lösenord finns. Loggar in genom att starta två DateTime varav den ena stängs av vid utlogg.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Användar-id. 0 om ingen användare finns.</returns>
        public static int LogInUser(string userName, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Name == userName.Trim() && u.Password == password && u.IsActive == true);
            if (user != null)
            {
                user.LastLogin = DateTime.Now;
                user.SessionTimer = DateTime.Now;
                db.Users.Update(user);
                db.SaveChanges();
                Console.WriteLine("\nYou have successfully logged in.");
                return user.Id;
            }
            return 0;
        }

        /// <summary>
        /// Metod för att logga ut användare. Stoppar en DateTime för att se hur länge användaren varit aktiv.
        /// </summary>
        /// <param name="id"></param>
        public static void LogOutUser(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id && u.SessionTimer > DateTime.Now.AddMinutes(-10));
            if (user != null)
            {
                user.SessionTimer = DateTime.MinValue;
                db.Users.Update(user);
                db.SaveChanges();
                Console.WriteLine("\nYou have successfully logged out. Welcome back.");
            }
        }
        /// <summary>
        /// KOllar om användaren är inloggad, baserad på SessionTimer.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>"Pong" om användaren är inloggad, annars string.Empty/returns>
        public static string Ping(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            if (user.SessionTimer! > DateTime.Now.AddMinutes(-10))
            {
                return "\nPong\n";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Skapa ny användare.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="passwordVerify"></param>
        /// <returns>True om användaren är skapad, annars false.</returns>
        public static bool Register(string name, string password, string passwordVerify)
        {
            Console.WriteLine();
            if (password != passwordVerify)
            {
                Console.WriteLine("Please check your password. Your passwords are not equal.");
                return false;
            }

            if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Name == name.Trim())))
            {
                Console.WriteLine("User already exists! Try another user name.");
                return false;
            }
            else
            {
                db.Users.Add(new User() { Name = name.Trim(), Password = password, IsActive = true, IsAdmin = false, SoldBooks = new List<SoldBook>() });
                db.SaveChanges();
                if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Name == name.Trim())))
                {
                    Console.WriteLine("You are now registred in our shop. Please login to start buying books.");
                    return true;
                }
                else
                {
                    return View.SomethingWentWrong();
                }
            }
        }
        /// <summary>
        /// Tittar om användare är admin, och lägger till antal böcker till bok.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="amount"></param>
        public static void SetAmount(int adminId, int bookId, int amount)
        {
            var book = db.Books.FirstOrDefault(b => b.Id == bookId);
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                if (book != null)
                {
                    book.Amount += amount;
                    Console.WriteLine($"The book stock is refilled with {amount} books.");
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("No book was found...");
                }
            }
        }

        /// <summary>
        /// Tittar om användare är admin, updaterar bok med ifylld info.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="bookId"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="price"></param>
        /// <returns>true om boken är uppdaterad, annars false</returns>
        public static bool UpdateBook(int adminId, int bookId, string title = "", string author = "", int price = 0)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var book = db.Books.FirstOrDefault(b => b.Id == bookId);
                if (book != null)
                {
                    if (title != "") book.Title = title;
                    if (author != "") book.Author = author;
                    if (price != 0) book.Price = price;
                    db.Update(book);
                    db.SaveChanges();
                    Console.WriteLine("You have updated a book.");
                    return true;
                }
                else
                {
                    return View.SomethingWentWrong();
                }
            }
            return false;
        }
        /// <summary>
        /// Kollar om användare är admin, byter namn på kategori om id finns.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="categoryId"></param>
        /// <param name="newName"></param>
        /// <returns>true om namnet är ändrat, annars false.</returns>
        public static bool UpdateCategory(int adminId, int categoryId, string newName)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var cat = db.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (cat != null)
                {
                    cat.Name = newName;
                    db.Update(cat);
                    db.SaveChanges();
                    if (Helper.DoesCategoryExist(db.Categories.FirstOrDefault(c => c.Name == newName && c.Id == categoryId)))
                    {
                        Console.WriteLine("You have updated a category.");
                        return true;
                    }
                }
                return View.SomethingWentWrong();

            }
            return false;

        }
    }
}
