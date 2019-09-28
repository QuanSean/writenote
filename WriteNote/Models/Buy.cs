using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WriteNote.Models
{
    [Table("tb_Buys")]
    public class Buy
    {
        [Key]
        [Required]
        [Column("ID_BUY")]
        public int BuyID { get; set; }

        [Required]
        [Column("ID_USER")]
        public int UserID { get; set; }

        [Required]
        [Column("ID_SUBSCRIPTION")]
        public int SubscriptionID { get; set; }

        [Required]
        [Column("PRICE")]
        public int Price { get; set; }

        [Required]
        [Column("COUNT_MONTH")]
        public int CountMonth { get; set; }

        [Required]
        [Column("DATETIME_BOUGHT")]
        public DateTime DatetimeBought { get; set; }
    }
}