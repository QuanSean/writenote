using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace WriteNote.Models
{
    [Table("tb_Notebooks")]
    public class NoteBook
    {
        [Key]
        [Required]
        [Column("ID_NOTEBOOK")]
        public int NotebookID { get; set; }

        [Required]
        [Column("ID_USER")]
        public int UserID { get; set; }

        [Required]
        [Column("TITLE")]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [Column("DATETIME_CREATE")]
        public DateTime DatetimeCreate { get; set; }
    }
}