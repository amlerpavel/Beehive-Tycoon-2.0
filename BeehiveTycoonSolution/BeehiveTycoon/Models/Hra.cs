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
            foreach(Ul ul in Uly)
                ul.DalsiKolo(Datum.CisloMesice);

            int posleniUl = Uly.Count - 1;

            if (Uly[posleniUl].VyrojitUl == true)
            {
                Ul ul = Uly[posleniUl].Vyrojit();
                Uly.Add(ul);
            }

            Datum.ZmenaData();
        }
    }
}
