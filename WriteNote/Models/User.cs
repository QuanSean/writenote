using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WriteNote.Models
{
    [Table("tb_Users")]
    public class User
    {
        [Key]
        [Required]
        [Column("ID_USER")]
        public int UserID { get; set; }

        [Column("USERNAME")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [Column("PASSWORD")]
        [StringLength(255/*, MinimumLength = 6*/)]
        public string Password { get; set; }

        [Required]
        [Column("EMAIL")]
        [StringLength(255)]
        public string Email { get; set; }

        [Column("FULLNAME")]
        [StringLength(255)]
        public string Fullname { get; set; }

        [Required]
        [Column("DATETIME_CREATE")]
        public DateTime DatetimeCreate { get; set; }

        [Column("FORGOT_PASSWORD")]
        public string ForgotPassword { get; set; }

        [Required]
        [Column("ID_SUBSCRIPTION")]
        public int SubscriptionID { get; set; }

        [Required]
        [Column("ID_ROLE")]
        public int RoleID { get; set; }
    }
}