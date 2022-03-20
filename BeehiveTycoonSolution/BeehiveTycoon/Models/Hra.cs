using System;
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

        public Hra(Datum datum, List<Ul> uly)
        {
            Datum = datum;
            Uly = uly;
        }

        public void Dalsikolo()
        {
            foreach (Ul ul in Uly.Where(u => u.Vcelstvo <= 0).ToArray())
                Uly.Remove(ul);
            
            foreach (Ul ul in Uly)
                ul.DalsiKolo(Datum.CisloMesice);

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
