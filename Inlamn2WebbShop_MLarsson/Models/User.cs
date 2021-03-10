﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inlamn2WebbShop_MLarsson.Models
{
    /// <summary>
    /// Kundtabell
    /// </summary>
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime SessionTimer { get; set; } 
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public List<SoldBook> SoldBooks { get; set; }


    }
}