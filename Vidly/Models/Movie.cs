using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }


        public DateTime DateAdded { get; set; }

        [Display(Name = "Number in Stock")]
        [Range(minimum: 1, maximum: 20)]
        public int NumberInStocks { get; set; }

        [Display(Name = "Genre")]
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}