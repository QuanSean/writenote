using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace WriteNote.Models
{
    [Table("tb_Roles")]
    public class Role
    {
        [Key]
        [Required]
        [Column("ID_ROLE")]
        public int RoleID { get; set; }

        [Required]
        [Column("NAME_ROLE")]
        public string RoleName { get; set; }
    }
}