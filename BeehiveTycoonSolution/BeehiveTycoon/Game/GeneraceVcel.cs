using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Game
{
    public class GeneraceVcel
    {
        public int Pocet { get; private set; }
        public double Vek { get; private set; }

        public GeneraceVcel(int pocet, double vek)
        {
            Pocet = pocet;
            Vek = vek;
        }

        public void Zestarnout(double oKolik)
        {
            Vek += oKolik;
        }

        public int OdstranitVcely(int vcely)
        {
            Pocet -= vcely;

            if (Pocet < 0)
                return Pocet * (-1);
            else
                return 0;
        }
    }
}
