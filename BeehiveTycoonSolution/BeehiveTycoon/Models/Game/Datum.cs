using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class Datum
    {
        public int Rok { get; private set; }
        public int CisloMesice { get; private set; }
        public string Mesic { get; private set; }

        private readonly string[] _mesice = { "Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen", "Listopad", "Prosinec" };

        public void ZmenaData()
        {
            
            CisloMesice += 1;

            if (CisloMesice == 13)
            {
                CisloMesice = 1;
                Rok += 1;
            }

            Mesic = _mesice[CisloMesice - 1];
        }

        public Datum(int cisloMesice, int rok)
        {
            CisloMesice = cisloMesice;
            Rok = rok;
            Mesic = _mesice[CisloMesice - 1];
        }
    }
}
