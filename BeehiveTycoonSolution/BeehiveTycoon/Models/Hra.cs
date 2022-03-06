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
        public Ul Ul { get; private set; }
        
        public Hra(Datum datum, Ul ul)
        {
            Datum = datum;
            Ul = ul;
        }

        public void Dalsikolo()
        {
            Ul.DalsiKolo(Datum.CisloMesice);
            Datum.ZmenaData();
        }
    }
}
