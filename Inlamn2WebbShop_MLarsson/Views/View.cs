using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inlamn2WebbShop_MLarsson.Database;
using Inlamn2WebbShop_MLarsson.Models;

namespace Inlamn2WebbShop_MLarsson.Views
{
    class View
    {
        /// <summary>
        /// Listar alla böcker i en boklista
        /// </summary>
        /// <param name="bookList"></param>
        public static void ListBooks(List<Book> bookList)
        {
            foreach (var book in bookList)
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
        /// Listar alla kategorier i en kategorilista
        /// </summary>
        /// <param name="categories"></param>
        public static void ListCategories(List<Category> categories)
        {
            Console.WriteLine("\nCATEGORIES: ");
            foreach (var cat in categories)
            {
                Console.WriteLine(cat.Name);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Listar alla böcker i en kategori.
        /// </summary>
        /// <param name="books"></param>
        public static void ListCategory(List<Book> books)
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

        /// <summary>
        /// Visar information om en användare.
        /// </summary>
        /// <param name="user"></param>
        public static void ListUser(User user)
        {
            Console.WriteLine("USER: ");
            Console.WriteLine($"ID: {user.Id} - {user.Name}");
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

        /// <summary>
        /// Visar meddelande om något inte gått som tänkt. 
        /// </summary>
        /// <returns>false</returns>
        public static bool SomethingWentWrong()
        {
            Console.WriteLine("\nSomething went wrong...");
            return false;
        }
    }
}
