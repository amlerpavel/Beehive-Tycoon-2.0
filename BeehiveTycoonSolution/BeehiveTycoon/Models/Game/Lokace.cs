using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class Lokace
    {
        public string Nazev { get; private set; }
        public int Id { get; private set; }

        public Lokace(string nazev, int id)
        {
            Nazev = nazev;
            Id = id;
        }

        /*
        lokace:
            Les
                -hodně nepratel
                -hodně medu, do května

            Louka
                -vice nepratel
                -hodně medu, od května

            Město
                -nekolik vcel zemre
                -malo medu
                -mene nepratel

            Pole
                -nekolik vcel zemre
                -hodne pylu v urcitou dobu, jinak malo

            Sousedova zahrada
                -neutralni lokace
         */
    }
}
