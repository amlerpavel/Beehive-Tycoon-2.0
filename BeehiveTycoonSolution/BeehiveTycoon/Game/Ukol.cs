using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Game
{
    public class Ukol
    {
        public int Id { get; private set; }
        public string Nazev { get; private set; }
        public Podrobnost[] Podrobnosti { get; private set; }

        public Ukol(int id, string nazev, Podrobnost[] podrobnosti)
        {
            Id = id;
            Nazev = nazev;
            Podrobnosti = podrobnosti;
        }
    }

    public class Podrobnost
    {
        public string Jmeno { get; private set; }
        public int Hodnota { get; private set; }

        public Podrobnost(string jmeno, int hodnota)
        {
            Jmeno = jmeno;
            Hodnota = hodnota;
        }
    }

    public class DataUkolu
    {
        public int Id { get; private set; }
        public int Hodnota { get; private set; }
        public int IdLokaceUlu { get; private set; }

        public DataUkolu(int id, int hodnota, int idLokaceUlu)
        {
            Id = id;
            Hodnota = hodnota;
            IdLokaceUlu = idLokaceUlu;
        }
    }
}
