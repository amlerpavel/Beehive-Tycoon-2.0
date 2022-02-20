using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models
{
    public class Hra
    {
        public Ul Ul { get; set; }
        public Nepritel Nepritel { get; set; }
        public List<GeneraceVcelstva> GeneraceVcelstev { get; set; }
        public List<Plastev0> Plastve { get; set; }
        public List<Ukol> SeznamUkolu { get; set; }
        public bool Vyhra { get; set; }
    }

    public struct Ul
    {
        public int Vcelstvo { get; set; }
        public int Med { get; set; }
        public string Mesic { get; set; }
    }

    public struct GeneraceVcelstva
    {
        public int Pocet { get; set; }
        public int Vek { get; set; }
    }

    public struct Ukol
    {
        public int IdUkolu { get; set; }
        public string Nazev { get; set; }
        public int PocetVcel { get; set; }
        public int PocetVajicek { get; set; }
        public int PocetMedu { get; set; }
        public int PocetPlastvi { get; set; }
        public int Platnost { get; set; }
    }

    public struct Pozadavek
    {
        public string Jmeno { get; set; }
        public int Pocet { get; set; }
    }

    public struct Plastev0
    {
        public int Med { get; set; }
        public int Vajicka { get; set; }
    }

    public struct Nepritel
    {
        public int IdNepritele { get; set; }
        public string JmenoNepritele { get; set; }
        public int PocetNepratel { get; set; }
        public int HPNepritele { get; set; }
        public int DalsiUtok { get; set; }
    }
}
