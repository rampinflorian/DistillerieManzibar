﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DistillerieGtaRP.Enums;

namespace DistillerieGtaRP.Models
{
    public class Command
    {
        public int CommandId { get; set; }

        [Required, Display(Name = "Type de liquide")]
        public LiquidCategory LiquidCategory { get; set; }

        [Required, Display(Name = "Création")] public DateTime CreatedAt { get; set; }
        [Required, Display(Name = "Quantité")] public int Quantity { get; set; }
        [Display(Name = "Payé le")] public DateTime? BilledAt { get; set; }
        [Display(Name = "Livré le")] public DateTime? DeliveryAt { get; set; }
        [Required] public int CompanyId { get; set; }
        [Display(Name = "Société")] public Company Company { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        [Required]public int PricingId { get; set; }
        public Pricing Pricing { get; set; }
        
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser PayementRecipient { get; set; }
    }
}