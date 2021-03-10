using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
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
        /// Hämtar böcker där författarens namn innenhåller 
        /// valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>Lista på böcker</returns>
        public static List<Book> GetAuthors(string keyword)
        {
            
            return db.Books.Include("Categories").Where(b => b.Author.Contains(keyword)).OrderBy(a=> a.Author).ToList();
        }

        public static User BuyBook(int userId, int bookId)
        {
            //todo: Kolla om boken och user finns. Flytta över boken till SoldBooks med DateTime datum och userId. , uppdatera users SessionTimer till DateTime.Now
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            var book = db.Books.Include("Categories").FirstOrDefault(b => b.Id == bookId);
          
            if (user.SessionTimer! > DateTime.Now.AddMinutes(-10) && book.Amount>0)
            {
                user.SessionTimer = DateTime.Now;
                book.Amount -= 1;
                if (book.Amount < 1)
                {
                    book.Amount = 0;
                }
                var soldBook = new SoldBook() { Title = book.Title, Author = book.Author, Price = book.Price, PurchasedDate = DateTime.Now, Categories = new List<Category>(), Users = new List<User>() };
                soldBook.Users.Add(user);
                foreach(var cat in book.Categories)
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
        /// Hämtar alla böcker som hinnehåller valt nyckelord.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>En lista med böcker.</returns>
        public static List<Book> GetBooks(string keyword)
        {
            return db.Books.Include("Categories").Where(b => b.Title.Contains(keyword)).OrderBy(o => o.Title).ToList();
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
            var user = db.Users.FirstOrDefault(u => u.Name == userName && u.Password == password && u.IsActive==true);
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
            }
        }
    }
}
