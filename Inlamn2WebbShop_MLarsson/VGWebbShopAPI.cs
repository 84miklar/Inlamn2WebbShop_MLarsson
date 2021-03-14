using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;
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
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsActive == true)))
                    {
                        Console.WriteLine($"{user.Name} is now an active user.");
                        return true;
                    }
                }
                Console.WriteLine("Something went wrong...");
                return false;
            }
            return false;
        }

        /// <summary>
        /// Visar den kund som köpt flest böcker.
        /// </summary>
        /// <param name="adminId"></param>
        public static User BestCustomer(int adminId) //TODO: best costumer
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var customer = db.Users.Include(s => s.SoldBooks).OrderByDescending(b => b.SoldBooks.Count()).FirstOrDefault();
                
                Console.WriteLine($"BEST CUSTOMER:\nAmount of books bought: {customer.SoldBooks.Count()}");
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
            var admin = db.Users.FirstOrDefault(a => a.Id == adminId && a.IsAdmin == true);

            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.IsAdmin = false;
                    db.SaveChanges();
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsAdmin == false)))
                    {
                        Console.WriteLine($"{user.Name} is now demoted to normal user.");
                        return true;
                    }
                }
                Console.WriteLine("Something went wrong...");
                return false;
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
                        Console.WriteLine($"{user.Name} is now an inactivated user.");
                        return true;
                    }

                }
                Console.WriteLine("Something went wrong...");
                return false;
            }
            return false;
        }

        /// <summary>
        /// Visar totalsumman av alla sålda böcker.
        /// </summary>
        /// <param name="adminId"></param>
        public static void MoneyEarned(int adminId) //TODO: Eventuellt totalsumma per boktitel. Vilken bok som säljer mest.
        {
            if (Helper.CheckIfAdmin(db.Users.FirstOrDefault(a => a.Id == adminId)))
            {
                var totalSum = db.SoldBooks.Sum(p => p.Price);
                Console.WriteLine();
                Console.WriteLine("TOTAL MONEY EARNED BY SOLD BOOKS:");
                Console.WriteLine($"    {totalSum}kr");
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
                    if (Helper.DoesUserExist(db.Users.FirstOrDefault(u => u.Id == userId && u.IsAdmin == true)))
                    {
                        Console.WriteLine($"{user.Name} is now promoted to administrator.");
                        return true;
                    }
                }
                Console.WriteLine("Something went wrong...");
                return false;
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
                Console.WriteLine("SOLD BOOKS:");
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
