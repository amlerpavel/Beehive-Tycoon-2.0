using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Db
{
    public class MDokoncenaHra
    {
        public int ObtiznostId { get; private set; }
        public int Rok { get; private set; }
        public int Mesic { get; private set; }
        public DateTime Datum { get; private set; }
        public string JmenoUzivatele { get; private set; }

        public MDokoncenaHra(int obtiznostId, int rok, int mesic, DateTime datum, string jmenoUzivatele)
        {
            ObtiznostId = obtiznostId;
            Rok = rok;
            Mesic = mesic;
            Datum = datum;
            JmenoUzivatele = jmenoUzivatele;
        }
    }
}
