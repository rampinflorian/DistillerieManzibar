﻿using System;
using System.ComponentModel.DataAnnotations;
using DistillerieGtaRP.Enums;

namespace DistillerieGtaRP.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        [Required] public string ApplicationUserId { get; set; }
        [Display(Name = "Employé")] public virtual ApplicationUser ApplicationUser { get; set; }
        [Required, Display(Name = "Type de liquide")] public LiquidCategory LiquidCategory { get; set; }
        [Required, Display(Name = "Destination")] public Destination Destination { get; set; }
        [Required, Display(Name = "Quantité")] public int Quantity { get; set; }
        [Required, Display(Name = "Date"), DataType(DataType.Date)]public DateTime CreatedAt { get; set; }
        [Display(Name = "Date paiement")] public DateTime? PayementAt { get; set; }
    }
}