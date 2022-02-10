﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class Ukol
    {
        public int Id { get; private set; }
        public string Nazev { get; private set; }
        public int PocetVcel { get; private set; }
        public int PocetVajicek { get; private set; }
        public int PocetMedu { get; private set; }
        public int PocetPlastvi { get; private set; }
        public int Platnost { get; private set; }

        public Ukol()
        {
            
        }
    }
}
