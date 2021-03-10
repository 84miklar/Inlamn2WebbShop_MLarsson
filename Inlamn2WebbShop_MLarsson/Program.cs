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

            // var list =  WebbShopAPI.GetCategories("h");
            //Helper.ListCategories(list);

            //var list = WebbShopAPI.GetAvailableBooks(2);
            //Helper.ListBooks(list);

            //var book = WebbShopAPI.GetBook(5);
            // Helper.ListBooks(book);

            //var book = WebbShopAPI.GetBooks("h");
            //Helper.ListBooks(book);

            //var authors = WebbShopAPI.GetAuthors("a");
            //Helper.ListBooks(authors);
            WebbShopAPI.LogInUser("TestClient", "Codic2021");
            WebbShopAPI.BuyBook(2, 5);

            //Todo: Skapa en uservariabel och skicka med i metoderna. KOlla om SessionTimer inte är mer än 15 min från LastLogin. Annars Login.

        }
    }
}
