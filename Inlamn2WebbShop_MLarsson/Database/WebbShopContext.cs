using System;
using System.Collections.Generic;
using System.Text;
using Inlamn2WebbShop_MLarsson.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Inlamn2WebbShop_MLarsson.Database
{
    public class WebbShopContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SoldBook> SoldBooks { get; set; }

        /// <summary>
        /// Det är en constructor till min databasklass, och den tar emot en parameter med databasinställningar i
        /// stil med det vi gör i OnConfiguring metoden, i ärlighetens namn, det som skapas i OnConfiguring
        ///metoden är precis det vi skickar in till Constructorn där, den skickar informationen vidare till
        ///huvudklassen.
        /// </summary>
        /// <param name="options"></param>
        public WebbShopContext(DbContextOptions<WebbShopContext> options) : base(options) { }

        /// <summary>
        /// Nästa rad är nästan likadan, det är en constructor som inte tar emot parametrar.
        ///När den kallats kommer den att anropa en metod i klassen ContextFactory för att få databasinställningar
        ///och skickar det sedan vidare till huvudklassen.
        ///Detta behövs egentligen inte, men jag la till det för att slippa krånglig kod.
        /// </summary>
        public WebbShopContext() : base(new ContextFactory().CreateOptions()) { }


    }
}
