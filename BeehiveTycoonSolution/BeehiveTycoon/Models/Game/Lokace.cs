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
        public Pyl Pyl { get; private set; }
        public int PNepratel { get; private set; } // v procentech, upravuje pocet utocicich nepratel na ul
        public int MaximalniOchrana { get; private set; } // upravuje max sanci na vytvoreni nepritele, kterou muzou vcely ziskat
        public int PZtraceneVcely { get; private set; } // procento, kolik vcel se ztrati za jedno kolo

        public Lokace(string nazev, int id, Pyl pyl, int pNepratel, int maximalniOchrana, int pZtraceneVcely)
        {
            Nazev = nazev;
            Id = id;
            Pyl = pyl;
            PNepratel = pNepratel;
            MaximalniOchrana = maximalniOchrana;
            PZtraceneVcely = pZtraceneVcely;
        }

        /*
        lokace:
            Les
                -hodně nepratel
                -hodně medu, do května

            Louka
                -vice nepratel
                -hodně medu, od května

            Město
                -nekolik vcel zemre
                -malo medu
                -mene nepratel

            Pole
                -nekolik vcel zemre
                -hodne pylu v urcitou dobu, jinak malo

            Sousedova zahrada
                -neutralni lokace
         */
    }

    public class Pyl
    {
        public Vystkyt Vystkyt { get; private set; }
        public MnostviNaVcelu MnostviNaVcelu { get; private set; }

        public Pyl(Vystkyt vystkyt, MnostviNaVcelu mnostviNaVcelu)
        {
            Vystkyt = vystkyt;
            MnostviNaVcelu = mnostviNaVcelu;
        }

        public int ZiskatMed(int vcely, int cisloMesice, int pMedu)
        {
            double med = 0;

            if (Vystkyt.Nejvice.Contains(cisloMesice))
                med = vcely * MnostviNaVcelu.Nejvice;
            else if (Vystkyt.Sezona.Contains(cisloMesice))
                med = vcely * MnostviNaVcelu.Sezona;
            else if (Vystkyt.Mene.Contains(cisloMesice))
                med = vcely * MnostviNaVcelu.Mene;
            else if (Vystkyt.Malo.Contains(cisloMesice))
                med = vcely * MnostviNaVcelu.Malo;

            med += med / 100 * pMedu;

            return Convert.ToInt32(med);
        }
    }

    public class Vystkyt
    {
        public int[] Nejvice { get; private set; }
        public int[] Sezona { get; private set; }
        public int[] Mene { get; private set; }
        public int[] Malo { get; private set; }

        public Vystkyt(int[] nejvice, int[] sezona, int[] mene, int[] malo)
        {
            Nejvice = nejvice;
            Sezona = sezona;
            Mene = mene;
            Malo = malo;
        }
    }
    public class MnostviNaVcelu
    {
        public double Nejvice { get; private set; }
        public double Sezona { get; private set; }
        public double Mene { get; private set; }
        public double Malo { get; private set; }

        public MnostviNaVcelu(double nejvice, double sezona, double mene, double malo)
        {
            Nejvice = nejvice;
            Sezona = sezona;
            Mene = mene;
            Malo = malo;
        }
    }
}
