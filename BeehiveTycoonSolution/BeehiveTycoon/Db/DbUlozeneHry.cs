using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Models;

namespace BeehiveTycoon.Db
{
    public interface IDbUlozeneHry
    {
        public void AktualizovatHru(string postup, int pozice, string jmenoUzivatele);
        public void PridatHru(string postup, int pozice, string jmenoUzivatele);
        public string ZiskatPostup(int pozice, string jmenoUzivatele);
        public void SmazatHru(int pozice, string jmenoUzivatele);
    }

    public class DbUlozeneHry
    {
        private readonly IDbUlozeneHry _databaze;

        public DbUlozeneHry(IDbUlozeneHry databaze)
        {
            _databaze = databaze;
        }

        public void AktualizovatHru(string postup, int pozice, string jmenoUzivatele)
        {
            _databaze.AktualizovatHru(postup, pozice, jmenoUzivatele);
        }

        public void PridatHru(string postup, int pozice, string jmenoUzivatele)
        {
            _databaze.SmazatHru(pozice, jmenoUzivatele);

            _databaze.PridatHru(postup, pozice, jmenoUzivatele);
        }

        public string ZiskatPostup(int pozice, string jmenoUzivatele)
        {
            return _databaze.ZiskatPostup(pozice, jmenoUzivatele);
        }

        public void SmazatHru(int pozice, string jmenoUzivatele)
        {
            _databaze.SmazatHru(pozice, jmenoUzivatele);
        }
    }
}
