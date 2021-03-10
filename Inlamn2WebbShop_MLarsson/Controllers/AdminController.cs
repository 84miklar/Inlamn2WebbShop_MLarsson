using System;
using System.Collections.Generic;
using System.Text;

namespace Inlamn2WebbShop_MLarsson.Controllers
{
    class AdminController
    {
        public static void AddBook(int adminId, string title, string author, int price, int amount )
        {
            //Todo: kolla om adminId.IsAdmin == true och person finns. Lägg till bok i DBSet Books.
            //Kolla om boken är inlagd. returnerna true eller false om fail. Om boken fanns innan, öka bookamout med 1, annars sätt till amount.
        }
        public static void SetAmount(int adminId, int bookId)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. Kolla om boken finns. Sätt antal böcker tillgängliga. returnera bok.

        }
        public static void ListUsers(int adminId)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. Foreacha hela userlistan.
        }
        public static void FindUser(int adminId, string keyword)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. Kolla alla personer som har keyword. Foreacha hela listan.
        }
        public static void UpdateBook(int adminId, int bookId, string title="", string author="", int price=0)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. Kolla om boken finns. Ändra allt som inte har "" eller 0. Kolla om det lyckats, returnera true eller false.
        }
        public static void AddCategory(int adminId, string name)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. Kolla om kategori finns, annars lägg till. Kolla om det lyckats, returnera true eller false.
        }
        public static void AddBookToCategory(int adminId, int bookId, int categoryId)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. KOlla om bok finns. Kolla om category finns, annars skapa (addCategory) och lägg till bok i den. Kolla om det lyckats, returnera true eller false.
        }
        public static void UpdateCategory(int adminId, int categoryId, string name)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. KOlla om category finns, isåfall ändra namn. Kolla om det lyckats, returnera true eller false.
        }
        public static void DeleteCategory(int adminId, int categoryId)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. Kolla om category finns, ta bort isåfall. Kolla om det lyckats, returnera true eller false.
        }
        public static void AddUser(int adminId, string name, string password)
        {
            //Todo: kolla om person finns och adminId.IsAdmin == true. kolla om person redan finns, annars lägg till namn och lösenord i lisan. Kolla om det lyckats, returnera true eller false.
        }
    }
    
}
