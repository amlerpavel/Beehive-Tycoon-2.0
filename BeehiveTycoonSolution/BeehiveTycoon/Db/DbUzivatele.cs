﻿using System;
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
    }

    public class DbUzivatele
    {
        private IDbUzivatele _databaze;

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
    }
}