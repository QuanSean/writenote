using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WriteNote.Models.ViewModels
{
    public class NoteBookViewModel
    {
        [Key]
        [Required]
        public int NotebookID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime DatetimeCreate { get; set; }
    }
}