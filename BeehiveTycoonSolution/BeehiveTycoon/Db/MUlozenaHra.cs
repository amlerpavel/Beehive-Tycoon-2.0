using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Db
{
    public class MUlozenaHra
    {
        public int Pozice { get; private set; }
        public DateTime Datum { get; private set; }
        public string JmenoUzivatele { get; private set; }

        public MUlozenaHra(int pozice, DateTime datum, string jmenoUzivatele)
        {
            Pozice = pozice;
            Datum = datum;
            JmenoUzivatele = jmenoUzivatele;
        }
    }
}
