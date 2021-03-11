using System;
using Inlamn2WebbShop_MLarsson.Controllers;
using Inlamn2WebbShop_MLarsson.Database;

namespace Inlamn2WebbShop_MLarsson
{
    class Program
    {
        static void Main(string[] args)
        {
           
            DatabaseCreator.Create();
            Seeder.Seed();
            //int userId = WebbShopAPI.LogInUser("TestClient", "Codic2021");
            // var list =  WebbShopAPI.GetCategories("h");
            //Helper.ListCategories(list);

            //var list = WebbShopAPI.GetAvailableBooks(2);
            //Helper.ListBooks(list);

            //var book = WebbShopAPI.GetBook(5);
            //Helper.ListBooks(book);
            //Console.WriteLine(WebbShopAPI.Ping(2));

            //var book = WebbShopAPI.GetBooks("h");
            //Helper.ListBooks(book);

            //var authors = WebbShopAPI.GetAuthors("a");
            //Helper.ListBooks(authors);
            //Console.WriteLine(WebbShopAPI.Ping(2));
            
            //WebbShopAPI.BuyBook(2, 5);
            
            //WebbShopAPI.LogOutUser(2);

            //WebbShopAPI.Register("Silvia", "TheQueen", "TheQueen");



            int adminId = WebbShopAPI.LogInUser("Administrator", "CodicRulez");

            //WebbShopAPI.AddBook(adminId, "Cosmos", "Stephen Hawking", 200, 5);

            //WebbShopAPI.SetAmount(adminId, 1, 6);

            //var userList = WebbShopAPI.ListUsers(adminId);
            //Helper.ListUsers(userList);

            //var userList = WebbShopAPI.FindUser(adminId, "te");
            //Helper.ListUsers(userList);

            //WebbShopAPI.UpdateBook(adminId, 2, price: 150);

            // WebbShopAPI.AddCategory(adminId, "Thriller");

            //WebbShopAPI.UpdateCategory(adminId, 2, "Horror");

            //WebbShopAPI.DeleteCategory(adminId, 5);

            //WebbShopAPI.AddUser(adminId, "CalleG", "Knugen");

            WebbShopAPI.DeleteBook(adminId, 4);
        }

        //TODO: .Trim() på alla användarnamn och lösenord.
      

    }
}
