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
                Console.WriteLine("\nCATEGORIES: ");
                foreach (var cat in categories)
                {
                    Console.WriteLine(cat.Name);
                }
                Console.WriteLine();
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
        public static void ListUsers(List<User> userList)
        {
            Console.WriteLine();
            foreach (var user in userList)
            {
                Console.WriteLine("USER: ");
                Console.WriteLine(user);
                if (user.SoldBooks != null)
                {
                    Console.Write($"Books bought: ");
                    foreach (var soldBook in user.SoldBooks)
                    {
                        Console.WriteLine($"{soldBook.Id} - {soldBook.Title}, ");
                    }
                    Console.WriteLine();
                }
            }
        }
        public static bool DoesCategoryExist(Category cat)
        {
            return cat != null;
        }
        public static bool DoesUserExist(User user)
        {
            return user != null;
        }

        public static bool DoesBookExist(Book book)
        {
            return book != null;
        }
        public static bool CheckIfAdmin(User user)
        {
            if (user.IsAdmin)
            {
                return true;
            }
            else
            {
                Console.WriteLine("You are not an administrator.");
                return false;
            }
        }

    }
}
