using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WriteNote.Models.ViewModels
{
    public class NoteDeleteViewModel
    {
        [Key]
        [Required]
        public int NoteID { get; set; }

        [Required]
        public string Title { get; set; }
    }
}