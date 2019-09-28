using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WriteNote.Models.ViewModels
{
    public class NoteViewModel
    {
        [Key]
        [Required]
        public int NoteID { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public int NotebookID { get; set; }

        [Required]
        public int IsDelete { get; set; }

        [Required]
        public string NotebookName { get; set; }

        [Required]
        public DateTime DatetimeCreate { get; set; }

        [Required]
        public DateTime DatetimeUpdate { get; set; }
    }
}