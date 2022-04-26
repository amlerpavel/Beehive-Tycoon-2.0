using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Db
{
    public class MUzivatel
    {
        public string Jmeno { get; private set; }
        public virtual List<MUlozenaHra> UlozeneHry { get; private set; }

        public MUzivatel(string jmeno, List<MUlozenaHra> ulozeneHry)
        {
            Jmeno = jmeno;
            UlozeneHry = ulozeneHry;
        }
    }
}
