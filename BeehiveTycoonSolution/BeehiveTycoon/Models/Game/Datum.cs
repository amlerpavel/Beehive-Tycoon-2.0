using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class Datum
    {
        public int _rok { get; set; }
        public int _cisloMesice { get; set; }
        public string _mesic { get; set; }

        private string[] _mesice = { "Leden", "Únor", "Březen", "Duben", "Květen", "Červen", "Červenec", "Srpen", "Září", "Říjen", "Listopad", "Prosinec" };

        public void ZmenaData()
        {
            
            _cisloMesice += 1;

            if (_cisloMesice == 13)
            {
                _cisloMesice = 1;
                _rok += 1;
            }

            _mesic = _mesice[_cisloMesice - 1];
        }

        public void NoveDatum(int cisloMesice, int rok)
        {
            _cisloMesice = cisloMesice;
            _rok = rok;
            _mesic = _mesice[_cisloMesice - 1];
        }
    }
}
