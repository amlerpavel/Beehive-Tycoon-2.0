using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeehiveTycoon.Models.Game
{
    public class Plastev
    {
        public int Med { get; private set; }

        public readonly int MaxMedu = 1000;

        public Plastev(int med)
        {
            Med = med;
        }

        public int PridatMed(int med)
        {
            if (med >= MaxMedu)
            {
                Med = MaxMedu;
                med -= MaxMedu;
            }
            else
            {
                if (med < 0)
                    Med = 0;
                else
                {
                    Med = med;
                    med = 0;
                }
            }

            return med;
        }
    }
}
