using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Game
{
    public class Obtiznost
    {
        public int Id { get; private set; }
        public string Nazev { get; private set; }
        public int PMedu { get; private set; }
        public int PNepratel { get; private set; }
        public int PMaxOchrana { get; private set; }
        public int PZtraceneVcely { get; private set; }

        public Obtiznost(int id, string nazev, int pMedu, int pNepratel, int pMaxOchrana, int pZtraceneVcely)
        {
            Id = id;
            Nazev = nazev;
            PMedu = pMedu;
            PNepratel = pNepratel;
            PMaxOchrana = pMaxOchrana;
            PZtraceneVcely = pZtraceneVcely;
        }
    }
}
