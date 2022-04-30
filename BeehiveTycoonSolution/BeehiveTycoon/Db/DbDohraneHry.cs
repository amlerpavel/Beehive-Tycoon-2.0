using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeehiveTycoon.Db
{
    public interface IDbDohraneHry
    {
        public void PridatDohranouHru(string jmenoUzivatele, int obtiznostId, int rok, int mesic);
        public List<MDokoncenaHra> NajitDohraneHry(string jmenoUzivatele);
        public void PrepsatDohranouHru(string jmenoUzivatele, int obtiznostId, int rok, int mesic);
    }

    public class DbDohraneHry
    {
        private readonly IDbDohraneHry _databaze;

        public DbDohraneHry(IDbDohraneHry databaze)
        {
            _databaze = databaze;
        }

        public void Aktualizovat(string jmenoUzivatele, int obtiznostId, int rok, int mesic)
        {
            MDokoncenaHra dokoncenaHra = _databaze.NajitDohraneHry(jmenoUzivatele).FirstOrDefault(d => d.ObtiznostId == obtiznostId);

            if (dokoncenaHra == null)
                _databaze.PridatDohranouHru(jmenoUzivatele, obtiznostId, rok, mesic);
            else
            {
                Debug.WriteLine(rok + "<=" + dokoncenaHra.Rok);
                Debug.WriteLine(mesic + "<" + dokoncenaHra.Mesic);
                if (rok <= dokoncenaHra.Rok && mesic < dokoncenaHra.Mesic)
                    _databaze.PrepsatDohranouHru(jmenoUzivatele, obtiznostId, rok, mesic);
            }
        }
    }
}
