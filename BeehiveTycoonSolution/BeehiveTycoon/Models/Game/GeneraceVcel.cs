using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class GeneraceVcel
    {
        public int _pocet { get; set; }
        public double _vek { get; set; }

        public void Zestarnout(double oKolik)
        {
            _vek += oKolik;
        }
    }
}
