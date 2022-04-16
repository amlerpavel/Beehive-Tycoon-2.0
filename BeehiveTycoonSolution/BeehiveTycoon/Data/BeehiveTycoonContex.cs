using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BeehiveTycoon.Models;
using BeehiveTycoon.Db;

namespace BeehiveTycoon.Data
{
    public class BeehiveTycoonContex : DbContext, IDbUzivatele
    {
        public BeehiveTycoonContex(DbContextOptions<BeehiveTycoonContex> options) : base(options) { }

        public DbSet<Uzivatel> Uzivatele { get; private set; }

        void IDbUzivatele.PridatUzivatele(string jmeno, string heslo)
        {
            Uzivatele.Add(new Uzivatel(jmeno, heslo));
            SaveChanges();
        }

        Uzivatel IDbUzivatele.NajitUzivatele(string jmeno)
        {
            Uzivatel uzivatel = Uzivatele.Where(u => u.Jmeno == jmeno).FirstOrDefault();

            return uzivatel;
        }
    }
}
