using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeehiveTycoon.Game
{
    public class Hra
    {
        public Datum Datum { get; private set; }
        public List<Ul> Uly { get; private set; }
        public bool Vyhra { get; private set; }
        public bool Prohra { get; private set; }
        public Obtiznost Obtiznost { get; private set; }

        public Hra(Datum datum, List<Ul> uly, bool vyhra, bool prohra, Obtiznost obtiznost)
        {
            Datum = datum;
            Uly = uly;
            Vyhra = vyhra;
            Prohra = prohra;
            Obtiznost = obtiznost;
        }

        public void Dalsikolo()
        {
            if(Vyhra == false && Prohra == false)
            {
                foreach (Ul ul in Uly.Where(u => u.Vcelstvo <= 0).ToArray())
                    Uly.Remove(ul);

                foreach (Ul ul in Uly)
                    ul.DalsiKolo(Datum.CisloMesice, Obtiznost);

                if (Uly.Count == Uly.Where(u => u.Vcelstvo <= 0).ToArray().Length)
                    Prohra = true;
                else if (Uly.Where(u => u.Med == (u.MaxPlastvi * 1000)).ToArray().Length == 5)
                    Vyhra = true;

                if (Vyhra == false && Prohra == false)
                {
                    Ul nejmladsiZivyUl = Uly.Where(u => u.Vcelstvo > 0).ToArray().Last();

                    if (nejmladsiZivyUl.VyrojitUl == true)
                    {
                        Ul ul = nejmladsiZivyUl.Vyrojit();
                        Uly.Add(ul);
                    }
                    Debug.WriteLine(nejmladsiZivyUl.Lokace.Nazev);
                    if (nejmladsiZivyUl.BylVyrojenUl == true)
                        Uly.Where(u => u.Vcelstvo > 0).Last().PovolitZnovuRoj();
                }

                Datum.ZmenaData();
            }
        }
    }
}
