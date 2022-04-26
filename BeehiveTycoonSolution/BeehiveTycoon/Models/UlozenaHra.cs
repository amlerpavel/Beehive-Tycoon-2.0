using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models
{
    public class UlozenaHra
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Postup { get; set; }
        [Required]
        public int Pozice { get; set; }
        [Required]
        public DateTime Datum { get; set; }
        [Required]
        public virtual Uzivatel Uzivatel { get; set; }
    }
}
