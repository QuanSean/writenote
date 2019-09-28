using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WriteNote.Models
{
    [Table("tb_Logs")]
    public class Log
    {
        [Key]
        [Column("ID_LOG")]
        [Required]
        public int LogID { get; set; }
        
        [Required]
        [Column("ID_USER")]
        public int UserID { get; set; }

        [Required]
        [Column("ID_NOTE")]
        public int NoteID { get; set; }

        [Required]
        [Column("DATETIME_CREATE")]
        public DateTime DatetimeCreate { get; set; }
    }
}