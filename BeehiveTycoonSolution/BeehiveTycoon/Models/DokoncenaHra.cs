using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BeehiveTycoon.Models
{
    public class DokoncenaHra
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ObtiznostId { get; set; }
        [Required]
        public int Rok { get; set; }
        [Required]
        public int Mesic { get; set; }
        [Required]
        public DateTime Datum { get; set; }
        [Required]
        public virtual Uzivatel Uzivatel { get; set; }
    }
}
