﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BeehiveTycoon.Models;
using BeehiveTycoon.Db;

namespace BeehiveTycoon.Data
{
    public class BeehiveTycoonContex : DbContext, IDbUzivatele, IDbUlozeneHry
    {
        public BeehiveTycoonContex(DbContextOptions<BeehiveTycoonContex> options) : base(options) { }

        public DbSet<Uzivatel> Uzivatele { get; set; }
        public DbSet<UlozenaHra> UlozeneHry { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UlozenaHra>().HasOne(u => u.Uzivatel).WithMany(u => u.UlozeneHry);
        }

        void IDbUzivatele.PridatUzivatele(string jmeno, string heslo)
        {
            Uzivatele.Add(new Uzivatel { Jmeno = jmeno, Heslo = heslo });
            SaveChanges();
        }

        Uzivatel IDbUzivatele.NajitUzivatele(string jmeno)
        {
            return Uzivatele.Where(u => u.Jmeno == jmeno).FirstOrDefault(); ;
        }

        MUzivatel IDbUzivatele.NajitMUzivatele(string jmeno)
        {
            Uzivatel uzivatel = NajitUzivatele(jmeno);

            List<MUlozenaHra> rozehraneHry = new();

            foreach (UlozenaHra ulozena in uzivatel.UlozeneHry)
                rozehraneHry.Add(new(ulozena.Pozice, ulozena.Datum, ulozena.Uzivatel.Jmeno));

            return new MUzivatel(uzivatel.Jmeno, rozehraneHry);
        }

        private Uzivatel NajitUzivatele(string jmeno)
        {
            return Uzivatele.First(u => u.Jmeno == jmeno);
        }

        void IDbUlozeneHry.AktualizovatHru(string postup, int pozice, string jmenoUzivatele)
        {
            UlozeneHry.First(u => (u.Uzivatel.Jmeno == jmenoUzivatele && u.Pozice == pozice)).Postup = postup;
            SaveChanges();
        }

        void IDbUlozeneHry.PridatHru(string postup, int slot, string jmenoUzivatele)
        {
            UlozeneHry.Add(new UlozenaHra { Postup = postup, Pozice = slot, Datum = DateTime.Now, Uzivatel = NajitUzivatele(jmenoUzivatele) });
            SaveChanges();
        }

        string IDbUlozeneHry.ZiskatPostup(int pozice, string jmenoUzivatele)
        {
            UlozenaHra ulozenaHra = NajitHru(pozice, jmenoUzivatele);

            if (ulozenaHra == null)
                return null;

            return ulozenaHra.Postup;
        }

        void IDbUlozeneHry.SmazatHru(int pozice, string jmenoUzivatele)
        {
            UlozenaHra ulozenaHra = NajitHru(pozice, jmenoUzivatele);
            
            if(ulozenaHra != null)
            {
                UlozeneHry.Remove(ulozenaHra);
                SaveChanges();
            }
        }

        private UlozenaHra NajitHru(int pozice, string jmenoUzivatele)
        {
            return UlozeneHry.FirstOrDefault(u => u.Uzivatel.Jmeno == jmenoUzivatele && u.Pozice == pozice);
        }
    }
}
