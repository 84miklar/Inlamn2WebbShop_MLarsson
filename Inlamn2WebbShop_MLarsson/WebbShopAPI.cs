using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks.Dataflow;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;
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
        /// Metod för att köpa en bok. Kollar om användaren är inloggad och att boken finns.
        /// Kopierar boken till tabellen SoldBooks och skapar koppling mellan den och användaren.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static User BuyBook(int userId, int bookId)
        {
            //todo: Kolla om boken och user finns. Flytta över boken till SoldBooks med DateTime datum och userId. , uppdatera users SessionTimer till DateTime.Now
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            var book = db.Books.Include("Categories").FirstOrDefault(b => b.Id == bookId);

            if (user.SessionTimer! > DateTime.Now.AddMinutes(-10) && book.Amount > 0)
            {
                user.SessionTimer = DateTime.Now;
                book.Amount -= 1;
                if (book.Amount < 1)
                {
                    book.Amount = 0;
                }
                var soldBook = new SoldBook() { Title = book.Title, Author = book.Author, Price = book.Price, PurchasedDate = DateTime.Now, Categories = new List<Category>(), Users = new List<User>() };
                soldBook.Users.Add(user);
                foreach (var cat in book.Categories)
                {
                    soldBook.Categories.Add(cat);
                }
                db.SoldBooks.Add(soldBook);
                db.Update(user);
                db.SaveChanges();
            }
            return user;
        }

        /// <summary>
        /// Hämtar böcker där författarens namn innenhåller 
        /// valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>Lista på böcker</returns>
        public static List<Book> GetAuthors(string keyword)
        {

            return db.Books.Include("Categories").Where(b => b.Author.Contains(keyword)).OrderBy(a => a.Author).ToList();
        }
        /// <summary>
        /// Hämtar böcker in en viss kategori baserad på kategori-id, 
        /// och antal böcker fler än 0.
        /// </summary>
        /// <param name="categoryId"></param>
        public static List<Book> GetAvailableBooks(int categoryId)
        {
            List<Book> books = new List<Book>();
            foreach (var cat in db.Categories.Include("Books").Where(c => c.Id == categoryId))
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
            return db.Books.Include("Categories").FirstOrDefault(b => b.Id == bookId);
        }

        /// <summary>
        /// Hämtar alla böcker som hinnehåller valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>En lista med böcker.</returns>
        public static List<Book> GetBooks(string keyword)
        {
            return db.Books.Include("Categories").Where(b => b.Title.Contains(keyword)).OrderBy(o => o.Title).ToList();
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
        /// Metod som kollar om en användare och lösenord finns. Loggar in genom att starta två DateTime varav den ena stängs av vid utlogg.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>Användar-id. 0 om ingen användare finns.</returns>
        public static int LogInUser(string userName, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Name == userName && u.Password == password && u.IsActive == true);
            if (user == null)
            {
                return 0;
            }
            else
            {
                user.LastLogin = DateTime.Now;
                user.SessionTimer = DateTime.Now;
                db.Users.Update(user);
                db.SaveChanges();
                Console.WriteLine("You have successfully logged in.");
                return user.Id;
            }
        }

        /// <summary>
        /// Metod för att logga ut användare. Stoppar en DateTime för att se hur länge användaren varit aktiv.
        /// </summary>
        /// <param name="id"></param>
        public static void LogOutUser(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id && u.SessionTimer > DateTime.Now.AddMinutes(-10));
            if (user == null)
            {

            }
            else
            {
                user.SessionTimer = DateTime.MinValue;
                db.Users.Update(user);
                db.SaveChanges();
                Console.WriteLine("You have successfully logged out. Welcome back.");
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
                return "Pong";
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
            if (password != passwordVerify)
            {
                Console.WriteLine("Please check your password. Your passwords are not equal.");
                return false;
            }

            var checkUser = db.Users.FirstOrDefault(u => u.Name == name);
            if (checkUser != null)
            {
                Console.WriteLine("User already exists! Try another user name.");
                return false;
            }
            else
            {
                db.Users.Add(new User() { Name = name, Password = password, IsActive = true, IsAdmin = false });
                db.SaveChanges();
                var user = db.Users.FirstOrDefault(u => u.Name == name);
                if (user != null)
                {
                    Console.WriteLine("You are now registred in our shop. Please login to start buying books.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Something went wrong. Please try again.");
                    return false;
                }
            }
        }

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
            var admin = db.Users.FirstOrDefault(a => a.Id == adminId && a.IsAdmin==true);
            var book = db.Books.FirstOrDefault(b => b.Title == title);
            if (admin != null)
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
                        Console.WriteLine("Something went wrong...");
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine("You are not an administrator.");
                return false;
            }
        }

        public static void SetAmount(int adminId, int bookId, int amount)
        {
            var admin = db.Users.FirstOrDefault(a => a.Id == adminId && a.IsAdmin == true);
            var book = db.Books.FirstOrDefault(b => b.Id == bookId);
            if (admin != null)
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
            else
            {
                Console.WriteLine("You are not an administrator.");
            }
        }
    }
}
