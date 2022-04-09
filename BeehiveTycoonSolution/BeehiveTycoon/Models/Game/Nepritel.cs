using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BeehiveTycoon.Models.Game
{
    public class Nepritel
    {
        public int Id { get; private set; }
        public string Jmeno { get; private set; }
        public int Pocet { get; private set; }
        public int Zivoty { get; private set; }
        public int ZivotJedince { get; private set; }
        public int Vek { get; private set; }
        public bool VrazednyUder { get; private set; }
        public bool Porazen { get; private set; }

        private readonly Random _nahodneCislo = new();

        private int _strazci;
        private int _zivotyStrazcu;
        private int _zivotStrazce;

        public Nepritel(int id, string jmeno, int pocet, int zivoty, int zivotJedince, int vek, bool vrazednyUder, bool porazen)
        {
            Id = id;
            Jmeno = jmeno;
            Pocet = pocet;
            Zivoty = zivoty;
            ZivotJedince = zivotJedince;
            Vek = vek;
            VrazednyUder = vrazednyUder;
            Porazen = porazen;
        }

        public void Invaze(int vcelstvo, int viceNepratel)
        {
            double min = 0;
            double max = 1;

            if (VrazednyUder == true)
            {
                if (Id == 3)
                {
                    min = vcelstvo / 8;
                    max = vcelstvo / 4;
                }
                else if (Id == 4)
                {
                    min = vcelstvo / 2;
                    max = (vcelstvo / 100) * 20 + vcelstvo;
                }

                VrazednyUder = false;
            }
            else
            {
                if (Id == 5)
                {
                    min = 2;
                    max = 7;
                }
                else if (Id == 3 || Id == 4)
                {
                    min = 4;
                    max = 28;
                }
            }

            min += min / 100 * viceNepratel;
            max += max / 100 * viceNepratel;

            Pocet += _nahodneCislo.Next(Convert.ToInt32(min), Convert.ToInt32(max));

            if (Id != 0)
                Zivoty = Pocet * ZivotJedince;

            Vek += 1;
        }
        public int Boj(int strazci, int zivotyStrazcu, int zivotStrazce)
        {
            _strazci = strazci;
            _zivotyStrazcu = zivotyStrazcu;
            _zivotStrazce = zivotStrazce;

            if (Id == 1 || Id == 2)
                Souboj1v1(new int[] { 2, 11 }, zivotStrazce);
            else if (Id == 3)
                Souboj(new int[] { 2, 9 }, new int[] { 40, 61 });
            else if (Id == 4)
                Souboj(new int[] { 20, 41 }, new int[] { 10, 31 });
            else if (Id == 5)
                Souboj1v1(new int[] { 1, 2 }, 0);

            return strazci - _strazci;
        }

        private void Souboj(int[] utokVcel, int[] utokNepritele)
        {
            while (Pocet > 0 && _strazci > 0)
            {
                Zivoty -= _strazci * _nahodneCislo.Next(utokVcel[0], utokVcel[1]);
                _zivotyStrazcu -= Pocet * _nahodneCislo.Next(utokNepritele[0], utokNepritele[1]);
                Pocet = Convert.ToInt32(Math.Ceiling(Zivoty / Convert.ToDouble(ZivotJedince)));
                _strazci = Convert.ToInt32(Math.Ceiling(_zivotyStrazcu / Convert.ToDouble(_zivotStrazce)));
            }

            if (Pocet <= 0)
                Porazen = true;
        }
        private void Souboj1v1(int[] utokVcel, int utokNepritele)
        {
            while (Pocet > 0 && _strazci > 0)
            {
                Zivoty -= _nahodneCislo.Next(utokVcel[0], utokVcel[1]);
                _zivotyStrazcu -= utokNepritele;
                Pocet = Convert.ToInt32(Math.Ceiling(Zivoty / Convert.ToDouble(ZivotJedince)));
                _strazci = Convert.ToInt32(Math.Ceiling(_zivotyStrazcu / Convert.ToDouble(_zivotStrazce)));
            }

            if (Pocet <= 0)
                Porazen = true;
        }
    }
}
