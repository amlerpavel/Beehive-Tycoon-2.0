using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models.Game;
using System.Diagnostics;

namespace BeehiveTycoon.Models
{
    public class Hra0
    {
        public Datum Datum { get; private set; }
        public Ul0 Ul0 { get; private set; }
        
        public Hra0(Datum datum, Ul0 ul0)
        {
            Datum = datum;
            Ul0 = ul0;
        }

        public void Dalsikolo()
        {
            Datum.ZmenaData();
            Ul0.DalsiKolo(Datum.CisloMesice);
        }
    }
}
