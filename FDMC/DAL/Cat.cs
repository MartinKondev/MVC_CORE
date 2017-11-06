using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    public class Cat
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Range(0, 25)]
        public string Age { get; set; }

        [Required]
        public string Breed { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
