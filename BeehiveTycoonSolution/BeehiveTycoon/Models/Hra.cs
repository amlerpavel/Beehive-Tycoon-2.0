﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models.Game;
using System.Diagnostics;

namespace BeehiveTycoon.Models
{
    public class Hra
    {
        public Datum Datum { get; private set; }
        public List<Ul> Uly { get; private set; }
        public bool Vyhra { get; private set; }
        public bool Prohra { get; private set; }

        public Hra(Datum datum, List<Ul> uly, bool vyhra, bool prohra)
        {
            Datum = datum;
            Uly = uly;
            Vyhra = vyhra;
            Prohra = prohra;
        }

        public void Dalsikolo()
        {
            if(Vyhra == false && Prohra == false)
            {
                foreach (Ul ul in Uly.Where(u => u.Vcelstvo <= 0).ToArray())
                    Uly.Remove(ul);

                foreach (Ul ul in Uly)
                    ul.DalsiKolo(Datum.CisloMesice);

                if (Uly.Count == Uly.Where(u => u.Vcelstvo <= 0).ToArray().Length)
                    Prohra = true;
                else if (Uly.Count == 5)
                    Vyhra = true;

                Ul posleniUl = Uly.Last();
                
                if (posleniUl.VyrojitUl == true)
                {
                    Ul ul = posleniUl.Vyrojit();
                    Uly.Add(ul);
                }

                Datum.ZmenaData();
            }
        }
    }
}
