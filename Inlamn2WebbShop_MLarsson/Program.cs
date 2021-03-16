﻿using System;
using Inlamn2WebbShop_MLarsson.Controllers;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Views;

namespace Inlamn2WebbShop_MLarsson
{
    class Program
    {
        static void Main(string[] args)
        {
            //Skapa databas och fyll på tabeller.
            DatabaseCreator.Create();
            Seeder.Seed();

            #region Go1-User
            //Logga in 
            int userId = WebbShopAPI.LogInUser("TestClient", "Codic2021");

            //Fråga efter kategorier
            var list = WebbShopAPI.GetCategories();
            View.ListCategories(list);

            //Välj kategori Horror
            var category = WebbShopAPI.GetCategory(2);
            View.ListCategory(category);

            //Skicka Ping
            Console.WriteLine(WebbShopAPI.Ping(2));

            //Lista böcker i kategorin Horror, med fler än 0 i antal.
            var bookList = WebbShopAPI.GetAvailableBooks(2);
            View.ListBooks(bookList);

            //Presentera all info boken Dr Sleep
            var book = WebbShopAPI.GetBook(3);
            View.ListBooks(book);

            //Köp Dr Sleep
            WebbShopAPI.BuyBook(userId, 3);

            //Skicka Ping
            Console.WriteLine(WebbShopAPI.Ping(2));

            //Presentera all info boken Dr Sleep för att kontrollera antal
            var checkAmount = WebbShopAPI.GetBook(3);
            View.ListBooks(checkAmount);

            //Logga ut användare.
            WebbShopAPI.LogOutUser(userId);

            Console.WriteLine("*******************");
            Console.ReadLine();
            #endregion Go1-User


            #region Go2-Admin
            //Logga in administratör
            int adminId = WebbShopAPI.LogInUser("Administrator", "CodicRulez");

            //Skapa kategori
            WebbShopAPI.AddCategory(adminId, "Facts");

            //Skapa en bok
            WebbShopAPI.AddBook(adminId, "A Brief History Of Time", "Stephen Hawking", 200, 5);

            //Skicka Ping
            Console.WriteLine(WebbShopAPI.Ping(2));

            //Lägg till bok i kategori
            WebbShopAPI.AddBookToCategory(adminId, 6, 5);

            //Logga ut användare.
            WebbShopAPI.LogOutUser(1);

            Console.WriteLine("*******************");
            Console.ReadLine();

            #endregion Go2-Admin


            #region Go3-Admin
            //Logga in administratör
            adminId = WebbShopAPI.LogInUser("Administrator", "CodicRulez");

            //Lägg till ny användare
            WebbShopAPI.AddUser(adminId, "CalleG", "Knugen");

            //Logga ut användare.
            WebbShopAPI.LogOutUser(adminId);

            Console.WriteLine("*******************");
            Console.ReadLine();

            #endregion Go3-Admin


            #region Go4-User
            //Registrera ny användare
            WebbShopAPI.Register("Silvia", "TheQueen", "TheQueen");

            // Logga in 
            int _userId = WebbShopAPI.LogInUser("Silvia", "TheQueen");

            //Lista kategorier efter keyword
            var cat = WebbShopAPI.GetCategories("h");
            View.ListCategories(cat);

            //Lista böcker efter keyword
            var bookKey = WebbShopAPI.GetBooks("C");
            View.ListBooks(bookKey);

            //Köp en bok
            WebbShopAPI.BuyBook(_userId, 5);
            WebbShopAPI.BuyBook(_userId, 5);
            WebbShopAPI.BuyBook(_userId, 4);

            //Lista alla böcker utefter författar-keyword 
            var authors = WebbShopAPI.GetAuthors("do");
            View.ListBooks(authors);

            //Logga ut användare.
            WebbShopAPI.LogOutUser(_userId);

            Console.WriteLine("*******************");
            Console.ReadLine();
            #endregion Go4-User


            #region Go5- Admin
            //Logga in administratör
             adminId = WebbShopAPI.LogInUser("Administrator", "CodicRulez");

            //Fyll på nya böcker
            WebbShopAPI.SetAmount(adminId, 2, 6);

            //Lista alla användare
            var userList = WebbShopAPI.ListUsers(adminId);
            View.ListUsers(userList);

            //Hitta användare baserat på keyword
            var userListKey = WebbShopAPI.FindUser(adminId, "te");
            View.ListUsers(userListKey);

            //Uppdatera en bok
            WebbShopAPI.UpdateBook(adminId, 2, price: 150);

            //Uppdatera en kategori
            WebbShopAPI.UpdateCategory(adminId, 2, "Thriller");

            //Skapa kategori
            WebbShopAPI.AddCategory(adminId, "Children");

            //Ta bort kategori
            WebbShopAPI.DeleteCategory(adminId, 6);

            //Ta bort bok
            WebbShopAPI.DeleteBook(adminId, 4);

            //Lista alla sålda böcker
            VGWebbShopAPI.SoldItems(adminId);

            //Visa totalsumma av sålda böcker
            VGWebbShopAPI.MoneyEarned(adminId);

            //Visa den användare som köpt flest böcker.
            var customer = VGWebbShopAPI.BestCustomer(adminId);
            View.ListUser(customer);

            VGWebbShopAPI.Promote(adminId, 2);

            VGWebbShopAPI.Demote(adminId, 2);

            VGWebbShopAPI.InactivateUser(adminId, 2);

            VGWebbShopAPI.ActivateUser(adminId, 2);

            //Logga ut användare.
            WebbShopAPI.LogOutUser(adminId);
            #endregion Go5-Admin
        }
    }
}
