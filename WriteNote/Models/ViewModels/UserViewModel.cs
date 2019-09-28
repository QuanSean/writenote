using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WriteNote.Models.ViewModels
{
    public class UserViewModel
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        public DateTime DatetimeCreate { get; set; }

        [Required]
        public string ForgotPassword { get; set; }

        [Required]
        public int SubscriptionID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string SubscriptionName { get; set; }
    }
}