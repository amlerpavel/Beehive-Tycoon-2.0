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
        public List<MDokoncenaHra> NajitDohraneHryU(string jmenoUzivatele);
        public void PrepsatDohranouHru(string jmenoUzivatele, int obtiznostId, int rok, int mesic);
        public List<MDokoncenaHra> NajitDohraneHryO(int obtiznostId);
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
            MDokoncenaHra dokoncenaHra = _databaze.NajitDohraneHryU(jmenoUzivatele).FirstOrDefault(d => d.ObtiznostId == obtiznostId);

            if (dokoncenaHra == null)
                _databaze.PridatDohranouHru(jmenoUzivatele, obtiznostId, rok, mesic);
            else
            {
                if (rok <= dokoncenaHra.Rok && mesic < dokoncenaHra.Mesic)
                    _databaze.PrepsatDohranouHru(jmenoUzivatele, obtiznostId, rok, mesic);
            }
        }
        public List<MDokoncenaHra> ZiskatDohraneHry(int obtiznostId)
        {
            List<MDokoncenaHra> dokonceneHry = _databaze.NajitDohraneHryO(obtiznostId);

            return dokonceneHry.OrderBy(d => d.Rok).ThenBy(d => d.Mesic).ThenBy(d => d.Datum).ToList();
        }
    }
}
