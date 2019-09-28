using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WriteNote.Models.ViewModels;

namespace WriteNote.Models
{
    [Table("tb_Notes")]
    public class Note
    {
        [Key]
        [Required]
        [Column("ID_NOTE")]
        public int NoteID { get; set; }

        [Required]
        [Column("ID_USER")]
        public int UserID { get; set; }

        [Required]
        [Column("TITLE")]
        [StringLength(255)]
        public string Title { get; set; }

        [Column("BODY")]
        public string Body { get; set; }

        [Required]
        [Column("ID_NOTEBOOK")]
        public int NotebookID { get; set; }

        [Required]
        [Column("IS_DELETE")]
        public int IsDeleted { get; set; }

        [Required]
        [Column("DATETIME_CREATE")]
        public DateTime DatetimeCreate { get; set; }

        [Required]
        [Column("DATETIME_UPDATE")]
        public DateTime DatetimeUpdate { get; set; }
    }
}