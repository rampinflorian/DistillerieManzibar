﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DistillerieGtaRP.Enums;
using Microsoft.AspNetCore.Identity;

namespace DistillerieGtaRP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }

        [Display(Name = "Taux")]
        public int Percentage { get; set; }
        public AccountStatus AccountStatus { get; set; }
        [NotMapped]
        public ICollection<Command> Commands { get; set; }
    }
}