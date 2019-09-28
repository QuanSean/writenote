using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WriteNote.Models.ViewModels
{
    public class BuyViewModel
    {
        [Key]
        [Required]
        public int BuyID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int SubscriptionID { get; set; }

        [Required]
        public string SubscriptionName { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int CountMonth { get; set; }

        [Required]
        public DateTime DatetimeBought { get; set; }
    }
}