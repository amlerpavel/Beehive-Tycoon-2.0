using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class Lokace
    {
        public string Nazev { get; private set; }
        public int Id { get; private set; }

        public Lokace(string nazev, int id)
        {
            Nazev = nazev;
            Id = id;
        }
    }
}
