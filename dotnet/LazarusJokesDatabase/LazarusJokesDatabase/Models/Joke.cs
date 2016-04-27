using System;
using System.ComponentModel.DataAnnotations;

namespace LazarusJokesDatabase.Models
{
    public class Joke
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Autor vtipu")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Vtip")]
        [DataType(DataType.MultilineText)]
        public string JokeText { get; set; }

        public int? UserVote { get; set; }
    }
}