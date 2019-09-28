using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace WriteNote.Models
{
    [Table("tb_NoteDeletes")]
    public class NoteDelete
    {
        [Key]
        [Column("ID_NOTEDELETE")]
        [Required]
        public int NoteDeleteID { get; set; }

        [Required]
        [Column("ID_USER")]
        public int UserID { get; set; }

        [Required]
        [Column("ID_NOTE")]
        public int NoteID { get; set; }

        [Required]
        [Column("DATETIME_DELETE")]
        public DateTime DateTimeDelete { get; set; }
    }
}