using System;
using System.Collections.Generic;
using System.Text;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;

namespace Inlamn2WebbShop_MLarsson
{
    public static class Helper
    {
        public static void ListCategories(List<Category> categories)
        {
            using (var db = new WebbShopContext())
            {
                foreach (var cat in categories)
                {
                    Console.WriteLine(cat.Name);
                }

            }
        }
        public static void ListBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                foreach (var cat in book.Categories)
                {
                    Console.Write("Category: ");
                    Console.WriteLine($"{cat.Name} ");
                }
                Console.WriteLine(book);
            }
        }
        public static void ListBooks(Book book)
        {
            
                foreach (var cat in book.Categories)
                {
                    Console.Write("Category: ");
                    Console.WriteLine($"{cat.Name} ");
                }
                Console.WriteLine(book);
            
        }
    }
}
