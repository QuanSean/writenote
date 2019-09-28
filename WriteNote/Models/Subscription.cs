using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WriteNote.Models
{
    [Table("tb_Subscriptions")]
    public class Subscription
    {
        [Key]
        [Required]
        [Column("ID_SUBSCRIPTION")]
        public int SubscriptionID { get; set; }

        [Required]
        [Column("NAME_SUBSCRIPTION")]
        public string SubscriptionName { get; set; }

        [Required]
        [Column("PRICE")]
        public decimal Price { get; set; }
    }
}