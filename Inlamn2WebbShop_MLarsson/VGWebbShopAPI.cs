using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;
using Inlamn2WebbShop_MLarsson.Views;
using Microsoft.EntityFrameworkCore;

namespace Inlamn2WebbShop_MLarsson
{
    public static class VGWebbShopAPI
    {
        /// <summary>
        /// Öpnnar en koppling till databasen, för att kunna använda i hela klassen.
        /// </summary>
        private static WebbShopContext db = new WebbShopContext();

        /// <summary>
        /// Aktiverar en inaktiverad användare.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <returns>true om lyckad, annars false</returns>
        public static bool ActivateUser(int adminId, int userId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.IsActive = true;
                    db.SaveChanges();
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsActive)))
                    {
                        Console.WriteLine($"\n{user.Name} is now an active user.");
                        return true;
                    }
                }
                return View.SomethingWentWrong();
            }
            return false;
        }

        /// <summary>
        /// Visar den kund som köpt flest böcker.
        /// </summary>
        /// <param name="adminId"></param>
        public static User BestCustomer(int adminId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var customer = db.Users.Include(s => s.SoldBooks).OrderByDescending(b => b.SoldBooks.Count()).FirstOrDefault();
                
                Console.WriteLine($"\nBEST CUSTOMER:\nAmount of books bought: {customer.SoldBooks.Count()}");
                return customer;
            }
            return null;
        }
       
        /// <summary>
        /// Gör en administratör till vanlig användare.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <returns>true om lyckad, annars false.</returns>
        public static bool Demote(int adminId, int userId)
        {
            var admin = db.Users.FirstOrDefault(a => a.Id == adminId && a.IsAdmin);

            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.IsAdmin = false;
                    db.SaveChanges();
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsAdmin == false)))
                    {
                        Console.WriteLine($"\n{user.Name} is now demoted to normal user.");
                        return true;
                    }
                }
                return View.SomethingWentWrong();
            }
            return false;
        }

        /// <summary>
        /// Inaktiverar en aktiv användare.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <returns>true om lyckad, annars false.</returns>
        public static bool InactivateUser(int adminId, int userId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.IsActive = false;
                    db.SaveChanges();
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsActive == false)))
                    {
                        Console.WriteLine($"\n{user.Name} is now an inactivated user.");
                        return true;
                    }

                }
                return View.SomethingWentWrong();
            }
            return false;
        }

        /// <summary>
        /// Visar totalsumman av alla sålda böcker.
        /// </summary>
        /// <param name="adminId"></param>
        public static void MoneyEarned(int adminId) 
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var bestBook = from b in db.SoldBooks
                               group b by b.Title into g
                               select new
                               {
                                   Title = g.Key,
                                   BookSum = g.Sum(a => a.Price)
                               };
                
                var totalSum = db.SoldBooks.Sum(p => p.Price);
                
                Console.WriteLine("\nTOTAL MONEY EARNED BY SOLD BOOKS:");
                Console.WriteLine($"    {totalSum}kr");
                foreach (var book in bestBook)
                {
                    Console.WriteLine($"SOLD BOOK: {book.Title} MONEY EARNED: {book.BookSum}");
                }

            }
        }

        /// <summary>
        /// Gör en användare till administratör.
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <returns>true om lyckad, annars false.</returns>
        public static bool Promote(int adminId, int userId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.IsAdmin = true;
                    db.SaveChanges();
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsAdmin)))
                    {
                        Console.WriteLine($"\n{user.Name} is now promoted to administrator.");
                        return true;
                    }
                }
                return View.SomethingWentWrong();
            }
            return false;
        }

        /// <summary>
        /// Listar alla sålda böcker.
        /// </summary>
        /// <param name="adminId"></param>
        public static void SoldItems(int adminId)
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                Console.WriteLine();
                Console.WriteLine("\nSOLD BOOKS:");
                foreach (var soldBook in db.SoldBooks.Include(u => u.Users))
                {
                    Console.WriteLine($"{soldBook.Title} - Date: {soldBook.PurchasedDate}");
                    foreach (var buyer in soldBook.Users)
                    {
                        Console.WriteLine($"    Customer: {buyer.Name}");
                    }
                }
            }
        }
    }
}
