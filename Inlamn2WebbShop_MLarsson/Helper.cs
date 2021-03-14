using System;
using System.Collections.Generic;
using System.Text;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;

namespace Inlamn2WebbShop_MLarsson
{
    public static class Helper
    {
        /// <summary>
        /// Listar alla kategorier i en kategorilista
        /// </summary>
        /// <param name="categories"></param>
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

        /// <summary>
        /// Listar alla böcker i en kategori.
        /// </summary>
        /// <param name="books"></param>
        public static void ListCategory(List<Book> books)
        {
            using (var db = new WebbShopContext())
            {
                    Console.WriteLine($"\nBOOKS IN CATEGORY: ");

                foreach (var book in books)
                {

                    foreach (var cat in book.Categories)
                    {
                        Console.WriteLine($"{cat.Name} - {book.Title}");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Listar alla böcker i en boklista
        /// </summary>
        /// <param name="books"></param>
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

        /// <summary>
        /// Presenterar en vald bok och dess kategori.
        /// </summary>
        /// <param name="book"></param>
        public static void ListBooks(Book book)
        {

            foreach (var cat in book.Categories)
            {
                Console.Write("Category: ");
                Console.WriteLine($"{cat.Name} ");
            }
            Console.WriteLine(book);

        }

        /// <summary>
        /// Listar alla användare i en användarlista, inklusive admin.
        /// </summary>
        /// <param name="userList"></param>
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
        public static void ListUser(User user)
        {
            Console.WriteLine();
           
                Console.WriteLine("USER: ");
                Console.WriteLine(user);
                if (user.SoldBooks != null)
                {
                    Console.Write($"Books bought: ");
                    foreach (var soldBook in user.SoldBooks)
                    {
                        Console.Write($"{soldBook.Title}, ");
                    }
                    Console.WriteLine();
                }
            
        }

        /// <summary>
        /// Hjälpmetod för att se om en kategori finns.
        /// </summary>
        /// <param name="cat"></param>
        /// <returns>True om kategorin finns, annars false.</returns>
        public static bool DoesCategoryExist(Category cat)
        {
            return cat != null;
        }

        /// <summary>
        /// Hjälpmetod för att se om en användare finns.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True om användare finns, annars false.</returns>
        public static bool DoesUserExist(User user)
        {
            return user != null;
        }

        /// <summary>
        /// Hjälpmetod för att se om en bok finns.
        /// </summary>
        /// <param name="book"></param>
        /// <returns>True om bok finns, annars false.</returns>
        public static bool DoesBookExist(Book book)
        {
            return book != null;
        }

        /// <summary>
        /// Hjälpmetod för att se om en användare är administratör.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True om användare är admininstratör, annars false.</returns>
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
