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
        public int Id { get; private set; }
        [Required]
        public string Jmeno { get; private set; }
        [Required]
        public string Heslo { get; private set; }

        public Uzivatel(string jmeno, string heslo)
        {
            Jmeno = jmeno;
            Heslo = heslo;
        }
    }

    public class DataRegistrace
    {
        public string Jmeno { get; set; }
        public string Heslo { get; set; }
        public string HesloZnovu { get; set; }

        public DataRegistrace(string jmeno, string heslo)
        {
            Jmeno = jmeno;
            Heslo = heslo;
        }
    }

    public class DataPrihlaseni
    {
        public string Jmeno { get; set; }
        public string Heslo { get; set; }

        public DataPrihlaseni(string jmeno, string heslo)
        {
            Jmeno = jmeno;
            Heslo = heslo;
        }
    }
}
