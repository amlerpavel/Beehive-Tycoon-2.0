using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeehiveTycoon.Data;
using BeehiveTycoon.Models;

namespace BeehiveTycoon.Db
{
    public interface IDbUzivatele
    {
        public void PridatUzivatele(string jmeno, string heslo);
        public Uzivatel NajitUzivatele(string jmeno);
        public MUzivatel NajitMUzivatele(string jmeno);
    }

    public class DbUzivatele
    {
        private readonly IDbUzivatele _databaze;

        public DbUzivatele(IDbUzivatele databaze)
        {
            _databaze = databaze;
        }

        public string PridatNovehoUzivatele(string jmeno, string heslo)
        {
            if (_databaze.NajitUzivatele(jmeno) != null)
                return "Toto jméno používá jiný uzivatel.";

            _databaze.PridatUzivatele(jmeno, BCrypt.Net.BCrypt.HashPassword(heslo));

            return "pridan";
        }

        public bool OveritUzivatele(string jmeno, string heslo)
        {
            Uzivatel uzivatel = _databaze.NajitUzivatele(jmeno);

            if (uzivatel == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(heslo, uzivatel.Heslo);
        }

        public MUzivatel ZiskatUzivatele(string jmeno)
        {
            MUzivatel uzivatel = _databaze.NajitMUzivatele(jmeno);

            return uzivatel;
        }
    }
}
