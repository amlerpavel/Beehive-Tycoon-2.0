using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BeehiveTycoon.Models
{
    public class Uzivatel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Jmeno { get; set; }
        [Required]
        public string Heslo { get; set; }
        [Required]
        public virtual List<UlozenaHra> UlozeneHry { get; set; }
    }
}
