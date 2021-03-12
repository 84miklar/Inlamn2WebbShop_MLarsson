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
            int userId = WebbShopAPI.LogInUser("TestClient", "Codic2021");

            var list = WebbShopAPI.GetCategories("h");
            Helper.ListCategories(list);

            var bookList = WebbShopAPI.GetAvailableBooks(2);
            Helper.ListBooks(bookList);

            var book = WebbShopAPI.GetBook(5);
            Helper.ListBooks(book);

            Console.WriteLine(WebbShopAPI.Ping(2));

            var bookKey = WebbShopAPI.GetBooks("h");
            Helper.ListBooks(bookKey);

            var authors = WebbShopAPI.GetAuthors("a");
            Helper.ListBooks(authors);

            Console.WriteLine(WebbShopAPI.Ping(2));

            WebbShopAPI.BuyBook(2, 2);

            WebbShopAPI.LogOutUser(2);

            WebbShopAPI.Register("Silvia", "TheQueen", "TheQueen");



            int adminId = WebbShopAPI.LogInUser("Administrator", "CodicRulez");

            WebbShopAPI.AddBook(adminId, "Cosmos", "Stephen Hawking", 200, 5);

            WebbShopAPI.SetAmount(adminId, 2, 6);

            var userList = WebbShopAPI.ListUsers(adminId);
            Helper.ListUsers(userList);

            var userListKey = WebbShopAPI.FindUser(adminId, "te");
            Helper.ListUsers(userListKey);

            WebbShopAPI.UpdateBook(adminId, 2, price: 150);

            WebbShopAPI.AddCategory(adminId, "Thriller");

            WebbShopAPI.UpdateCategory(adminId, 2, "Terrifying");

            WebbShopAPI.DeleteCategory(adminId, 5);

            WebbShopAPI.AddUser(adminId, "CalleG", "Knugen");

            WebbShopAPI.DeleteBook(adminId, 4);

            VGWebbShopAPI.SoldItems(adminId);

            VGWebbShopAPI.MoneyEarned(adminId);

            //VGWebbShopAPI.BestCostumer(adminId);

            VGWebbShopAPI.Promote(adminId, 2);

            VGWebbShopAPI.Demote(adminId, 2);

            VGWebbShopAPI.InactivateUser(adminId, 2);

            VGWebbShopAPI.ActivateUser(adminId, 2);

        }

       


    }
}
