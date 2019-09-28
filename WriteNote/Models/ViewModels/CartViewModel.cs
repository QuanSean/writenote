using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WriteNote.Models.ViewModels
{
    public class CartViewModel
    {
        [Key]
        [Required]
        public int SubscriptionID { get; set; }

        [Required]
        public string SubscriptionName { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Count { get; set; }
    }
}